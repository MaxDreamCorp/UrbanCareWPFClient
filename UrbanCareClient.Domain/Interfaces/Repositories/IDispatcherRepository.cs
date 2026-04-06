using UrbanCareClient.Domain.Commands;
using UrbanCareClient.Domain.DTOs;

namespace UrbanCareClient.Domain.Interfaces.Repositories
{
    public interface IDispatcherRepository
    {
        Task<List<ExecutorResponseDTO>?> GetCompanyExecutorsAsync(int companyId, CancellationToken cancellationToken = default);
        Task<List<OrderResponseDTO>> GetCompanyOrdersAsync(int companyId, CancellationToken cancellationToken = default);
        Task<List<OrderResponseDTO>> GetCompanyNewOrdersAsync(int companyId, CancellationToken cancellationToken = default);
        Task<List<OrderResponseDTO>> GetCompanyInProgressOrdersAsync(int companyId, CancellationToken cancellationToken = default);
        Task<List<string>?> AppointExecutorToOrderAsync(AppointExecutorToOrderCommand cmd, CancellationToken cancellationToken = default);
    }
}
