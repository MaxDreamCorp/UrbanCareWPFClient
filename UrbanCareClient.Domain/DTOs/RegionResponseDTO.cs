namespace UrbanCareClient.Domain.DTOs
{
    public record RegionResponseDTO(int Id,
        string Name,
        string CommonAddress,
        ManagementCompanyResponseDTO ManagementCompany);
}
