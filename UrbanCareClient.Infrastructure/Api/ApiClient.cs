using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using UrbanCareClient.Application.Security;
using UrbanCareClient.Domain.DTOs;
using UrbanCareClient.Domain.ValueObjects;

namespace UrbanCareClient.Infrastructure.Api
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly SecureTokenStorage _secureTokenStorage;

        public ApiClient(HttpClient httpClient, SecureTokenStorage secureTokenStorage)
        {
            _httpClient = httpClient;

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _secureTokenStorage = secureTokenStorage;
            LoadToken();
        }

        private void LoadToken()
        {
            var token = _secureTokenStorage.LoadToken();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }


        public async Task<(bool isDeleted, string? error)> DeleteAsync(string endpoint, int id, CancellationToken cancellationToken = default)
        {
            try
            {
                LoadToken();

                var response = await _httpClient.DeleteAsync($"{endpoint}/{id}", cancellationToken);

                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    return new(true, null);
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return new(false, "Элемент не найден");
                else
                    return new(false, null);
            }
            catch (Exception ex)
            {
                return new(false, ex.Message);
            }
        }

        public async Task<ApiResponse<T>> GetAsync<T>(string endpoint, Dictionary<string, string>? queryParams = null, CancellationToken cancellationToken = default)
        {
            try
            {
                LoadToken();

                var url = queryParams != null
                    ? endpoint + QueryHelper.AddParamsFromDictionary(queryParams)
                    : endpoint;

                var response = await _httpClient.GetAsync(url, cancellationToken);

                var res = await HandleResponse<T>(response, cancellationToken);
                if (res == null)
                    throw new Exception("Get request must returns any response content");

                return res;
            }
            catch (Exception ex)
            {
                return ApiResponse<T>.GetError(new() { new("", ex.Message) });
            }
        }

        public async Task<ApiResponse<TResponse>?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken cancellationToken = default)
        {
            try
            {
                LoadToken();

                var response = await _httpClient.PostAsJsonAsync(endpoint, data, _jsonSerializerOptions, cancellationToken);

                var res = await HandleResponse<TResponse>(response, cancellationToken);
                return res;
            }
            catch (Exception ex)
            {
                return ApiResponse<TResponse>.GetError(new() { new("", ex.Message) });
            }
        }

        public async Task<ApiResponse<TResponse>?> PutAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken cancellationToken = default)
        {
            try
            {
                LoadToken();

                var response = await _httpClient.PutAsJsonAsync(endpoint, data, _jsonSerializerOptions, cancellationToken);

                var res = await HandleResponse<TResponse>(response, cancellationToken);
                return res;
            }
            catch (Exception ex)
            {
                return ApiResponse<TResponse>.GetError(new() { new("", ex.Message) });
            }
        }


        private async Task<ApiResponse<T>?> HandleResponse<T>(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                // TODO: handle unauth response

            }

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var data = await response.Content.ReadFromJsonAsync<T>(_jsonSerializerOptions, cancellationToken);
                    return ApiResponse<T>.Success(data);
                }
                catch (Exception)
                {
                    return null;
                }
            }

            try
            {
                var errors = await response.Content.ReadFromJsonAsync<List<ErrorDTO>>(_jsonSerializerOptions, cancellationToken);
                if (errors == null) throw new();
                return ApiResponse<T>.GetError(errors, response.StatusCode);
            }
            catch (Exception)
            {
                var error = await response.Content.ReadAsStringAsync(cancellationToken);
                return ApiResponse<T>.GetError(new() { new("", error) }, response.StatusCode);
            }
        }
    }
}
