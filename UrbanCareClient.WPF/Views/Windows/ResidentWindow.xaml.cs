using System.Windows;
using UrbanCareClient.Application.Services.ApiServices;
using UrbanCareClient.Application.Services.OtherServices;
using UrbanCareClient.Domain.Enums;
using UrbanCareClient.WPF.Interfaces;
using UrbanCareClient.WPF.Services;
using UrbanCareClient.WPF.Views.ModalWindows;
using UrbanCareClient.WPF.Views.ModalWindows.CardWindows;
using UrbanCareClient.WPF.Views.UserControls;

namespace UrbanCareClient.WPF.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для ResidentWindow.xaml
    /// </summary>
    public partial class ResidentWindow : Window
    {
        private readonly ResidentService _residentService;
        private readonly OrderService _orderService;
        private readonly INavigationService _navigationService;
        private readonly GetterDIServices _getterDIServices;

        private int _newOrdersCount;
        private int _inProgressOrdersCount;
        private int _waitingForPaymentOrdersCount;
        private int _finishedOrdersCount;
        private int _canceledOrdersCount;

        public ResidentWindow(GetterDIServices getterDIServices, INavigationService navigationService, ResidentService residentService, OrderService orderService)
        {
            InitializeComponent();
            _getterDIServices = getterDIServices;
            _navigationService = navigationService;
            _residentService = residentService;
            _orderService = orderService;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var residentDataResponse = await _residentService.GetMyResidentData();
            if (residentDataResponse == null)
            {
                MessageBox.Show("Ошибка получения данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                var authWindow = _navigationService.GetWindow<LogInModalWindow>();
                authWindow.Show();
                Close();
                return;
            }

            TemporaryDataStorage.ResidentData = residentDataResponse;
            TemporaryDataStorage.CurrentUserId = residentDataResponse.Id;
            TemporaryDataStorage.UserData = residentDataResponse.UserData;

            await _orderService.GetOrderStatuses();
            await _orderService.GetOrderCategories();
            await _orderService.GetPriorities();

            FullNameTxt.Text = TemporaryDataStorage.UserData.Fullname;
            MCNameTxt.Text = TemporaryDataStorage.ResidentData.Apartment.Building.Region.ManagementCompany.Name;

            await SetOrders();
        }

        private async void CreateOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            var _orderCard = new OrderCardWindow(_getterDIServices, _orderService, Enums.WindowOperations.Create);
            _orderCard.ShowDialog();
            await SetOrders();
        }

        private async Task SetOrders()
        {
            ActiveOrdersPanel.Children.Clear();
            var orders = await _residentService.GetMyOrders();

            foreach (var order in orders)
            {
                var orderControl = new OrderControl();
                orderControl.ViewModel = new UserControls.ViewModels.OrderControlViewModel
                {
                    Order = order
                };

                if (order.OrderStatus.Id < (int)OrderStatusEnum.Finished)
                    ActiveOrdersPanel.Children.Add(orderControl);
            }

            _newOrdersCount = orders.Where(o => o.OrderStatus.Id == (int)OrderStatusEnum.New).Count();
            _inProgressOrdersCount = orders.Where(o => o.OrderStatus.Id == (int)OrderStatusEnum.InProgress).Count();
            _waitingForPaymentOrdersCount = orders.Where(o => o.OrderStatus.Id == (int)OrderStatusEnum.WaitingForPayment).Count();
            _finishedOrdersCount = orders.Where(o => o.OrderStatus.Id == (int)OrderStatusEnum.Finished).Count();
            _canceledOrdersCount = orders.Where(o => o.OrderStatus.Id == (int)OrderStatusEnum.Canceled).Count();

            RefreshCounters();
        }

        private void RefreshCounters()
        {
            NewOrdersTxt.Text = _newOrdersCount.ToString();
            InProgressOrdersTxt.Text = _inProgressOrdersCount.ToString();
            WaitingForPaymentOrdersTxt.Text = _waitingForPaymentOrdersCount.ToString();
            FinishedOrdersTxt.Text = _finishedOrdersCount.ToString();
            CanceledOrdersTxt.Text = _canceledOrdersCount.ToString();
        }
    }
}
