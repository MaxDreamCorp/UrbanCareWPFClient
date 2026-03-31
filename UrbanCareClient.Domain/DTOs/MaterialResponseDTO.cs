namespace UrbanCareClient.Domain.DTOs
{
    public record MaterialResponseDTO(
        int Id,
        string Name,
        string Unit,
        decimal Price);
}
