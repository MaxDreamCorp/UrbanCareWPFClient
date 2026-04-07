namespace UrbanCareClient.Domain.Commands
{
    public record MarkAsCompletedByExecutorCommand(
        int ExecutorUserId,
        int OrderId,
        decimal WorkPayment);
}
