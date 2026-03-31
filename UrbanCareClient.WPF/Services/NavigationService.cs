using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using UrbanCareClient.WPF.Interfaces;

namespace UrbanCareClient.WPF.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T GetWindow<T>() where T : Window
        {
            var window = _serviceProvider.GetRequiredService<T>();
            return window;
        }
    }
}
