using UrbanCareClient.Domain.DTOs;
using UrbanCareClient.Domain.Interfaces.Repositories;

namespace UrbanCareClient.Application.Services.ApiServices
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDataResponseDTO?> GetMyUserData()
        {
            return await _userRepository.GetMyUserDataAsync();
        }

        public async Task<List<ManagementCompanyResponseDTO>> GetAllManagementCompanies()
        {
            var managementCompanies = await _userRepository.GetAllManagementCompaniesAsync();

            if (managementCompanies == null)
                throw new Exception();

            return managementCompanies;
        }


        public async Task<bool> CheckIsEmployee(int userId)
        {
            return await _userRepository.CheckIsEmployeeAsync(userId);
        }
    }
}
