namespace UrbanCareClient.Domain.Commands
{
    public record UpdateRegionCommand(int Id,
        string Name,
        string CommonAddress,
        int ManagementCompanyId);
}
