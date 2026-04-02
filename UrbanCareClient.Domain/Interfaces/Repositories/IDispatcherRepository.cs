using UrbanCareClient.Domain.DTOs;

namespace UrbanCareClient.Domain.Interfaces.Repositories
{
    public interface IDispatcherRepository
    {
        Task<List<ExecutorResponseDTO>?> GetCompanyExecutorsAsync(int companyId, CancellationToken cancellationToken = default);
    }
}
