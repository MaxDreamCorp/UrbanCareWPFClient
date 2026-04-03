using UrbanCareClient.Domain.Commands;
using UrbanCareClient.Domain.DTOs;
using UrbanCareClient.Domain.Interfaces.Repositories;

namespace UrbanCareClient.Application.Services.ApiServices
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<List<string>?> CreateAdmin(CreateAdminCommand cmd) =>
            await _employeeRepository.CreateAdminAsync(cmd);

        public async Task<List<string>?> CreateDispatcher(CreateDispatcherCommand cmd) =>
            await _employeeRepository.CreateDispatcherAsync(cmd);

        public async Task<List<string>?> CreateExecutor(CreateExecutorCommand cmd) =>
            await _employeeRepository.CreateExecutorAsync(cmd);

        public async Task<List<EmployeePositionResponseDTO>> GetEmployeePositions()
        {
            return await _employeeRepository.GetEmployeePositionsAsync();
        }

        public async Task<List<EmployeeStatusResponseDTO>> GetEmployeeStatuses()
        {
            return await _employeeRepository.GetEmployeeStatusesAsync();
        }

        public async Task<List<QualificationCategoriesNamesResponseDTO>> GetQualificationCategoriesNames()
        {
            return await _employeeRepository.GetQualificationCategoriesNamesAsync();
        }

        public async Task<EmployeeDataResponseDTO?> GetMyEmployee()
        {
            return await _employeeRepository.GetMyEmployeeAsync();
        }
    }
}
