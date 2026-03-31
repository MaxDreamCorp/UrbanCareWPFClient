using UrbanCareClient.Domain.Commands;
using UrbanCareClient.Domain.DTOs;
using UrbanCareClient.Domain.Interfaces.Repositories;
using UrbanCareClient.Infrastructure.Api;

namespace UrbanCareClient.Infrastructure.Data.Repositories
{
    public class AuthorizationRepository : IAuthorizationRepository
    {
        private readonly ApiClient _apiClient;
        private const string ENDPOINT_CONTROLLER = "authorization/";

        public AuthorizationRepository(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<LogInResponseDTO?> LogInAsync(LogInCommand cmd, CancellationToken cancellationToken = default)
        {
            var response = await _apiClient
                .PostAsync<LogInCommand, LogInResponseDTO>
                ($"{ENDPOINT_CONTROLLER}log_in", cmd, cancellationToken);
            if (response == null) return null;

            if (response.Data != null && response.Errors == null)
                return response.Data;

            if (response.Errors != null)
                return new(null, -1, response.Errors);
            return null;
        }

        public async Task<RegistrationResponseDTO?> RegistrateAsync(RegistrationCommand cmd, CancellationToken cancellationToken = default)
        {
            var response = await _apiClient
                .PostAsync<RegistrationCommand, RegistrationResponseDTO>
                ($"{ENDPOINT_CONTROLLER}registrate", cmd, cancellationToken);
            if (response == null) return null;

            if (response.Data != null && response.Errors == null)
                return response.Data;

            if (response.Errors != null)
                return new(-1, response.Errors);
            return null;
        }
    }
}
