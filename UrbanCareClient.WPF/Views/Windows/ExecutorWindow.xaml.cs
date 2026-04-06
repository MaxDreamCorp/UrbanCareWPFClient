using System.Windows;
using System.Windows.Input;
using UrbanCareClient.Application.Services.ApiServices;
using UrbanCareClient.Application.Services.OtherServices;
using UrbanCareClient.Domain.Interfaces.Repositories;
using UrbanCareClient.WPF.Interfaces;
using UrbanCareClient.WPF.Services;
using UrbanCareClient.WPF.Views.ModalWindows;

namespace UrbanCareClient.WPF.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для ExecutorWindow.xaml
    /// </summary>
    public partial class ExecutorWindow : Window
    {
        private readonly GetterDIServices _getterDIServices;
        private readonly IExecutorRepository _executorRepository;
        private readonly OrderService _orderService;
        private readonly EmployeeService _employeeService;
        private readonly INavigationService _navigationService;
        private readonly UserService _userService;

        public ExecutorWindow(GetterDIServices getterDIServices, OrderService orderService, EmployeeService employeeService, INavigationService navigationService, UserService userService, IExecutorRepository executorRepository)
        {
            InitializeComponent();
            _getterDIServices = getterDIServices;
            _orderService = orderService;
            _employeeService = employeeService;
            _navigationService = navigationService;
            _userService = userService;
            _executorRepository = executorRepository;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var userData = await _userService.GetMyUserData();
            if (userData == null)
            {
                MessageBox.Show("Ошибка получения данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                var authWindow = _navigationService.GetWindow<LogInModalWindow>();
                authWindow.Show();
                Close();
                return;
            }
            TemporaryDataStorage.CurrentUserId = userData.Id;

            var employeeDataResponse = await _employeeService.GetMyEmployee();
            while (employeeDataResponse == null)
            {
                MessageBox.Show("Вам необходимо заполнить данные работника", "", MessageBoxButton.OK, MessageBoxImage.Information);
                EmployeeCreatingModalWindow employeeCreatingModalWindow = _getterDIServices.GetService<EmployeeCreatingModalWindow>();
                employeeCreatingModalWindow = _getterDIServices.GetService<EmployeeCreatingModalWindow>();
                employeeCreatingModalWindow.ShowDialog();
                employeeDataResponse = await _employeeService.GetMyEmployee();
            }

            TemporaryDataStorage.EmployeeData = employeeDataResponse;
            TemporaryDataStorage.ManagementCompany = employeeDataResponse.ManagementCompany;

            var response = await _executorRepository.UpdateStatusToAvailableAsync();
            if (response != null && response.Count > 0)
            {
                MessageBox.Show(string.Join("\n", response), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            }

            MCNameTxt.Text = $"УК: {TemporaryDataStorage.EmployeeData.ManagementCompany.Name}";
            FullNameTxt.Text = TemporaryDataStorage.EmployeeData.UserData.Fullname;
            PositionTxt.Text = TemporaryDataStorage.EmployeeData.EmployeePosition.Name;
        }

        private void ExecutorAppointedOrders_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ExecutorAppointedOrdersPanel.Visibility == Visibility.Visible)
            {
                ExecutorAppointedOrdersPanel.Visibility = Visibility.Collapsed;
                ExecutorAppointedOrderChevronUp.Visibility = Visibility.Collapsed;
                ExecutorAppointedOrderChevronDown.Visibility = Visibility.Visible;
            }
            else
            {
                ExecutorAppointedOrdersPanel.Visibility = Visibility.Visible;
                ExecutorAppointedOrderChevronUp.Visibility = Visibility.Visible;
                ExecutorAppointedOrderChevronDown.Visibility = Visibility.Collapsed;
            }
        }

        private void InProgressOrders_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (InProgressOrdersPanel.Visibility == Visibility.Visible)
            {
                InProgressOrdersPanel.Visibility = Visibility.Collapsed;
                InProgressOrderChevronUp.Visibility = Visibility.Collapsed;
                InProgressOrderChevronDown.Visibility = Visibility.Visible;
            }
            else
            {
                InProgressOrdersPanel.Visibility = Visibility.Visible;
                InProgressOrderChevronUp.Visibility = Visibility.Visible;
                InProgressOrderChevronDown.Visibility = Visibility.Collapsed;
            }
        }

        private void MarkedAsCompletedOrders_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (MarkedAsCompletedOrdersPanel.Visibility == Visibility.Visible)
            {
                MarkedAsCompletedOrdersPanel.Visibility = Visibility.Collapsed;
                MarkedAsCompletedOrderChevronUp.Visibility = Visibility.Collapsed;
                MarkedAsCompletedOrderChevronDown.Visibility = Visibility.Visible;
            }
            else
            {
                MarkedAsCompletedOrdersPanel.Visibility = Visibility.Visible;
                MarkedAsCompletedOrderChevronUp.Visibility = Visibility.Visible;
                MarkedAsCompletedOrderChevronDown.Visibility = Visibility.Collapsed;
            }
        }

        private void CompletedOrders_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (CompletedOrdersPanel.Visibility == Visibility.Visible)
            {
                CompletedOrdersPanel.Visibility = Visibility.Collapsed;
                CompletedOrderChevronUp.Visibility = Visibility.Collapsed;
                CompletedOrderChevronDown.Visibility = Visibility.Visible;
            }
            else
            {
                CompletedOrdersPanel.Visibility = Visibility.Visible;
                CompletedOrderChevronUp.Visibility = Visibility.Visible;
                CompletedOrderChevronDown.Visibility = Visibility.Collapsed;
            }

        }
    }
}
