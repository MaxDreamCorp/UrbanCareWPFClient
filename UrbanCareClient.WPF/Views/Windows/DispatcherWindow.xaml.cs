using System.Windows;
using UrbanCareClient.Application.Services.ApiServices;
using UrbanCareClient.Application.Services.OtherServices;
using UrbanCareClient.Domain.Enums;
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
        private readonly OrderService _orderService;
        private readonly EmployeeService _employeeService;
        private readonly INavigationService _navigationService;
        private readonly UserService _userService;

        public DispatcherWindow(GetterDIServices getterDIServices, DispatcherService dispatcherService, EmployeeService employeeService, INavigationService navigationService, OrderService orderService, UserService userService)
        {
            InitializeComponent();
            _getterDIServices = getterDIServices;
            _dispatcherService = dispatcherService;
            _employeeService = employeeService;
            _navigationService = navigationService;
            _orderService = orderService;
            _userService = userService;
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

            var response = await _employeeService.UpdateStatusToWorking();
            if (response != null)
            {
                MessageBox.Show(string.Join("\n", response), "", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            }

            await _orderService.GetOrderStatuses();
            await _orderService.GetOrderCategories();
            await _orderService.GetPriorities();

            MCNameTxt.Text = $"УК: {TemporaryDataStorage.EmployeeData.ManagementCompany.Name}";
            FullNameTxt.Text = TemporaryDataStorage.EmployeeData.UserData.Fullname;
            PositionTxt.Text = TemporaryDataStorage.EmployeeData.EmployeePosition.Name;

            await SetExecutors();

            await SetOrders();
        }

        private async Task SetOrders()
        {
            if (TemporaryDataStorage.ManagementCompany == null)
                return;
            NewOrdersPanel.Children.Clear();
            ActiveOrdersPanel.Children.Clear();
            var ordersResponse = await _dispatcherService.GetCompanyOrders(TemporaryDataStorage.ManagementCompany.Id);
            foreach (var order in ordersResponse.OrderBy(o => o.Priority.Id))
            {
                var orderControl = new MiniOrderControl(_getterDIServices, _orderService);
                orderControl.ViewModel = new UserControls.ViewModels.OrderControlViewModel { Order = order };

                if (order.OrderStatus.Id == (int)OrderStatusEnum.New)
                    NewOrdersPanel.Children.Add(orderControl);
                else if (order.OrderStatus.Id >= (int)OrderStatusEnum.ExecutorAppointed && order.OrderStatus.Id <= (int)OrderStatusEnum.MarkedAsCompletedByExecutor)
                    ActiveOrdersPanel.Children.Add(orderControl);

                orderControl.ExecutorAppointed += async (s, e) =>
                {
                    await SetOrders();
                    await SetExecutors();
                };
            }
        }

        private async Task SetExecutors()
        {
            if (TemporaryDataStorage.ManagementCompany == null)
                return;
            ExecutorsPanel.Children.Clear();
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
