using UrbanCareClient.Domain.Commands;
using UrbanCareClient.Domain.DTOs;
using UrbanCareClient.Domain.Interfaces.Repositories;
using UrbanCareClient.Infrastructure.Api;

namespace UrbanCareClient.Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApiClient _apiClient;
        private const string ENDPOINT_CONTROLLER = "user/";

        public UserRepository(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<bool> CheckIsEmployeeAsync(int userId, CancellationToken cancellationToken = default)
        {
            var parametrs = new Dictionary<string, string>() { { nameof(userId), userId.ToString() } };
            var response = await _apiClient
                .GetAsync<bool>($"{ENDPOINT_CONTROLLER}check_is_employee", parametrs, cancellationToken);
            return response.Data;
        }

        public async Task<List<ManagementCompanyResponseDTO>?> GetAllManagementCompaniesAsync(CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.GetAsync<List<ManagementCompanyResponseDTO>>($"{ENDPOINT_CONTROLLER}get_all_management_companies");
            return response.Data;
        }

        public async Task<UserDataResponseDTO?> GetMyUserDataAsync(CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.GetAsync<UserDataResponseDTO>($"{ENDPOINT_CONTROLLER}get_my_user_data");
            return response.Data;
        }

    }
}
