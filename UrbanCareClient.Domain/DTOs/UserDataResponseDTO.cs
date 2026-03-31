namespace UrbanCareClient.Domain.DTOs
{
    public record UserDataResponseDTO(
        int Id,
        string Fullname,
        string Email,
        string Phone,
        int RoleId,
        DateOnly DateOfBirth);
}
