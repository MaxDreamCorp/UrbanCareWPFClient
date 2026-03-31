namespace UrbanCareClient.Domain.Commands
{
    public record CreateBuildingCommand(string Number,
        string Address,
        int RegionId,
        int BuildingTypeId,
        short YearBuit,
        int FloorCount,
        int WallMaterialId,
        int FloorMaterialId);
}
