using System.Windows;
using System.Windows.Controls;
using UrbanCareClient.Application.Services.ApiServices;
using UrbanCareClient.Domain.Enums;
using UrbanCareClient.WPF.Services;
using UrbanCareClient.WPF.Views.UserControls.ViewModels;

namespace UrbanCareClient.WPF.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ExecutorOrderControl.xaml
    /// </summary>
    public partial class ExecutorOrderControl : UserControl
    {
        private readonly GetterDIServices _getterDIServices;
        private readonly OrderService _orderService;
        private readonly ExecutorService _executorService;
        public event EventHandler? OrderUpdated;

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                nameof(ViewModel),
                typeof(OrderControlViewModel),
                typeof(ExecutorOrderControl),
                new PropertyMetadata(null, OnViewModelChanged));

        public OrderControlViewModel ViewModel
        {
            get => (OrderControlViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public ExecutorOrderControl(GetterDIServices getterDIServices, OrderService orderService)
        {
            InitializeComponent();
            _getterDIServices = getterDIServices;
            _orderService = orderService;
            _executorService = _getterDIServices.GetService<ExecutorService>();
        }

        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ExecutorOrderControl control && e.NewValue is OrderControlViewModel orderControlViewModel)
            {
                control.LayoutRoot.DataContext = e.NewValue;

                control.HeaderTxt.Text = $"Заказ №{orderControlViewModel.Order.Id}";
                control.DescriptionTxt.Text = orderControlViewModel.Order.Description;


                string address = $"{orderControlViewModel.Order.Building.Region.CommonAddress}, {orderControlViewModel.Order.Building.Address}";

                if (orderControlViewModel.Order.Apartment != null)
                    address += $", кв. {orderControlViewModel.Order.Apartment.Number}";

                control.AddressTxt.Text = address;
                control.ResidentFullnameTxt.Text = orderControlViewModel.Order.Resident.UserData.Fullname;
                control.ResidentPhoneTxt.Text = orderControlViewModel.Order.ContactPhone;

                if (orderControlViewModel.Order.Dispatcher != null)
                {
                    control.InWorkPanel.Visibility = Visibility.Visible;
                    control.DispatcherTxt.Text = $"Диспетчер: {orderControlViewModel.Order.Dispatcher.UserData.Fullname}";
                }

                if (orderControlViewModel.Order.OrderExecutors != null && orderControlViewModel.Order.OrderExecutors.Count > 0)
                    control.ExecutorsTxt.Text = $"Исполнители: {string.Join(", ", orderControlViewModel.Order.OrderExecutors.Select(oe => oe.Employee.UserData.Fullname))}";

                control.PaymentPanel.Visibility = Visibility.Visible;

                if (orderControlViewModel.Order.OrderMaterials != null && orderControlViewModel.Order.OrderMaterials.Count > 0)
                    control.MaterialsTxt.Text = $"{string.Join("\n", orderControlViewModel.Order.OrderMaterials.Select(om => $"{om.Material.Name} x ({om.Quantity} {om.Material.Unit}"))}";

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
                        control.StartBtn.Visibility = Visibility.Visible;
                        control.AddMaterialsBtn.Visibility = Visibility.Visible;
                        control.MarkAsCompletedBtn.Visibility = Visibility.Collapsed;
                        break;
                    case OrderStatusEnum.MarkedAsCompletedByExecutor:
                        control.StatusTxt.Foreground = StylesService.MarkedAsCompletedBrush;
                        control.StatusBdr.Background = StylesService.MarkedAsCompletedBgBrush;
                        control.MarkAsCompletedBtn.Visibility = Visibility.Collapsed;

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
                        control.ButtonsPanel.Visibility = Visibility.Collapsed;
                        break;
                    case OrderStatusEnum.Canceled:
                        control.StatusTxt.Foreground = StylesService.CanceledBrush;
                        control.StatusBdr.Background = StylesService.CanceledBgBrush;
                        break;
                    default:
                        break;
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

        private async void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            var mboxResult = MessageBox.Show("Вы уверены, что хотите начать выполнение заказа?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mboxResult != MessageBoxResult.Yes)
                return;
            var response = await _executorService.AcceptOrder(ViewModel.Order.Id);
            if (response != null)
            {
                MessageBox.Show(string.Join("\n", response), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Вы начали выполнение заказа", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            OrderUpdated?.Invoke(this, new());
        }

        private async void MarkAsCompletedBtn_Click(object sender, RoutedEventArgs e)
        {
            var mboxResult = MessageBox.Show("Вы уверены, что хотите отметить заказ как выполненный?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mboxResult != MessageBoxResult.Yes)
                return;

            var response = await _executorService.MarkAsCompleted(ViewModel.Order.Id);
            if (response != null)
            {
                MessageBox.Show(string.Join("\n", response), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Вы отметили заказ как выполненный", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            OrderUpdated?.Invoke(this, new());
        }

        private void AddMaterialsBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
