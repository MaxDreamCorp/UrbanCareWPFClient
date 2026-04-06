using System.Windows;
using System.Windows.Controls;
using UrbanCareClient.Application.Services.ApiServices;
using UrbanCareClient.Application.Services.OtherServices;
using UrbanCareClient.Domain.Commands;
using UrbanCareClient.Domain.DTOs;
using UrbanCareClient.WPF.Services;
using UrbanCareClient.WPF.Views.UserControls;
using UrbanCareClient.WPF.Views.UserControls.ViewModels;

namespace UrbanCareClient.WPF.Views.ModalWindows
{
    /// <summary>
    /// Логика взаимодействия для AppointingExecutorToOrderModalWindow.xaml
    /// </summary>
    public partial class AppointingExecutorToOrderModalWindow : Window
    {
        private readonly OrderResponseDTO _orderResponseDTO;
        private readonly DispatcherService _dispatcherService;
        private readonly OrderService _orderService;
        private readonly GetterDIServices _getterDIServices;


        public AppointingExecutorToOrderModalWindow(DispatcherService dispatcherService, OrderResponseDTO orderResponseDTO, OrderService orderService, GetterDIServices getterDIServices)
        {
            InitializeComponent();
            _dispatcherService = dispatcherService;
            _orderResponseDTO = orderResponseDTO;
            _orderService = orderService;
            _getterDIServices = getterDIServices;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var orderControl = new MiniOrderControl(_getterDIServices, _orderService);
            orderControl.ViewModel = new OrderControlViewModel { Order = _orderResponseDTO };
            orderControl.ButtonsPanel.Visibility = Visibility.Collapsed;
            OrderPanel.Children.Add(orderControl);

            if (TemporaryDataStorage.ManagementCompany == null)
                return;

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

                    var checkBox = new CheckBox()
                    {
                        Content = executorControl,
                        Margin = new Thickness(10, 0, 0, 0)
                    };
                    ExecutorsPanel.Children.Add(checkBox);
                }
            }
        }

        private async void AppointBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TemporaryDataStorage.EmployeeData == null)
                return;

            foreach (var child in ExecutorsPanel.Children)
            {
                if (child is CheckBox checkBox && checkBox.IsChecked == true && checkBox.Content is MiniExecutorControl executorControl)
                {
                    var cmd = new AppointExecutorToOrderCommand(_orderResponseDTO.Id,
                        TemporaryDataStorage.EmployeeData.UserData.Id,
                        executorControl.ViewModal.Executor.EmployeeData.Id);

                    var response = await _dispatcherService.AppointExecutorToOrder(cmd);
                    if (response != null)
                    {
                        MessageBox.Show(string.Join("\n", response), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show("Исполнители успешно назначены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }

                }
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
