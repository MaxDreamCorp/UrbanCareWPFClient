using UrbanCareClient.Domain.Interfaces.Repositories;

namespace UrbanCareClient.Application.Services.ApiServices
{
    public class TestService
    {
        private readonly ITestRepository _repository;

        public TestService(ITestRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> CheckRequesting()
        {
            var response = await _repository.CheckRequesting();
            return response.Response;
        }
    }

}
