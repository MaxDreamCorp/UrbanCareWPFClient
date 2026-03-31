namespace UrbanCareClient.Domain.DTOs
{
    public record UserPersonalDatumRequestDTO(
        PassportDatumRequestDTO PassportData,
        DateOnly DateOfBirth,
        string Snils,
        string Inn);
}
