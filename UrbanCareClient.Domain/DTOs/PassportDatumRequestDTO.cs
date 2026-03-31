namespace UrbanCareClient.Domain.DTOs
{
    public record PassportDatumRequestDTO(
        string Seria,
        string Number,
        string Department,
        string DepartmentCode);
}
