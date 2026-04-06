using UrbanCareClient.Domain.Commands;
using UrbanCareClient.Domain.DTOs;
using UrbanCareClient.Domain.Interfaces.Repositories;
using UrbanCareClient.Infrastructure.Api;

namespace UrbanCareClient.Infrastructure.Data.Repositories
{
    public class DispatcherRepository : IDispatcherRepository
    {
        private readonly ApiClient _apiClient;
        private const string ENDPOINT_CONTROLLER = "dispatcher/";

        public DispatcherRepository(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<ExecutorResponseDTO>?> GetCompanyExecutorsAsync(int companyId, CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.GetAsync<List<ExecutorResponseDTO>?>($"{ENDPOINT_CONTROLLER}get_company_executors/{companyId}", null, cancellationToken);
            if (response == null)
                throw new Exception("Ошибка запроса");

            return response.Data;
        }

        public async Task<List<OrderResponseDTO>> GetCompanyNewOrdersAsync(int companyId, CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.GetAsync<List<OrderResponseDTO>?>($"{ENDPOINT_CONTROLLER}get_company_new_orders/{companyId}", null, cancellationToken);
            if (response == null)
                throw new Exception("Ошибка запроса");

            if (response.Data == null)
                return new List<OrderResponseDTO>();
            return response.Data;
        }

        public async Task<List<OrderResponseDTO>> GetCompanyInProgressOrdersAsync(int companyId, CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.GetAsync<List<OrderResponseDTO>?>($"{ENDPOINT_CONTROLLER}get_company_in_progress_orders/{companyId}", null, cancellationToken);
            if (response == null)
                throw new Exception("Ошибка запроса");

            if (response.Data == null)
                return new List<OrderResponseDTO>();
            return response.Data;
        }

        public async Task<List<OrderResponseDTO>> GetCompanyOrdersAsync(int companyId, CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.GetAsync<List<OrderResponseDTO>?>($"{ENDPOINT_CONTROLLER}get_company_orders/{companyId}", null, cancellationToken);
            if (response == null)
                throw new Exception("Ошибка запроса");

            if (response.Data == null)
                return new List<OrderResponseDTO>();
            return response.Data;
        }

        public async Task<List<string>?> AppointExecutorToOrderAsync(AppointExecutorToOrderCommand cmd, CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.PutAsync<AppointExecutorToOrderCommand, List<string>?>($"{ENDPOINT_CONTROLLER}appoint_executor_to_order", cmd, cancellationToken);

            if (response == null)
                return null;

            if (response.Errors != null && response.Errors.Count > 0)
                return response.Errors.Select(e => e.Message).ToList();

            return null;
        }
    }
}
