namespace UrbanCareClient.Domain.DTOs
{
    public record MaterialResponseDTO(
        int Id,
        StorageResponseDTO Storage,
        string Name,
        string Unit,
        decimal Price,
        int AmountAtStorage);
}
