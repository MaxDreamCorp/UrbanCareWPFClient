using UrbanCareClient.Application.Services.OtherServices;
using UrbanCareClient.Domain.Commands;
using UrbanCareClient.Domain.DTOs;
using UrbanCareClient.Domain.Interfaces.Repositories;

namespace UrbanCareClient.Application.Services.ApiServices
{
    public class ResidentService
    {
        private readonly IResidentRepository _residentRepository;

        public ResidentService(IResidentRepository residentRepository)
        {
            _residentRepository = residentRepository;
        }

        public async Task<List<string>?> CreateResident(CreateResidentCommand cmd) =>
            await _residentRepository.CreateResidentAsync(cmd);

        public async Task<ResidentResponseDTO?> GetMyResidentData() =>
            await _residentRepository.GetMyResidentDataAsync();

        public async Task<List<OrderResponseDTO>> GetMyOrders()
        {
            var orders = await _residentRepository.GetMyOrdersAsync();
            TemporaryDataStorage.MyOrders = orders;
            return orders;
        }

        public async Task<List<string>?> ConfirmOrderCompletion(int orderId) =>
            await _residentRepository.ConfirmOrderCompletionAsync(orderId);

        public async Task<List<string>?> ImitatePayment(int orderId) =>
            await _residentRepository.ImitatePaymentAsync(orderId);
    }
}
