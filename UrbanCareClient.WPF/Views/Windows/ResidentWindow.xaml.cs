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
        private int _executoAppointedOrdersCount;
        private int _inProgressOrdersCount;
        private int _markedAsCompletedByExecutorOrdersCount;
        private int _pendingPaymentOrdersCount;
        private int _completedOrdersCount;
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

            foreach (var order in orders.OrderByDescending(o => o.CreatedAt))
            {
                var orderControl = new OrderControl(_getterDIServices, _orderService);
                orderControl.ViewModel = new UserControls.ViewModels.OrderControlViewModel
                {
                    Order = order
                };

                if (order.OrderStatus.Id < (int)OrderStatusEnum.MarkedAsCompletedByExecutor)
                    ActiveOrdersPanel.Children.Add(orderControl);
                else if (order.OrderStatus.Id == (int)OrderStatusEnum.MarkedAsCompletedByExecutor)
                    MarkedAsCompletedOrdersPanel.Children.Add(orderControl);
                else if (order.OrderStatus.Id == (int)OrderStatusEnum.PendingPayment)
                    PendingPaymentOrdersPanel.Children.Add(orderControl);
                else if (order.OrderStatus.Id == (int)OrderStatusEnum.Completed)
                    CompletedOrdersPanel.Children.Add(orderControl);
                else if (order.OrderStatus.Id == (int)OrderStatusEnum.Canceled)
                    CanceledOrdersPanel.Children.Add(orderControl);
            }

            _newOrdersCount = orders.Where(o => o.OrderStatus.Id == (int)OrderStatusEnum.New).Count();
            _executoAppointedOrdersCount = orders.Where(o => o.OrderStatus.Id == (int)OrderStatusEnum.ExecutorAppointed).Count();
            _inProgressOrdersCount = orders.Where(o => o.OrderStatus.Id == (int)OrderStatusEnum.InProgress).Count();
            _markedAsCompletedByExecutorOrdersCount = orders.Where(o => o.OrderStatus.Id == (int)OrderStatusEnum.MarkedAsCompletedByExecutor).Count();
            _pendingPaymentOrdersCount = orders.Where(o => o.OrderStatus.Id == (int)OrderStatusEnum.PendingPayment).Count();
            _completedOrdersCount = orders.Where(o => o.OrderStatus.Id == (int)OrderStatusEnum.Completed).Count();
            _canceledOrdersCount = orders.Where(o => o.OrderStatus.Id == (int)OrderStatusEnum.Canceled).Count();

            RefreshCounters();
        }

        private void RefreshCounters()
        {
            NewOrdersTxt.Text = _newOrdersCount.ToString();
            ExecutorAppointedOrdersTxt.Text = _executoAppointedOrdersCount.ToString();
            InProgressOrdersTxt.Text = _inProgressOrdersCount.ToString();
            MarkedAsCompletedByExecutorOrdersTxt.Text = _markedAsCompletedByExecutorOrdersCount.ToString();
            PendingPaymentOrdersTxt.Text = _pendingPaymentOrdersCount.ToString();
            CompletedOrdersTxt.Text = _completedOrdersCount.ToString();
            CanceledOrdersTxt.Text = _canceledOrdersCount.ToString();
        }

        private void ActiveOrders_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ActiveOrdersPanel.Visibility == Visibility.Visible)
            {
                ActiveOrdersPanel.Visibility = Visibility.Collapsed;
                ActiveOrderChevronUp.Visibility = Visibility.Collapsed;
                ActiveOrderChevronDown.Visibility = Visibility.Visible;
            }
            else
            {
                ActiveOrdersPanel.Visibility = Visibility.Visible;
                ActiveOrderChevronUp.Visibility = Visibility.Visible;
                ActiveOrderChevronDown.Visibility = Visibility.Collapsed;
            }
        }

        private void PendingPaymentOrders_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (PendingPaymentOrdersPanel.Visibility == Visibility.Visible)
            {
                PendingPaymentOrdersPanel.Visibility = Visibility.Collapsed;
                PendingPaymentOrderChevronUp.Visibility = Visibility.Collapsed;
                PendingPaymentOrderChevronDown.Visibility = Visibility.Visible;
            }
            else
            {
                PendingPaymentOrdersPanel.Visibility = Visibility.Visible;
                PendingPaymentOrderChevronUp.Visibility = Visibility.Visible;
                PendingPaymentOrderChevronDown.Visibility = Visibility.Collapsed;
            }
        }

        private void CompletedOrders_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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

        private void CanceledOrders_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (CanceledOrdersPanel.Visibility == Visibility.Visible)
            {
                CanceledOrdersPanel.Visibility = Visibility.Collapsed;
                CanceledOrderChevronUp.Visibility = Visibility.Collapsed;
                CanceledOrderChevronDown.Visibility = Visibility.Visible;
            }
            else
            {
                CanceledOrdersPanel.Visibility = Visibility.Visible;
                CanceledOrderChevronUp.Visibility = Visibility.Visible;
                CanceledOrderChevronDown.Visibility = Visibility.Collapsed;
            }
        }

        private void MarkedAsCompletedOrders_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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
    }
}