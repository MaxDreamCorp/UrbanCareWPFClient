namespace UrbanCareClient.Domain.DTOs
{
    public record QualificationCategoryResponseDTO(
        int Id,
        string Name,
        string Code,
        float MinExperienceYears,
        float SalaryCoefficient);
}
