using UrbanCareClient.Domain.DTOs;

namespace UrbanCareClient.Application.Services.OtherServices
{
    public static class TemporaryDataStorage
    {
        public static int CurrentUserId { get; set; }
        public static ManagementCompanyResponseDTO? ManagementCompany { get; set; }
        public static UserDataResponseDTO? UserData { get; set; }
        public static ResidentResponseDTO? ResidentData { get; set; }
        public static EmployeeDataResponseDTO? EmployeeData { get; set; }

        public static List<RoleResponseDTO> Roles { get; set; } = new List<RoleResponseDTO>();
        public static List<BuildingTypeResponseDTO> BuildingTypes { get; set; } = new List<BuildingTypeResponseDTO>();
        public static List<FloorMaterialResponseDTO> FloorMaterials { get; set; } = new List<FloorMaterialResponseDTO>();
        public static List<WallMaterialResponseDTO> WallMaterials { get; set; } = new List<WallMaterialResponseDTO>();

        public static List<OrderCategoryResponseDTO> OrderCategories { get; set; } = new List<OrderCategoryResponseDTO>();
        public static List<OrderTypeResponseDTO> OrderTypes { get; set; } = new List<OrderTypeResponseDTO>();
        public static List<OrderStatusResponseDTO> OrderStatuses { get; set; } = new List<OrderStatusResponseDTO>();
        public static List<PriorityResponseDTO> Priorities { get; set; } = new List<PriorityResponseDTO>();

        public static List<OrderResponseDTO>? MyOrders { get; set; }

        public static List<RegionResponseDTO>? Regions { get; set; }
        public static List<BuildingResponseDTO>? Buildings { get; set; }
        public static List<ApartmentResponseDTO>? Apartments { get; set; }
    }
}
