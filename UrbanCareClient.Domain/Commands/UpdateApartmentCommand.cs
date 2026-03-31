namespace UrbanCareClient.Domain.Commands
{
    public record UpdateApartmentCommand(int Id,
        int Number,
        int BuildingId,
        int? Entrance,
        int Floor,
        int RoomCount);
}
