namespace UrbanCareClient.Domain.Commands
{
    public record UpdateOrderFromResident(int Id,
        string Description,
        int OrderCategoryId,
        int BuildingId,
        int? ApartmentId,
        int PriorityId,
        string ContactPhone,
        string ContactEmail);
}
