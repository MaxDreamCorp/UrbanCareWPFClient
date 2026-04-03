using UrbanCareClient.Domain.DTOs;
using UrbanCareClient.Domain.Interfaces.Repositories;
using UrbanCareClient.Infrastructure.Api;

namespace UrbanCareClient.Infrastructure.Data.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApiClient _apiClient;
        private const string ENDPOINT_CONTROLLER = "company/";

        public CompanyRepository(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<RoleResponseDTO>> GetRolesAsync(CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.GetAsync<List<RoleResponseDTO>>($"{ENDPOINT_CONTROLLER}get_roles", null, cancellationToken);


            if (response.Data == null)
                throw new Exception("Ошибка запроса");

            return response.Data;
        }

        public async Task<(List<ApartmentResponseDTO>? apartments, List<string>? errors)> GetApartmentsByManagmentCompanyAsync(int managementCompanyId, CancellationToken cancellationToken = default)
        {
            var parametrs = new Dictionary<string, string>() {
                {"managementCompanyId", managementCompanyId.ToString()},
            };
            var response = await _apiClient.GetAsync<List<ApartmentResponseDTO>?>($"{ENDPOINT_CONTROLLER}apartment/get_apartments_by_management_company", parametrs, cancellationToken);

            return (response.Data, response.Errors?.Select(e => e.Message).ToList());
        }

        public async Task<(List<BuildingResponseDTO>? buildings, List<string>? errors)> GetBuildingsByManagementCompanyAsync(int managementCompanyId, CancellationToken cancellationToken = default)
        {
            var parametrs = new Dictionary<string, string>() {
                {"managementCompanyId", managementCompanyId.ToString()},
            };
            var response = await _apiClient.GetAsync<List<BuildingResponseDTO>?>($"{ENDPOINT_CONTROLLER}building/get_buildings_by_management_company", parametrs, cancellationToken);

            return (response.Data, response.Errors?.Select(e => e.Message).ToList());
        }

        public async Task<List<BuildingTypeResponseDTO>> GetBuildingTypesAsync(CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.GetAsync<List<BuildingTypeResponseDTO>>($"{ENDPOINT_CONTROLLER}building/get_building_types", null, cancellationToken);

            if (response.Data == null)
                return new();

            return response.Data;
        }

        public async Task<List<FloorMaterialResponseDTO>> GetFloorMaterialsAsync(CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.GetAsync<List<FloorMaterialResponseDTO>>($"{ENDPOINT_CONTROLLER}building/get_floor_materials", null, cancellationToken);

            if (response.Data == null)
                return new();

            return response.Data;
        }

        public async Task<(List<RegionResponseDTO>? regions, List<string>? errors)> GetRegionsByManagementCompanyAsync(int managementCompanyId, CancellationToken cancellationToken = default)
        {
            var parametrs = new Dictionary<string, string>() {
                {"managementCompanyId", managementCompanyId.ToString()},
            };
            var response = await _apiClient.GetAsync<List<RegionResponseDTO>?>($"{ENDPOINT_CONTROLLER}region/get_regions_by_management_company", parametrs, cancellationToken);

            return (response.Data, response.Errors?.Select(e => e.Message).ToList());

        }

        public async Task<List<WallMaterialResponseDTO>> GetWallMaterialsAsync(CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.GetAsync<List<WallMaterialResponseDTO>>($"{ENDPOINT_CONTROLLER}building/get_wall_materials", null, cancellationToken);

            if (response.Data == null)
                return new();

            return response.Data;
        }

        public async Task<(List<RegionResponseDTO> regions, List<string>? errors)> GetAllRegionsAsync(CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.GetAsync<List<RegionResponseDTO>>($"{ENDPOINT_CONTROLLER}region/get_all_regions", null, cancellationToken);

            if (response.Data == null)
                return new(new(), new() { "Ошибка запроса" });

            return (response.Data, response.Errors?.Select(e => e.Message).ToList());
        }

        public async Task<(List<BuildingResponseDTO> buildings, List<string>? errors)> GetBuildingsByRegionAsync(int regionId, CancellationToken cancellationToken = default)
        {
            var parametrs = new Dictionary<string, string>() {
                {"regionId", regionId.ToString()},
            };
            var response = await _apiClient.GetAsync<List<BuildingResponseDTO>>($"{ENDPOINT_CONTROLLER}building/get_buildings_by_region", parametrs, cancellationToken);

            if (response.Data == null)
                return new(new(), new() { "Ошибка запроса" });

            return (response.Data, response.Errors?.Select(e => e.Message).ToList());
        }

        public async Task<(List<ApartmentResponseDTO> apartments, List<string>? errors)> GetApartmentsByBuildingAsync(int buildingId, CancellationToken cancellationToken = default)
        {
            var parametrs = new Dictionary<string, string>() {
                {"buildingId", buildingId.ToString()},
            };
            var response = await _apiClient.GetAsync<List<ApartmentResponseDTO>>($"{ENDPOINT_CONTROLLER}apartment/get_apartments_by_building", parametrs, cancellationToken);

            if (response.Data == null)
                return new(new(), new() { "Ошибка запроса" });

            return (response.Data, response.Errors?.Select(e => e.Message).ToList());
        }
    }
}
