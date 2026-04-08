using UrbanCareClient.Domain.DTOs;

namespace UrbanCareClient.Domain.Interfaces.Repositories
{
    public interface ICompanyRepository
    {
        Task<List<RoleResponseDTO>> GetRolesAsync(CancellationToken cancellationToken = default);
        Task<List<BuildingTypeResponseDTO>> GetBuildingTypesAsync(CancellationToken cancellationToken = default);
        Task<List<FloorMaterialResponseDTO>> GetFloorMaterialsAsync(CancellationToken cancellationToken = default);
        Task<List<WallMaterialResponseDTO>> GetWallMaterialsAsync(CancellationToken cancellationToken = default);
        Task<(List<RegionResponseDTO>? regions, List<string>? errors)> GetRegionsByManagementCompanyAsync(int managementCompanyId, CancellationToken cancellationToken = default);
        Task<(List<BuildingResponseDTO>? buildings, List<string>? errors)> GetBuildingsByManagementCompanyAsync(int managementCompanyId, CancellationToken cancellationToken = default);
        Task<(List<ApartmentResponseDTO>? apartments, List<string>? errors)> GetApartmentsByManagmentCompanyAsync(int managementCompanyId, CancellationToken cancellationToken = default);
        Task<(List<RegionResponseDTO> regions, List<string>? errors)> GetAllRegionsAsync(CancellationToken cancellationToken = default);
        Task<(List<BuildingResponseDTO> buildings, List<string>? errors)> GetBuildingsByRegionAsync(int regionId, CancellationToken cancellationToken = default);
        Task<(List<ApartmentResponseDTO> apartments, List<string>? errors)> GetApartmentsByBuildingAsync(int buildingId, CancellationToken cancellationToken = default);
        Task<(List<MaterialResponseDTO> materials, List<string>? errors)> GetMaterialsByManagementCompanyAsync(int managementCompanyId, CancellationToken cancellationToken = default);
    }
}
