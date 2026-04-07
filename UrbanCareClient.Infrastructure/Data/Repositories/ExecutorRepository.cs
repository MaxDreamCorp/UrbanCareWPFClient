using UrbanCareClient.Domain.Commands;
using UrbanCareClient.Domain.DTOs;
using UrbanCareClient.Domain.Interfaces.Repositories;
using UrbanCareClient.Infrastructure.Api;

namespace UrbanCareClient.Infrastructure.Data.Repositories
{
    public class ExecutorRepository : IExecutorRepository
    {
        private readonly ApiClient _apiClient;
        private const string ENDPOINT_CONTROLLER = "executor/";

        public ExecutorRepository(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<string>?> UpdateStatusToAvailableAsync(CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.PutAsync<string, List<string>?>($"{ENDPOINT_CONTROLLER}update_status_to_available", "", cancellationToken);

            if (response == null)
                return null;

            if (response.Errors != null && response.Errors.Count > 0)
                return response.Errors.Select(e => e.Message).ToList();
            return null;
        }

        public async Task<List<string>?> UpdateStatusToOnOrderAsync(CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.PutAsync<string, List<string>?>($"{ENDPOINT_CONTROLLER}update_status_to_on_order", "", cancellationToken);

            if (response == null)
                return null;

            if (response.Errors != null && response.Errors.Count > 0)
                return response.Errors.Select(e => e.Message).ToList();
            return null;
        }

        public async Task<List<OrderResponseDTO>> GetExecutorOrders(CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.GetAsync<List<OrderResponseDTO>>($"{ENDPOINT_CONTROLLER}get_executor_orders", null, cancellationToken);

            if (response == null)
                throw new Exception("Ошибка запроса");

            if (response.Data == null)
                return new List<OrderResponseDTO>();
            return response.Data;
        }

        public async Task<List<string>?> AcceptOrderAsync(int orderId, CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.PutAsync<string, List<string>?>($"{ENDPOINT_CONTROLLER}accept_order/{orderId}", "", cancellationToken);

            if (response == null)
                return null;

            if (response.Errors != null && response.Errors.Count > 0)
                return response.Errors.Select(e => e.Message).ToList();
            return null;
        }

        public async Task<List<string>?> MarkAsCompletedAsync(MarkAsCompletedByExecutorCommand cmd, CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.PutAsync<MarkAsCompletedByExecutorCommand, List<string>?>($"{ENDPOINT_CONTROLLER}mark_as_completed", cmd, cancellationToken);

            if (response == null)
                return null;

            if (response.Errors != null && response.Errors.Count > 0)
                return response.Errors.Select(e => e.Message).ToList();
            return null;
        }
    }
}
