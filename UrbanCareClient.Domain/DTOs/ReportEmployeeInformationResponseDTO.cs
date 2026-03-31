namespace UrbanCareClient.Domain.DTOs
{
    public record ReportEmployeeInformationResponseDTO(
        int Id,
        string Fullname,
        string Email,
        string Phone,
        string Position,
        string Category,
        string Status,
        int ExperienceYears,
        int Salary,
        string? Notes);
}
