using UrbanCareClient.Application.Services.OtherServices;
using UrbanCareClient.Domain.DTOs;
using UrbanCareClient.Domain.Interfaces.Repositories;

namespace UrbanCareClient.Application.Services.ApiServices
{
    public class CompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<List<RoleResponseDTO>> GetRoles()
        {
            var roles = await _companyRepository.GetRolesAsync();
            TemporaryDataStorage.Roles = roles;
            return roles;
        }

        public async Task<List<BuildingTypeResponseDTO>> GetBuildingTypes()
        {
            var buildingTypes = await _companyRepository.GetBuildingTypesAsync();
            TemporaryDataStorage.BuildingTypes = buildingTypes; 
            return buildingTypes;
        }

        public async Task<List<FloorMaterialResponseDTO>> GetFloorMaterials()
        {
            var floorMaterials = await _companyRepository.GetFloorMaterialsAsync();
            TemporaryDataStorage.FloorMaterials = floorMaterials;
            return floorMaterials;
        }

        public async Task<List<WallMaterialResponseDTO>> GetWallMaterials()
        {
            var wallMaterials = await _companyRepository.GetWallMaterialsAsync();
            TemporaryDataStorage.WallMaterials = wallMaterials;
            return wallMaterials;
        }

        public async Task<(List<RegionResponseDTO>? regions, List<string>? errors)> GetRegionsByManagementCompany(int managementCompanyId)
        {
            var regions = await _companyRepository.GetRegionsByManagementCompanyAsync(managementCompanyId);
            TemporaryDataStorage.Regions = regions.regions;
            return regions;
        }

        public async Task<(List<BuildingResponseDTO>? buildings, List<string>? errors)> GetBuildingsByManagementCompany(int managementCompanyId)
        {
            var buildings = await _companyRepository.GetBuildingsByManagementCompanyAsync(managementCompanyId);
            TemporaryDataStorage.Buildings = buildings.buildings;
            return buildings;
        }

        public async Task<(List<ApartmentResponseDTO>? apartments, List<string>? errors)> GetApartmentsByManagmentCompany(int managementCompanyId)
        {
            var apartments = await _companyRepository.GetApartmentsByManagmentCompanyAsync(managementCompanyId);
            TemporaryDataStorage.Apartments = apartments.apartments;
            return apartments;
        }

        public async Task<(List<RegionResponseDTO> regions, List<string>? errors)> GetAllRegions()
        {
            return await _companyRepository.GetAllRegionsAsync();
        }

        public async Task<(List<BuildingResponseDTO> buildings, List<string>? errors)> GetBuildingsByRegion(int regionId)
        {
            return await _companyRepository.GetBuildingsByRegionAsync(regionId);
        }

        public async Task<(List<ApartmentResponseDTO> apartments, List<string>? errors)> GetApartmentsByBuilding(int buildingId)
        {
            return await _companyRepository.GetApartmentsByBuildingAsync(buildingId);
        }
    }
}
