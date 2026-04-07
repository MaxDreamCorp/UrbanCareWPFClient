using UrbanCareClient.Domain.Commands;
using UrbanCareClient.Domain.DTOs;

namespace UrbanCareClient.Domain.Interfaces.Repositories
{
    public interface IResidentRepository
    {
        Task<List<string>?> CreateResidentAsync(CreateResidentCommand cmd, CancellationToken cancellationToken = default);
        Task<ResidentResponseDTO?> GetMyResidentDataAsync(CancellationToken cancellationToken = default);
        Task<List<OrderResponseDTO>> GetMyOrdersAsync(CancellationToken cancellationToken = default);
        Task<List<string>?> ConfirmOrderCompletionAsync(int orderId, CancellationToken cancellationToken = default);
    }
}
