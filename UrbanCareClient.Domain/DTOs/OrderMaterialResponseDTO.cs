namespace UrbanCareClient.Domain.DTOs
{
    public record OrderMaterialResponseDTO(
    int Id,
    int OrderId,
    MaterialResponseDTO Material,
    int Quantity);
}
