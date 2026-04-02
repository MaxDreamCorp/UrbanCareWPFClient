using System.Windows;
using UrbanCareClient.Application.Services.ApiServices;
using UrbanCareClient.Application.Services.OtherServices;
using UrbanCareClient.WPF.Interfaces;
using UrbanCareClient.WPF.Services;
using UrbanCareClient.WPF.Views.ModalWindows;
using UrbanCareClient.WPF.Views.UserControls;

namespace UrbanCareClient.WPF.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для DispatcherWindow.xaml
    /// </summary>
    public partial class DispatcherWindow : Window
    {
        private readonly GetterDIServices _getterDIServices;
        private readonly DispatcherService _dispatcherService;
        private readonly EmployeeService _employeeService;
        private readonly INavigationService _navigationService;

        public DispatcherWindow(GetterDIServices getterDIServices, DispatcherService dispatcherService, EmployeeService employeeService, INavigationService navigationService)
        {
            InitializeComponent();
            _getterDIServices = getterDIServices;
            _dispatcherService = dispatcherService;
            _employeeService = employeeService;
            _navigationService = navigationService;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var employeeDataResponse = await _employeeService.GetMyEmployee();
            if (employeeDataResponse == null)
            {
                MessageBox.Show("Ошибка получения данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                var authWindow = _navigationService.GetWindow<LogInModalWindow>();
                authWindow.Show();
                Close();
                return;
            }

            TemporaryDataStorage.EmployeeData = employeeDataResponse;
            TemporaryDataStorage.ManagementCompany = employeeDataResponse.ManagementCompany;

            MCNameTxt.Text = $"УК: {TemporaryDataStorage.EmployeeData.ManagementCompany.Name}";
            FullNameTxt.Text = TemporaryDataStorage.EmployeeData.UserData.Fullname;
            PositionTxt.Text = TemporaryDataStorage.EmployeeData.EmployeePosition.Name;

            var executorsResponse = await _dispatcherService.GetCompanyExecutors(TemporaryDataStorage.ManagementCompany.Id);
            if (executorsResponse != null)
            {
                ExecutorsPanel.Children.Clear();

                foreach (var executor in executorsResponse.OrderBy(e => e.EmployeeData.EmployeeStatus.Id))
                {
                    var executorControl = new MiniExecutorControl()
                    {
                        ViewModal = new UserControls.ViewModels.MiniExecutorControlViewModal { Executor = executor }
                    };
                    ExecutorsPanel.Children.Add(executorControl);
                }
            }
        }
    }
}
