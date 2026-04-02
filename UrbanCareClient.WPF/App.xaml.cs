using System.IO;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using UrbanCareClient.Application.Security;
using UrbanCareClient.Application.Services.ApiServices;
using UrbanCareClient.Domain.Interfaces.Repositories;
using UrbanCareClient.Infrastructure.Api;
using UrbanCareClient.Infrastructure.Data.Repositories;
using UrbanCareClient.Infrastructure.Options;
using UrbanCareClient.WPF.Interfaces;
using UrbanCareClient.WPF.Services;
using UrbanCareClient.WPF.Views.ModalWindows;
using UrbanCareClient.WPF.Views.UserControls;
using UrbanCareClient.WPF.Views.Windows;

namespace UrbanCareClient.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private readonly IHost _host;
        private readonly IConfiguration _configuration;

        public App()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    ConfigureServices(services);
                })
                .Build();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApiOptions>(_configuration.GetSection(nameof(ApiOptions)));


            services.AddTransient<LogInModalWindow>();
            services.AddTransient<RegistrationModalWindow>();
            services.AddTransient<EmployeeCreatingModalWindow>();
            services.AddTransient<ResidentCreatingModalWindow>();
            services.AddTransient<SingleChoiceControl>();
            services.AddTransient<DataChoiceModalWindow>();
            services.AddTransient<AdminWindow>();
            services.AddTransient<ResidentWindow>();
            services.AddTransient<DispatcherWindow>();

            services.AddSingleton<INavigationService>(provider => new NavigationService(provider));
            services.AddSingleton<GetterDIServices>(provider => new GetterDIServices(provider));
            services.AddSingleton<SecureTokenStorage>();

            services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAdministrationRepository, AdministrationRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IResidentRepository, ResidentRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IDispatcherRepository, DispatcherRepository>();

            services.AddScoped<AuthorizationService>();
            services.AddScoped<UserService>();
            services.AddScoped<AdministrationService>();
            services.AddScoped<EmployeeService>();
            services.AddScoped<CompanyService>();
            services.AddScoped<ResidentService>();
            services.AddScoped<OrderService>();
            services.AddScoped<DispatcherService>();

            services.AddHttpClient<ApiClient>((serviceProvider, client) =>
            {
                var apiOptions = serviceProvider.GetRequiredService<IOptions<ApiOptions>>().Value;
                client.BaseAddress = new Uri(apiOptions.TestApiRootUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("User-Agent", "UrbanCareClient/1.0");

            });

            services.AddScoped<TestService>();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            var logInWindow = _host.Services.GetRequiredService<LogInModalWindow>();
            logInWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync();
            }
            base.OnExit(e);
        }
    }

}
