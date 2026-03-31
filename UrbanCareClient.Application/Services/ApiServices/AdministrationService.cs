using UrbanCareClient.Domain.Commands;
using UrbanCareClient.Domain.DTOs;
using UrbanCareClient.Domain.Interfaces.Repositories;

namespace UrbanCareClient.Application.Services.ApiServices
{
    public class AdministrationService
    {
        private readonly IAdministrationRepository _administrationRepository;

        public AdministrationService(IAdministrationRepository administrationRepository)
        {
            _administrationRepository = administrationRepository;
        }


        public async Task<ManagementCompanyResponseDTO?> GetMyManagementCompany() =>
            await _administrationRepository.GetMyManagementCompanyAsync();

        public async Task<List<ReportEmployeeInformationResponseDTO>?> GetMyManagementCompanyEmployees() =>
            await _administrationRepository.GetMyManagementCompanyEmployeesAsync();

        public async Task<List<string>?> CreateRegion(CreateRegionCommand cmd) =>
            await _administrationRepository.CreateRegionAsync(cmd);

        public async Task<List<string>?> UpdateRegion(UpdateRegionCommand cmd) =>
            await _administrationRepository.UpdateRegionAsync(cmd);

        public async Task<(bool isDeleted, string? error)> DeleteRegion(int regionId) =>
            await _administrationRepository.DeleteRegionAsync(regionId);

        public async Task<List<string>?> CreateBuilding(CreateBuildingCommand cmd) =>
            await _administrationRepository.CreateBuildingAsync(cmd);

        public async Task<List<string>?> UpdateBuilding(UpdateBuildingCommand cmd) =>
            await _administrationRepository.UpdateBuildingAsync(cmd);

        public async Task<(bool isDeleted, string? error)> DeleteBuilding(int buildingId) =>
            await _administrationRepository.DeleteBuildingAsync(buildingId);

        public async Task<List<string>?> CreateApartment(CreateApartmentCommand cmd) =>
            await _administrationRepository.CreateApartmentAsync(cmd);

        public async Task<List<string>?> UpdateApartment(UpdateApartmentCommand cmd) =>
            await _administrationRepository.UpdateApartmentAsync(cmd);

        public async Task<(bool isDeleted, string? error)> DeleteApartment(int apartmentId) =>
            await _administrationRepository.DeleteApartmentAsync(apartmentId);

    }
}
