using UrbanCareClient.Domain.DTOs;
using UrbanCareClient.Domain.Interfaces.Repositories;

namespace UrbanCareClient.Application.Services.ApiServices
{
    public class DispatcherService
    {
        private readonly IDispatcherRepository _dispatcherRepository;

        public DispatcherService(IDispatcherRepository dispatcherRepository)
        {
            _dispatcherRepository = dispatcherRepository;
        }

        public async Task<List<ExecutorResponseDTO>?> GetCompanyExecutors(int companyId) =>
            await _dispatcherRepository.GetCompanyExecutorsAsync(companyId);
    }
}
