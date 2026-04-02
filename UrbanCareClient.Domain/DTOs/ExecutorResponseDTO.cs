namespace UrbanCareClient.Domain.DTOs
{
    public record ExecutorResponseDTO(
        EmployeeDataResponseDTO EmployeeData,
        int ActiveTasksCount,
        int FinishedTasksCount);
}
