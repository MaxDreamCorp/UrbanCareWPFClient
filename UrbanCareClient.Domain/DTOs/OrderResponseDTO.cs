namespace UrbanCareClient.Domain.DTOs
{
    public record OrderResponseDTO(
        int Id,
        ResidentResponseDTO Resident,
        string Description,
        OrderCategoryResponseDTO OrderCategory,
        BuildingResponseDTO Building,
        ApartmentResponseDTO? Apartment,
        PriorityResponseDTO Priority,
        string ContactPhone,
        string ContactEmail,
        OrderStatusResponseDTO OrderStatus,
        DateTime CreatedAt,
        List<OrderMaterialResponseDTO>? OrderMaterials);
}
