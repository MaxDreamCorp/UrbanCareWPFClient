namespace UrbanCareClient.Domain.Commands
{
    public record CreateRegionCommand(int ManagementCompanyId,
        string Name,
        string CommonAddress);
}
