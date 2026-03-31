namespace UrbanCareClient.Domain.DTOs
{
    public record PaymentResponseDTO(
        int Id,
        int OrderId,
        decimal Amount,
        PaymentMethodResponseDTO PaymentMethod,
        string PaymentCode,
        DateTime PaidAt);
}
