namespace UrbanCareClient.Domain.Commands
{
    public record CreateApartmentCommand(int Number,
        int BuildingId,
        int? Entrance,
        int Floor,
        int RoomCount);
}
