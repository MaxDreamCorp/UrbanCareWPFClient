using UrbanCareClient.Domain.Commands;
using UrbanCareClient.Domain.DTOs;
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

        public async Task<List<OrderResponseDTO>> GetExecutorOrders() =>
            await _executorRepository.GetExecutorOrders();

        public async Task<List<string>?> AcceptOrder(int orderId) =>
            await _executorRepository.AcceptOrderAsync(orderId);

        public async Task<List<string>?> MarkAsCompleted(MarkAsCompletedByExecutorCommand cmd) =>
            await _executorRepository.MarkAsCompletedAsync(cmd);
    }
}
