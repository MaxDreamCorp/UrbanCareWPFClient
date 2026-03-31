using UrbanCareClient.Domain.DTOs;
using UrbanCareClient.Domain.Enums;
using UrbanCareClient.WPF.ViewDTOs;

namespace UrbanCareClient.WPF.Services
{
    public static class ConverterService
    {
        public static RoleEnum? StringToRoleEnum(string str)
        {
            return str switch
            {
                "Разработчик" => RoleEnum.Developer,
                "Администратор" => RoleEnum.Admin,
                "Менеджер" => RoleEnum.Manager,
                "Диспетчер" => RoleEnum.Dispatcher,
                "Исполнитель" => RoleEnum.Executor,
                "Житель" => RoleEnum.Resident,
                _ => null
            };
        }


        public static RegionViewDTO RegionToViewDTO(RegionResponseDTO region) =>
            new RegionViewDTO(
                region.Id,
                region.Name,
                region.CommonAddress,
                region.ManagementCompany.Name);

        public static BuildingViewDTO BuildingToViewDTO(BuildingResponseDTO building) =>
            new BuildingViewDTO(
                building.Id,
                building.Number,
                $"{building.Region.CommonAddress}, {building.Address}",
                building.Region.Name,
                building.BuildingType.Type,
                building.YearBuit,
                building.FloorCount,
                building.WallMaterial.Name,
                building.FloorMaterial.Name);

        public static ApartmentViewDTO ApartmentToViewDTO(ApartmentResponseDTO apartment) =>
            new ApartmentViewDTO(
                            apartment.Id,
                            apartment.Number,
                            apartment.Building.Number,
                            apartment.Entrance,
                            apartment.Floor,
                            apartment.RoomCount,
                            apartment.IsFree ? "Свободна" : "Имеет жильца");

        public static List<RegionViewDTO> RegionsToViewDTOs(List<RegionResponseDTO>? regions) =>
            regions?.Select(RegionToViewDTO).ToList() ?? new List<RegionViewDTO>();

        public static List<BuildingViewDTO> BuildingsToViewDTOs(List<BuildingResponseDTO>? buildings) =>
            buildings?.Select(BuildingToViewDTO).ToList() ?? new List<BuildingViewDTO>();

        public static List<ApartmentViewDTO> ApartmentsToViewDTOs(List<ApartmentResponseDTO>? apartments) =>
            apartments?.Select(ApartmentToViewDTO).ToList() ?? new List<ApartmentViewDTO>();
    }
}
