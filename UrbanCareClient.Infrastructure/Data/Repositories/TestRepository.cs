using UrbanCareClient.Domain.DTOs;
using UrbanCareClient.Domain.Interfaces.Repositories;
using UrbanCareClient.Infrastructure.Api;

namespace UrbanCareClient.Infrastructure.Data.Repositories
{
    public class TestRepository : ITestRepository
    {
        private readonly ApiClient _apiClient;

        public TestRepository(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<TestDTO?> CheckRequesting(CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.GetAsync<TestDTO>("test/check_requesting");
            return response.Data;
        }
    }
}
