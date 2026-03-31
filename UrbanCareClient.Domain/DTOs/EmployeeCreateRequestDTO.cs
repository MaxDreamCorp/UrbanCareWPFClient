namespace UrbanCareClient.Domain.DTOs
{
    public record EmployeeCreateRequestDTO(int UserId,
        int ManagementCompanyId,
        int EmployeePositionId,
        int QualificationCategoryId,
        DateOnly EmploymentDate,
        int ExpereienceYears,
        int Salary,
        string? Notes);
}
