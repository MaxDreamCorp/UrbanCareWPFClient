namespace UrbanCareClient.Domain.DTOs
{
    public record BuildingResponseDTO(
        int Id,
        string Number,
        string Address,
        RegionResponseDTO Region,
        BuildingTypeResponseDTO BuildingType,
        short YearBuit,
        int FloorCount,
        WallMaterialResponseDTO WallMaterial,
        FloorMaterialResponseDTO FloorMaterial);
}
