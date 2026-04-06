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
    }
}
