namespace UrbanCareClient.Domain.DTOs
{
    public record EmployeeDataResponseDTO(int Id,
        int UserId,
        ManagementCompanyResponseDTO ManagementCompany,
        EmployeePositionResponseDTO EmployeePosition,
        EmployeeStatusResponseDTO EmployeeStatus,
        QualificationCategoryResponseDTO QualificationCategory,
        DateOnly EmploymentDate,
        int ExpereienceYears,
        int Salary,
        string? Notes);
}
