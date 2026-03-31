using UrbanCareClient.Application.Security;
using UrbanCareClient.Domain.Commands;
using UrbanCareClient.Domain.DTOs;
using UrbanCareClient.Domain.Interfaces.Repositories;

namespace UrbanCareClient.Application.Services.ApiServices
{
    public class AuthorizationService
    {
        private readonly IAuthorizationRepository _authorizationRepository;
        private readonly SecureTokenStorage _secureTokenStorage;

        public AuthorizationService(IAuthorizationRepository authorizationRepository, SecureTokenStorage secureTokenStorage)
        {
            _authorizationRepository = authorizationRepository;
            _secureTokenStorage = secureTokenStorage;
        }

        public async Task<(int userId, List<string>? errors)> RegistrateAsync(RegistrationCommand cmd)
        {
            var response = await _authorizationRepository.RegistrateAsync(cmd);
            if (response == null)
                return (-1, new() { "Ошибка запроса"});

            if (response.Errors != null)
                return new(-1, response.Errors.Select(e => e.Message).ToList());

            return (response.UserId, null);
        }

        public async Task<(int roleId, List<string>? errors)> LogInAsync(LogInCommand cmd)
        {
            var response = await _authorizationRepository.LogInAsync(cmd);
            if (response == null)
                return (-1, null);

            if (response.Errors != null)
                return (-1, response.Errors.Select(e => e.Message).ToList());

            if (response.Token != null)
                _secureTokenStorage.SaveToken(response.Token);

            return (response.RoleId, null);
        }

    }
}
