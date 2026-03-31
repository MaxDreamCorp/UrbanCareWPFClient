using UrbanCareClient.Domain.Commands;
using UrbanCareClient.Domain.DTOs;

namespace UrbanCareClient.Domain.Interfaces.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<ErrorDTO>?> CreateAdminAsync(CreateAdminCommand cmd, CancellationToken cancellationToken = default);
        Task<List<EmployeePositionResponseDTO>> GetEmployeePositionsAsync(CancellationToken cancellationToken = default);
        Task<List<EmployeeStatusResponseDTO>> GetEmployeeStatusesAsync(CancellationToken cancellationToken = default);
        Task<List<QualificationCategoriesNamesResponseDTO>> GetQualificationCategoriesNamesAsync(CancellationToken cancellationToken = default);
        Task<EmployeeDataResponseDTO?> GetMyEmployeeAsync(CancellationToken cancellationToken = default);
    }
}
