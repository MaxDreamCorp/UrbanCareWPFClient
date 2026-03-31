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

                control.StatusTxt.Text = orderControlViewModel.Order.OrderStatus.Status;

                switch ((OrderStatusEnum)orderControlViewModel.Order.OrderStatus.Id)
                {
                    case OrderStatusEnum.New:
                        control.StatusTxt.Foreground = StylesService.NewBrush;
                        control.StatusBdr.Background = StylesService.NewBgBrush;
                        break;
                    case OrderStatusEnum.InProgress:
                        control.StatusTxt.Foreground = StylesService.InProgressBrush;
                        control.StatusBdr.Background = StylesService.InProgressBgBrush;
                        break;
                    case OrderStatusEnum.WaitingForPayment:
                        control.StatusTxt.Foreground = StylesService.WaitingForPaymentBrush;
                        control.StatusBdr.Background = StylesService.WaitingForPaymentBgBrush;
                        break;
                    case OrderStatusEnum.Finished:
                        control.StatusTxt.Foreground = StylesService.FinishedBrush;
                        control.StatusBdr.Background = StylesService.FinishedBgBrush;
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
    }
}
