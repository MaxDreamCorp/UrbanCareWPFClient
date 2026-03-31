using UrbanCareClient.Application.Services.OtherServices;
using UrbanCareClient.Domain.Commands;
using UrbanCareClient.Domain.DTOs;
using UrbanCareClient.Domain.Interfaces.Repositories;

namespace UrbanCareClient.Application.Services.ApiServices
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<List<string>?> CreateOrder(CreateOrderCommand cmd) =>
            await _orderRepository.CreateOrderAsync(cmd);

        public async Task<List<string>?> UpdateOrder(UpdateOrderFromResident cmd) =>
            await _orderRepository.UpdateOrderAsync(cmd);

        public async Task<List<OrderCategoryResponseDTO>> GetOrderCategories()
        {
            var categories = await _orderRepository.GetOrderCategoriesAsync();
            TemporaryDataStorage.OrderCategories = categories;
            var types = categories.Select(c => c.OrderType).DistinctBy(ot => ot.Type).ToList();
            TemporaryDataStorage.OrderTypes = types;
            return categories;
        }

        public async Task<List<OrderStatusResponseDTO>> GetOrderStatuses()
        {
            var statuses = await _orderRepository.GetOrderStatusesAsync();
            TemporaryDataStorage.OrderStatuses = statuses;
            return statuses;
        }

        public async Task<List<PriorityResponseDTO>> GetPriorities()
        {
            var priorities = await _orderRepository.GetPrioritiesAsync();
            TemporaryDataStorage.Priorities = priorities;
            return priorities;
        }
    }
}
