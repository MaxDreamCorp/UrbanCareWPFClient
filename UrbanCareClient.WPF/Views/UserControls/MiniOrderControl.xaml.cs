using System.Windows;
using System.Windows.Controls;
using UrbanCareClient.Application.Services.ApiServices;
using UrbanCareClient.Domain.Enums;
using UrbanCareClient.WPF.Services;
using UrbanCareClient.WPF.Views.ModalWindows;
using UrbanCareClient.WPF.Views.ModalWindows.CardWindows;
using UrbanCareClient.WPF.Views.UserControls.ViewModels;

namespace UrbanCareClient.WPF.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для MiniOrderControl.xaml
    /// </summary>
    public partial class MiniOrderControl : UserControl
    {
        public event EventHandler? ExecutorAppointed;
        private readonly GetterDIServices _getterDIServices;
        private readonly OrderService _orderService;
        private readonly DispatcherService _dispatcherService;

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                nameof(ViewModel),
                typeof(OrderControlViewModel),
                typeof(MiniOrderControl),
                new PropertyMetadata(null, OnViewModelChanged));

        public OrderControlViewModel ViewModel
        {
            get => (OrderControlViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public MiniOrderControl(GetterDIServices getterDIServices, OrderService orderService)
        {
            InitializeComponent();
            _getterDIServices = getterDIServices;
            _orderService = orderService;
            _dispatcherService = getterDIServices.GetService<DispatcherService>();
        }


        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MiniOrderControl control && e.NewValue is OrderControlViewModel orderControlViewModel)
            {
                control.LayoutRoot.DataContext = e.NewValue;

                control.HeaderTxt.Text = $"Заказ №{orderControlViewModel.Order.Id}";
                control.DescriptionTxt.Text = orderControlViewModel.Order.Description;
                string address = $"{orderControlViewModel.Order.Building.Region.CommonAddress}, {orderControlViewModel.Order.Building.Address}";

                if (orderControlViewModel.Order.Apartment != null)
                    address += $", кв. {orderControlViewModel.Order.Apartment.Number}";

                control.FullnameTxt.Text = orderControlViewModel.Order.Resident.UserData.Fullname;
                control.DateTxt.Text = orderControlViewModel.Order.CreatedAt.ToString("dd.MM.yyyy");
                control.ContactPhoneTxt.Text = orderControlViewModel.Order.ContactPhone;
                control.ContactEmailTxt.Text = orderControlViewModel.Order.ContactEmail;


                if (orderControlViewModel.Order.OrderStatus.Id == (int)OrderStatusEnum.InProgress || orderControlViewModel.Order.OrderStatus.Id == (int)OrderStatusEnum.PendingPayment)
                {
                    control.SetExecutorBtn.Visibility = Visibility.Collapsed;

                    control.InWorkPanel.Visibility = Visibility.Visible;
                    if (orderControlViewModel.Order.Dispatcher != null)
                        control.DispatcherTxt.Text = $"Диспетчер: {orderControlViewModel.Order.Dispatcher.UserData.Fullname}";
                    if (orderControlViewModel.Order.OrderExecutors != null && orderControlViewModel.Order.OrderExecutors.Count > 0)
                        control.ExecutorsTxt.Text = $"Исполнители: {string.Join(", ", orderControlViewModel.Order.OrderExecutors.Select(oe => oe.Employee.UserData.Fullname))}";
                }

                control.StatusTxt.Text = orderControlViewModel.Order.OrderStatus.Status;

                switch ((OrderStatusEnum)orderControlViewModel.Order.OrderStatus.Id)
                {
                    case OrderStatusEnum.New:
                        control.StatusTxt.Foreground = StylesService.NewBrush;
                        control.StatusBdr.Background = StylesService.NewBgBrush;
                        break;
                    case OrderStatusEnum.ExecutorAppointed:
                        control.StatusTxt.Foreground = StylesService.ExecutorAppointedBrush;
                        control.StatusBdr.Background = StylesService.ExecutorAppointedBgBrush;
                        break;
                    case OrderStatusEnum.MarkedAsCompletedByExecutor:
                        control.StatusTxt.Foreground = StylesService.MarkedAsCompletedBrush;
                        control.StatusBdr.Background = StylesService.MarkedAsCompletedBgBrush;
                        break;
                    case OrderStatusEnum.InProgress:
                        control.StatusTxt.Foreground = StylesService.InProgressBrush;
                        control.StatusBdr.Background = StylesService.InProgressBgBrush;
                        break;
                    case OrderStatusEnum.PendingPayment:
                        control.StatusTxt.Foreground = StylesService.PendingPaymentBrush;
                        control.StatusBdr.Background = StylesService.PendingPaymentBgBrush;
                        break;
                    case OrderStatusEnum.Completed:
                        control.StatusTxt.Foreground = StylesService.CompletedBrush;
                        control.StatusBdr.Background = StylesService.CompletedBgBrush;
                        break;
                    case OrderStatusEnum.Canceled:
                        control.StatusTxt.Foreground = StylesService.CanceledBrush;
                        control.StatusBdr.Background = StylesService.CanceledBgBrush;
                        break;
                    default:
                        break;
                }

                if (orderControlViewModel.Order.OrderStatus.Id > (int)OrderStatusEnum.New)
                {
                    if (orderControlViewModel.Order.Dispatcher != null)
                    {
                        control.InWorkPanel.Visibility = Visibility.Visible;
                        control.DispatcherTxt.Text = $"Диспетчер: {orderControlViewModel.Order.Dispatcher.UserData.Fullname}";
                    }

                    if (orderControlViewModel.Order.OrderExecutors != null && orderControlViewModel.Order.OrderExecutors.Count > 0)
                        control.ExecutorsTxt.Text = $"Исполнители: {string.Join(", ", orderControlViewModel.Order.OrderExecutors.Select(oe => oe.Employee.UserData.Fullname))}";

                    control.SetExecutorBtn.Visibility = Visibility.Collapsed;
                }

                control.PriorityTxt.Text = orderControlViewModel.Order.Priority.Priority;

                switch ((PriorityEnum)orderControlViewModel.Order.Priority.Id)
                {
                    case PriorityEnum.Critical:
                        control.PriorityTxt.Foreground = StylesService.PriorityCriticalBrush;
                        control.PriorityBdr.Background = StylesService.PriorityCriticalBgBrush;
                        break;
                    case PriorityEnum.High:
                        control.PriorityTxt.Foreground = StylesService.PriorityHighBrush;
                        control.PriorityBdr.Background = StylesService.PriorityHighBgBrush;
                        break;
                    case PriorityEnum.Medium:
                        control.PriorityTxt.Foreground = StylesService.PriorityMediumBrush;
                        control.PriorityBdr.Background = StylesService.PriorityMediumBgBrush;
                        break;
                    case PriorityEnum.Low:
                        control.PriorityTxt.Foreground = StylesService.PriorityLowBrush;
                        control.PriorityBdr.Background = StylesService.PriorityLowBgBrush;
                        break;
                    case PriorityEnum.Planning:
                        control.PriorityTxt.Foreground = StylesService.PriorityPlannedBrush;
                        control.PriorityBdr.Background = StylesService.PriorityPlannedBgBrush;
                        break;
                    default:
                        break;
                }
            }
        }

        private void SetExecutorBtn_Click(object sender, RoutedEventArgs e)
        {
            var appointingExecutorToOrderModalWindow = new AppointingExecutorToOrderModalWindow(_dispatcherService, ViewModel.Order, _orderService, _getterDIServices);
            appointingExecutorToOrderModalWindow.Closed += (s, args) =>
            {
                ExecutorAppointed?.Invoke(this, EventArgs.Empty);
            };
            appointingExecutorToOrderModalWindow.ShowDialog();
        }

        private void MoreBtn_Click(object sender, RoutedEventArgs e)
        {
            var orderCardWindow = new OrderCardWindow(_getterDIServices, _orderService, Enums.WindowOperations.Read, ViewModel.Order);
            orderCardWindow.ShowDialog();
        }
    }
}