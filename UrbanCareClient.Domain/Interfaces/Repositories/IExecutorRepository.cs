namespace UrbanCareClient.Domain.Interfaces.Repositories
{
    public interface IExecutorRepository
    {
        Task<List<string>?> UpdateStatusToAvailableAsync(CancellationToken cancellationToken = default);
        Task<List<string>?> UpdateStatusToOnOrderAsync(CancellationToken cancellationToken = default);
    }
}
