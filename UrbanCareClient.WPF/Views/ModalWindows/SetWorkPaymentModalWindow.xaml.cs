using System.Windows;

namespace UrbanCareClient.WPF.Views.ModalWindows
{
    /// <summary>
    /// Логика взаимодействия для SetWorkPaymentModalWindow.xaml
    /// </summary>
    public partial class SetWorkPaymentModalWindow : Window
    {
        public decimal WorkPayment { get; private set; } = 0;
        public bool IsSet { get; private set; }

        public SetWorkPaymentModalWindow()
        {
            InitializeComponent();
        }

        private void SetBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PaymentTextBox.Text))
            {
                var mboxResult = MessageBox.Show("Вы уверены, что хотите отменить установку суммы оплаты?\nВ данном случае сумма будет равна 0!", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (mboxResult == MessageBoxResult.Yes)
                {
                    IsSet = true;
                    Close();
                    return;
                }
            }
            if (decimal.TryParse(PaymentTextBox.Text, out decimal payment))
            {
                var mboxResult = MessageBox.Show($"Вы уверены, что хотите установить сумму оплаты {payment}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (mboxResult == MessageBoxResult.Yes)
                {
                    WorkPayment = payment;
                    IsSet = true;
                    Close();
                    return;
                }
            }
            MessageBox.Show("Пожалуйста, введите корректную сумму оплаты.", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            IsSet = false;
            Close();
        }
    }
}
