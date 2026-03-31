using UrbanCareClient.Domain.Commands;
using UrbanCareClient.Domain.DTOs;
using UrbanCareClient.Domain.Interfaces.Repositories;
using UrbanCareClient.Infrastructure.Api;

namespace UrbanCareClient.Infrastructure.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApiClient _apiClient;
        private const string ENDPOINT_CONTROLLER = "order/";

        public OrderRepository(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<string>?> CreateOrderAsync(CreateOrderCommand cmd, CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.PostAsync<CreateOrderCommand, bool>($"{ENDPOINT_CONTROLLER}create_order", cmd, cancellationToken);

            if (response == null)
                return new() { "Ошибка запроса" };

            if (response.Errors != null)
                return response.Errors.Select(x => x.Message).ToList();

            return null;
        }

        public async Task<List<OrderCategoryResponseDTO>> GetOrderCategoriesAsync(CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.GetAsync<List<OrderCategoryResponseDTO>>($"{ENDPOINT_CONTROLLER}get_order_categories", cancellationToken: cancellationToken);

            if (response == null)
                return new();

            return response.Data ?? new();
        }

        public async Task<List<OrderStatusResponseDTO>> GetOrderStatusesAsync(CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.GetAsync<List<OrderStatusResponseDTO>>($"{ENDPOINT_CONTROLLER}get_order_statuses", cancellationToken: cancellationToken);

            if (response == null)
                return new();

            return response.Data ?? new();
        }

        public async Task<List<PriorityResponseDTO>> GetPrioritiesAsync(CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.GetAsync<List<PriorityResponseDTO>>($"{ENDPOINT_CONTROLLER}get_priorities", cancellationToken: cancellationToken);

            if (response == null)
                return new();

            return response.Data ?? new();
        }

        public async Task<List<string>?> UpdateOrderAsync(UpdateOrderFromResident cmd, CancellationToken cancellationToken = default)
        {
            var response = await _apiClient.PostAsync<UpdateOrderFromResident, bool>($"{ENDPOINT_CONTROLLER}update_order", cmd, cancellationToken);

            if (response == null)
                return new() { "Ошибка запроса" };

            if (response.Errors != null)
                return response.Errors.Select(x => x.Message).ToList();

            return null;
        }
    }
}
