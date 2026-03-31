using UrbanCareClient.Domain.Commands;
using UrbanCareClient.Domain.DTOs;

namespace UrbanCareClient.Domain.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<List<string>?> CreateOrderAsync(CreateOrderCommand cmd, CancellationToken cancellationToken = default);
        Task<List<string>?> UpdateOrderAsync(UpdateOrderFromResident cmd, CancellationToken cancellationToken = default);
        Task<List<OrderCategoryResponseDTO>> GetOrderCategoriesAsync(CancellationToken cancellationToken = default);
        Task<List<OrderStatusResponseDTO>> GetOrderStatusesAsync(CancellationToken cancellationToken = default);
        Task<List<PriorityResponseDTO>> GetPrioritiesAsync(CancellationToken cancellationToken = default);
    }
}
