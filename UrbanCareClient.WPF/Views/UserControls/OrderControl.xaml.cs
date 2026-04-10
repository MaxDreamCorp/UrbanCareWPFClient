using System.Windows;
using System.Windows.Controls;
using UrbanCareClient.Application.Services.ApiServices;
using UrbanCareClient.Domain.Enums;
using UrbanCareClient.WPF.Services;
using UrbanCareClient.WPF.Views.ModalWindows.CardWindows;
using UrbanCareClient.WPF.Views.UserControls.ViewModels;

namespace UrbanCareClient.WPF.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для OrderControl.xaml
    /// </summary>
    public partial class OrderControl : UserControl
    {
        private readonly GetterDIServices _getterDIServices;
        private readonly OrderService _orderService;
        private readonly ResidentService _residentService;
        public event EventHandler? OrderUpdated;

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                nameof(ViewModel),
                typeof(OrderControlViewModel),
                typeof(OrderControl),
                new PropertyMetadata(null, OnViewModelChanged));

        public OrderControlViewModel ViewModel
        {
            get => (OrderControlViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public OrderControl(GetterDIServices getterDIServices, OrderService orderService)
        {
            InitializeComponent();
            _getterDIServices = getterDIServices;
            _orderService = orderService;
            _residentService = getterDIServices.GetService<ResidentService>();
        }

        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is OrderControl control && e.NewValue is OrderControlViewModel orderControlViewModel)
            {
                control.LayoutRoot.DataContext = e.NewValue;

                control.HeaderTxt.Text = $"Заказ №{orderControlViewModel.Order.Id}";
                control.DescriptionTxt.Text = orderControlViewModel.Order.Description;
                control.CategoryTxt.Text = orderControlViewModel.Order.OrderCategory.Category;
                control.TypeTxt.Text = orderControlViewModel.Order.OrderCategory.OrderType.Type;


                string address = $"{orderControlViewModel.Order.Building.Region.CommonAddress}, {orderControlViewModel.Order.Building.Address}";

                if (orderControlViewModel.Order.Apartment != null)
                    address += $", кв. {orderControlViewModel.Order.Apartment.Number}";

                control.AddressTxt.Text = address;
                control.DateTxt.Text = orderControlViewModel.Order.CreatedAt.ToString("dd.MM.yyyy");
                control.ContactPhoneTxt.Text = orderControlViewModel.Order.ContactPhone;
                control.ContactEmailTxt.Text = orderControlViewModel.Order.ContactEmail;

                if (orderControlViewModel.Order.OrderStatus.Id >= (int)OrderStatusEnum.ExecutorAppointed)
                {
                    control.InWorkPanel.Visibility = Visibility.Visible;
                    if (orderControlViewModel.Order.Dispatcher != null)
                        control.DispatcherTxt.Text = $"Диспетчер: {orderControlViewModel.Order.Dispatcher.UserData.Fullname}";
                    if (orderControlViewModel.Order.OrderExecutors != null && orderControlViewModel.Order.OrderExecutors.Count > 0)
                        control.ExecutorsTxt.Text = $"Исполнители: {string.Join(", ", orderControlViewModel.Order.OrderExecutors.Select(oe => oe.Employee.UserData.Fullname))}";
                }

                if (orderControlViewModel.Order.OrderStatus.Id >= (int)OrderStatusEnum.ExecutorAppointed)
                {
                    control.PaymentPanel.Visibility = Visibility.Visible;
                    decimal totalCost = 0;

                    if (orderControlViewModel.Order.OrderMaterials != null && orderControlViewModel.Order.OrderMaterials.Count > 0)
                    {
                        control.MaterialsTxt.Visibility = Visibility.Visible;
                        control.MaterialsTxt.Text = $"Расходники:\n{string.Join("\n", orderControlViewModel.Order.OrderMaterials.Select(om => $"- {om.Material.Name} - {om.Material.Price} руб. x ({om.Quantity} {om.Material.Unit}.)"))}";
                        totalCost += orderControlViewModel.Order.OrderMaterials.Sum(om => om.Quantity * om.Material.Price);
                    }

                    if (orderControlViewModel.Order.OrderExecutors != null && orderControlViewModel.Order.OrderExecutors.Count > 0)
                    {
                        control.MaterialsTxt.Text += $"\nРабота: {orderControlViewModel.Order.OrderExecutors.Sum(oe => oe.WorkPayment)} руб.";
                        totalCost += orderControlViewModel.Order.OrderExecutors.Sum(oe => oe.WorkPayment) ?? 0;
                    }

                    control.PaymentTxt.Text = $"Итого: {totalCost} руб.";
                    if (totalCost == 0 || orderControlViewModel.Order.OrderStatus.Id < (int)OrderStatusEnum.PendingPayment)
                        control.PayBtn.Visibility = Visibility.Collapsed;
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
                        control.ConfirmCompletionBtn.Visibility = Visibility.Visible;
                        break;
                    case OrderStatusEnum.InProgress:
                        control.StatusTxt.Foreground = StylesService.InProgressBrush;
                        control.StatusBdr.Background = StylesService.InProgressBgBrush;
                        break;
                    case OrderStatusEnum.PendingPayment:
                        control.StatusTxt.Foreground = StylesService.PendingPaymentBrush;
                        control.StatusBdr.Background = StylesService.PendingPaymentBgBrush;

                        control.MoreBtn.Visibility = Visibility.Collapsed;
                        break;
                    case OrderStatusEnum.Completed:
                        control.StatusTxt.Foreground = StylesService.CompletedBrush;
                        control.StatusBdr.Background = StylesService.CompletedBgBrush;

                        control.PayBtn.Visibility = Visibility.Collapsed;
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

        private void MoreBtn_Click(object sender, RoutedEventArgs e)
        {
            var orderCard = new OrderCardWindow(_getterDIServices, _orderService, Enums.WindowOperations.Edit, ViewModel.Order);
            orderCard.ShowDialog();
            if (orderCard.OrderResponseDTO != null)
                ViewModel = new OrderControlViewModel { Order = orderCard.OrderResponseDTO };
        }

        private async void ConfirmCompletionBtn_Click(object sender, RoutedEventArgs e)
        {
            var mboxResult = MessageBox.Show("Вы уверены, что хотите подтвердить выполнение заказа?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mboxResult != MessageBoxResult.Yes)
                return;

            var response = await _residentService.ConfirmOrderCompletion(ViewModel.Order.Id);
            if (response != null)
            {
                MessageBox.Show(string.Join("\n", response), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Заказ успешно подтвержден", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            OrderUpdated?.Invoke(this, EventArgs.Empty);
        }

        private async void PayBtn_Click(object sender, RoutedEventArgs e)
        {
            var mboxResult = MessageBox.Show("Вы уверены, что хотите оплатить заказ?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mboxResult != MessageBoxResult.Yes)
                return;

            var response = await _residentService.ImitatePayment(ViewModel.Order.Id);
            if (response != null)
            {
                MessageBox.Show(string.Join("\n", response), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Заказ успешно оплачен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            OrderUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}
