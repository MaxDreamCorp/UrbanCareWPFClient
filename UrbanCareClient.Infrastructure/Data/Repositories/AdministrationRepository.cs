using UrbanCareClient.Domain.Commands;
using UrbanCareClient.Domain.DTOs;
using UrbanCareClient.Domain.Interfaces.Repositories;
using UrbanCareClient.Infrastructure.Api;

namespace UrbanCareClient.Infrastructure.Data.Repositories
{
    public class AdministrationRepository : IAdministrationRepository
    {
        private readonly ApiClient _apiClient;
        private const string ENDPOINT_CONTROLLER = "administration/";

        public AdministrationRepository(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<ReportEmployeeInformationResponseDTO>?> GetMyManagementCompanyEmployeesAsync(CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.GetAsync<List<ReportEmployeeInformationResponseDTO>?>($"{ENDPOINT_CONTROLLER}management_company/get_management_company_employees");
            return response.Data;
        }

        public async Task<ManagementCompanyResponseDTO?> GetMyManagementCompanyAsync(CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.GetAsync<ManagementCompanyResponseDTO>($"{ENDPOINT_CONTROLLER}management_company/get_my_management_company");
            return response.Data;
        }

        public async Task<List<string>?> CreateRegionAsync(CreateRegionCommand cmd, CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.PostAsync<CreateRegionCommand, bool>($"{ENDPOINT_CONTROLLER}region/create_region", cmd, cancellationToken);

            if (response == null)
                return new() { "Ошибка запроса" };

            if (response.Errors != null)
                return response.Errors.Select(x => x.Message).ToList();

            return null;
        }

        public async Task<List<string>?> UpdateRegionAsync(UpdateRegionCommand cmd, CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.PutAsync<UpdateRegionCommand, bool>($"{ENDPOINT_CONTROLLER}region/update_region", cmd, cancellationToken);

            if (response == null)
                return new() { "Ошибка запроса" };

            if (response.Data == false)
                return new() { "Не удалось изменить регион" };

            if (response.Errors != null)
                return response.Errors.Select(x => x.Message).ToList();

            return null;
        }

        public async Task<(bool isDeleted, string? error)> DeleteRegionAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _apiClient.DeleteAsync($"{ENDPOINT_CONTROLLER}region/delete_region", id, cancellationToken);
        }

        public async Task<List<string>?> CreateBuildingAsync(CreateBuildingCommand cmd, CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.PostAsync<CreateBuildingCommand, bool>($"{ENDPOINT_CONTROLLER}building/create_building", cmd, cancellationToken);

            if (response == null)
                return new() { "Ошибка запроса" };

            if (response.Errors != null)
                return response.Errors.Select(x => x.Message).ToList();

            return null;
        }

        public async Task<List<string>?> UpdateBuildingAsync(UpdateBuildingCommand cmd, CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.PutAsync<UpdateBuildingCommand, bool>($"{ENDPOINT_CONTROLLER}building/update_building", cmd, cancellationToken);

            if (response == null)
                return new() { "Ошибка запроса" };

            if (response.Data == false)
                return new() { "Не удалось изменить здание" };

            if (response.Errors != null)
                return response.Errors.Select(x => x.Message).ToList();

            return null;
        }

        public async Task<(bool isDeleted, string? error)> DeleteBuildingAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _apiClient.DeleteAsync($"{ENDPOINT_CONTROLLER}building/delete_building", id, cancellationToken);
        }

        public async Task<List<string>?> CreateApartmentAsync(CreateApartmentCommand cmd, CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.PostAsync<CreateApartmentCommand, bool>($"{ENDPOINT_CONTROLLER}apartment/create_apartment", cmd, cancellationToken);

            if (response == null)
                return new() { "Ошибка запроса" };

            if (response.Errors != null)
                return response.Errors.Select(x => x.Message).ToList();

            return null;
        }

        public async Task<List<string>?> UpdateApartmentAsync(UpdateApartmentCommand cmd, CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.PutAsync<UpdateApartmentCommand, bool>($"{ENDPOINT_CONTROLLER}apartment/update_apartment", cmd, cancellationToken);

            if (response == null)
                return new() { "Ошибка запроса" };

            if (response.Data == false)
                return new() { "Не удалось изменить квартиру" };

            if (response.Errors != null)
                return response.Errors.Select(x => x.Message).ToList();

            return null;
        }

        public async Task<(bool isDeleted, string? error)> DeleteApartmentAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _apiClient.DeleteAsync($"{ENDPOINT_CONTROLLER}apartment/delete_apartment", id, cancellationToken);
        }
    }
}
