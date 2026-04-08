namespace UrbanCareClient.Domain.Commands
{
    public record AddMaterialsToOrderCommand(
        int ExecutorUserId,
       int OrderId,
       Dictionary<int, int> Materials);
}
