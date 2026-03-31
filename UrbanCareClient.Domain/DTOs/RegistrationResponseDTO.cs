namespace UrbanCareClient.Domain.DTOs
{
    public record RegistrationResponseDTO(int UserId, List<ErrorDTO>? Errors);
}
