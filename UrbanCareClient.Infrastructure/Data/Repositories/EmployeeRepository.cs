using UrbanCareClient.Domain.Commands;
using UrbanCareClient.Domain.DTOs;
using UrbanCareClient.Domain.Interfaces.Repositories;
using UrbanCareClient.Infrastructure.Api;

namespace UrbanCareClient.Infrastructure.Data.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApiClient _apiClient;
        private const string ENDPOINT_CONTROLLER = "employee/";

        public EmployeeRepository(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<ErrorDTO>?> CreateAdminAsync(CreateAdminCommand cmd, CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.PostAsync<CreateAdminCommand, List<ErrorDTO>?>($"{ENDPOINT_CONTROLLER}create_admin", cmd, cancellationToken);

            if (response == null) return null;

            return response.Data;
        }

        public async Task<List<EmployeePositionResponseDTO>> GetEmployeePositionsAsync(CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.GetAsync<List<EmployeePositionResponseDTO>>($"{ENDPOINT_CONTROLLER}get_all_employee_positions");
            if (response.Data == null)
                return new();
            return response.Data;
        }

        public async Task<List<EmployeeStatusResponseDTO>> GetEmployeeStatusesAsync(CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.GetAsync<List<EmployeeStatusResponseDTO>>($"{ENDPOINT_CONTROLLER}get_all_employee_statuses");
            if (response.Data == null)
                return new();
            return response.Data;
        }

        public async Task<EmployeeDataResponseDTO?> GetMyEmployeeAsync(CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.GetAsync<EmployeeDataResponseDTO?>($"{ENDPOINT_CONTROLLER}get_my_employee");
            return response.Data;
        }

        public async Task<List<QualificationCategoriesNamesResponseDTO>> GetQualificationCategoriesNamesAsync(CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.GetAsync<List<QualificationCategoriesNamesResponseDTO>>($"{ENDPOINT_CONTROLLER}get_all_qualification_categories_names");
            if (response.Data == null)
                return new();
            return response.Data;
        }
    }
}
