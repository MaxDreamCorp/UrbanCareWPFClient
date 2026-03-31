using Microsoft.Extensions.DependencyInjection;

namespace UrbanCareClient.WPF.Services
{
    public class GetterDIServices
    {
        private readonly IServiceProvider _serviceProvider;

        public GetterDIServices(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T GetService<T>() where T : class
        {
            var service = _serviceProvider.GetRequiredService<T>();
            return service;
        }
    }
}
