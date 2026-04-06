using UrbanCareClient.Domain.Interfaces.Repositories;

namespace UrbanCareClient.Application.Services.ApiServices
{
    public class ExecutorService
    {
        private readonly IExecutorRepository _executorRepository;

        public ExecutorService(IExecutorRepository executorRepository)
        {
            _executorRepository = executorRepository;
        }

        public async Task<List<string>?> UpdateStatusToAvailable() =>
            await _executorRepository.UpdateStatusToAvailableAsync();

        public async Task<List<string>?> UpdateStatusToOnOrder() =>
            await _executorRepository.UpdateStatusToOnOrderAsync();
    }
}
