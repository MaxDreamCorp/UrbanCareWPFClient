using UrbanCareClient.Domain.DTOs;

namespace UrbanCareClient.Domain.Interfaces.Repositories
{
    public interface ITestRepository
    {
        Task<TestDTO?> CheckRequesting(CancellationToken cancellationToken = default);
    }
}
