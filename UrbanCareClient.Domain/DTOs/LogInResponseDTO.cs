namespace UrbanCareClient.Domain.DTOs
{
    public record LogInResponseDTO(string? Token, int RoleId, List<ErrorDTO>? Errors);
}
