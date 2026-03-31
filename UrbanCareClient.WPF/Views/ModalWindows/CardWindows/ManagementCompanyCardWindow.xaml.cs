using System.Windows;
using System.Windows.Media;
using UrbanCareClient.WPF.Services;

namespace UrbanCareClient.WPF.Views.ModalWindows.CardWindows
{
    /// <summary>
    /// Логика взаимодействия для ManagementCompanyCardWindow.xaml
    /// </summary>
    public partial class ManagementCompanyCardWindow : Window
    {
        public int adminId;
        public ManagementCompanyCardWindow()
        {
            InitializeComponent();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckFields()) return;


        }

        private bool CheckFields()
        {
            List<bool> checkings = new List<bool> {
            ValidateFieldsService.ValidateTextBoxFilled(NameInp, "Название компании", Colors.Gray),
            ValidateFieldsService.ValidateTextBoxFilled(AddressInp, "Адрес", Colors.Gray),
            adminId > 0
            };
            return checkings.All(x => x);
        }
    }
}
