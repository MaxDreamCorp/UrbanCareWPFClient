using UrbanCareClient.Domain.Commands;
using UrbanCareClient.Domain.DTOs;

namespace UrbanCareClient.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<bool> CheckIsEmployeeAsync(int userId, CancellationToken cancellationToken = default);
        Task<UserDataResponseDTO?> GetMyUserDataAsync(CancellationToken cancellationToken = default);
        Task<List<ManagementCompanyResponseDTO>?> GetAllManagementCompaniesAsync(CancellationToken cancellationToken = default);
    }
}
