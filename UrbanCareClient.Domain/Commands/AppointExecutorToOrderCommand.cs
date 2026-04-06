namespace UrbanCareClient.Domain.Commands
{
    public record AppointExecutorToOrderCommand(int OrderId,
       int DispatcherId,
       int ExecutorId);
}
