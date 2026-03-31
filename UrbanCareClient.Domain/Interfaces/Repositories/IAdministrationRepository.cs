using UrbanCareClient.Domain.Commands;
using UrbanCareClient.Domain.DTOs;

namespace UrbanCareClient.Domain.Interfaces.Repositories
{
    public interface IAdministrationRepository
    {
        Task<ManagementCompanyResponseDTO?> GetMyManagementCompanyAsync(CancellationToken cancellationToken = default);
        Task<List<ReportEmployeeInformationResponseDTO>?> GetMyManagementCompanyEmployeesAsync(CancellationToken cancellationToken = default);
        Task<List<string>?> CreateRegionAsync(CreateRegionCommand cmd, CancellationToken cancellationToken = default);
        Task<List<string>?> UpdateRegionAsync(UpdateRegionCommand cmd, CancellationToken cancellationToken = default);
        Task<(bool isDeleted, string? error)> DeleteRegionAsync(int id, CancellationToken cancellationToken = default);
        Task<List<string>?> CreateBuildingAsync(CreateBuildingCommand cmd, CancellationToken cancellationToken = default);
        Task<List<string>?> UpdateBuildingAsync(UpdateBuildingCommand cmd, CancellationToken cancellationToken = default);
        Task<(bool isDeleted, string? error)> DeleteBuildingAsync(int id, CancellationToken cancellationToken = default);
        Task<List<string>?> CreateApartmentAsync(CreateApartmentCommand cmd, CancellationToken cancellationToken = default);
        Task<List<string>?> UpdateApartmentAsync(UpdateApartmentCommand cmd, CancellationToken cancellationToken = default);
        Task<(bool isDeleted, string? error)> DeleteApartmentAsync(int id, CancellationToken cancellationToken = default);
    }
}
