using UrbanCareClient.Domain.Commands;
using UrbanCareClient.Domain.DTOs;

namespace UrbanCareClient.Domain.Interfaces.Repositories
{
    public interface IAuthorizationRepository
    {
        Task<RegistrationResponseDTO?> RegistrateAsync(RegistrationCommand cmd, CancellationToken cancellationToken = default);
        Task<LogInResponseDTO?> LogInAsync(LogInCommand cmd, CancellationToken cancellationToken = default);
    }
}
