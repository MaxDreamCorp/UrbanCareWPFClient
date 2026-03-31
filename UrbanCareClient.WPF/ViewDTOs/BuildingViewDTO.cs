namespace UrbanCareClient.WPF.ViewDTOs
{
    public record BuildingViewDTO(
        int Id,
        string Number,
        string Address,
        string Region,
        string BuildingType,
        short YearBuit,
        int FloorCount,
        string WallMaterial,
        string FloorMaterial);
}
