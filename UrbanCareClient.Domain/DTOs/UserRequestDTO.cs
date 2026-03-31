namespace UrbanCareClient.Domain.DTOs
{
    public record UserRequestDTO(
        string Fullname,
        string Email,
        string Phone,
        string Password,
        int RoleId,
        UserPersonalDatumRequestDTO UserPersonalData);
}
