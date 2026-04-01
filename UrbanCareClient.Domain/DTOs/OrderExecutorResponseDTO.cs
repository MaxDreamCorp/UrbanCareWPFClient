namespace UrbanCareClient.Domain.DTOs
{
    public record OrderExecutorResponseDTO(
       EmployeeDataResponseDTO Employee,
       decimal? WorkPayment);
}
