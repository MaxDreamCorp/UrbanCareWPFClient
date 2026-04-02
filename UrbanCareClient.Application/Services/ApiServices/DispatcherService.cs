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

        public async Task<List<OrderResponseDTO>> GetCompanyNewOrders(int companyId) =>
            await _dispatcherRepository.GetCompanyNewOrdersAsync(companyId);

        public async Task<List<OrderResponseDTO>> GetCompanyInProgressOrders(int companyId) =>
            await _dispatcherRepository.GetCompanyInProgressOrdersAsync(companyId);
    }
}
