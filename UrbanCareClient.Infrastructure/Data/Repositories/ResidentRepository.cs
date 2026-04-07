using UrbanCareClient.Domain.Commands;
using UrbanCareClient.Domain.DTOs;
using UrbanCareClient.Domain.Interfaces.Repositories;
using UrbanCareClient.Infrastructure.Api;

namespace UrbanCareClient.Infrastructure.Data.Repositories
{
    public class ResidentRepository : IResidentRepository
    {
        private readonly ApiClient _apiClient;
        private const string ENDPOINT_CONTROLLER = "resident/";

        public ResidentRepository(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<string>?> CreateResidentAsync(CreateResidentCommand cmd, CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.PostAsync<CreateResidentCommand, bool>($"{ENDPOINT_CONTROLLER}create_resident", cmd, cancellationToken);

            if (response == null)
                return new() { "Ошибка запроса" };

            return response.Errors?.Select(e => e.Message).ToList();
        }

        public async Task<ResidentResponseDTO?> GetMyResidentDataAsync(CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.GetAsync<ResidentResponseDTO?>($"{ENDPOINT_CONTROLLER}get_my_resident_data", null, cancellationToken);

            if (response == null)
                throw new Exception("Ошибка запроса");

            return response.Data;
        }

        public async Task<List<OrderResponseDTO>> GetMyOrdersAsync(CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.GetAsync<List<OrderResponseDTO>>($"{ENDPOINT_CONTROLLER}get_my_orders", null, cancellationToken);

            if (response == null)
                throw new Exception("Ошибка запроса");

            return response.Data ?? new();
        }

        public async Task<List<string>?> ConfirmOrderCompletionAsync(int orderId, CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.PutAsync<string, List<string>?>($"{ENDPOINT_CONTROLLER}confirm_order_completion/{orderId}", "", cancellationToken);

            if (response == null)
                return null;

            if (response.Errors != null && response.Errors.Count > 0)
                return response.Errors.Select(e => e.Message).ToList();

            return null;
        }
    }
}
