namespace UrbanCareClient.WPF.ViewDTOs
{
    public record ApartmentViewDTO(
        int Id,
        int Number,
        string Building,
        int? Entrance,
        int Floor,
        int RoomCount,
        string Status);
}
