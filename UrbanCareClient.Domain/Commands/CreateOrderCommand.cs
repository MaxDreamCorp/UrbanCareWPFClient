namespace UrbanCareClient.Domain.Commands
{
    public record CreateOrderCommand(
        int ResidentId,
        string Description,
        int OrderCategoryId,
        int BuildingId,
        int? ApartmentId,
        int PriorityId,
        string ContactPhone,
        string ContactEmail);
}
