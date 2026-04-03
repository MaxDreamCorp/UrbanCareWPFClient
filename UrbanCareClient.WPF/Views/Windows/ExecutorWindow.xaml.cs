using System.Windows;
using System.Windows.Input;

namespace UrbanCareClient.WPF.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для ExecutorWindow.xaml
    /// </summary>
    public partial class ExecutorWindow : Window
    {
        public ExecutorWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ExecutorAppointedOrders_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ExecutorAppointedOrders.Visibility == Visibility.Visible)
            {
                ExecutorAppointedOrders.Visibility = Visibility.Collapsed;
                ExecutorAppointedOrderChevronUp.Visibility = Visibility.Collapsed;
                ExecutorAppointedOrderChevronDown.Visibility = Visibility.Visible;
            }
            else
            {
                ExecutorAppointedOrders.Visibility = Visibility.Visible;
                ExecutorAppointedOrderChevronUp.Visibility = Visibility.Visible;
                ExecutorAppointedOrderChevronDown.Visibility = Visibility.Collapsed;
            }
        }

        private void InProgressOrders_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (InProgressOrders.Visibility == Visibility.Visible)
            {
                InProgressOrders.Visibility = Visibility.Collapsed;
                InProgressOrderChevronUp.Visibility = Visibility.Collapsed;
                InProgressOrderChevronDown.Visibility = Visibility.Visible;
            }
            else
            {
                InProgressOrders.Visibility = Visibility.Visible;
                InProgressOrderChevronUp.Visibility = Visibility.Visible;
                InProgressOrderChevronDown.Visibility = Visibility.Collapsed;
            }
        }

        private void MarkedAsCompletedOrders_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (MarkedAsCompletedOrders.Visibility == Visibility.Visible)
            {
                MarkedAsCompletedOrders.Visibility = Visibility.Collapsed;
                MarkedAsCompletedOrderChevronUp.Visibility = Visibility.Collapsed;
                MarkedAsCompletedOrderChevronDown.Visibility = Visibility.Visible;
            }
            else
            {
                MarkedAsCompletedOrders.Visibility = Visibility.Visible;
                MarkedAsCompletedOrderChevronUp.Visibility = Visibility.Visible;
                MarkedAsCompletedOrderChevronDown.Visibility = Visibility.Collapsed;
            }
        }

        private void CompletedOrders_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (CompletedOrders.Visibility == Visibility.Visible)
            {
                CompletedOrders.Visibility = Visibility.Collapsed;
                CompletedOrderChevronUp.Visibility = Visibility.Collapsed;
                CompletedOrderChevronDown.Visibility = Visibility.Visible;
            }
            else
            {
                CompletedOrders.Visibility = Visibility.Visible;
                CompletedOrderChevronUp.Visibility = Visibility.Visible;
                CompletedOrderChevronDown.Visibility = Visibility.Collapsed;
            }

        }
    }
}
