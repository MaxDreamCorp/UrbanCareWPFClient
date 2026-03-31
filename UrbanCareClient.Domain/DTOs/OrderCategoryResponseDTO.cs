namespace UrbanCareClient.Domain.DTOs
{
    public record OrderCategoryResponseDTO(
       int Id,
       string Category,
       OrderTypeResponseDTO OrderType);
}