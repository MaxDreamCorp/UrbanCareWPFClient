using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UrbanCareClient.Application.Services.ApiServices;
using UrbanCareClient.Application.Services.OtherServices;
using UrbanCareClient.Domain.Commands;
using UrbanCareClient.Domain.Enums;
using UrbanCareClient.WPF.Interfaces;
using UrbanCareClient.WPF.Services;
using UrbanCareClient.WPF.Views.Windows;

namespace UrbanCareClient.WPF.Views.ModalWindows
{
    /// <summary>
    /// Логика взаимодействия для LogInModalWindow.xaml
    /// </summary>
    public partial class LogInModalWindow : Window
    {
        private readonly INavigationService _navigationService;
        private readonly AuthorizationService _authorizationService;
        private readonly CompanyService _companyService;
        private readonly SolidColorBrush _borderBrush;

        public LogInModalWindow(INavigationService navigationService, AuthorizationService authorizationService, CompanyService companyService)
        {
            InitializeComponent();
            _navigationService = navigationService;
            _authorizationService = authorizationService;
            _borderBrush = (SolidColorBrush)System.Windows.Application.Current.Resources["BorderBrush"];


            _companyService = companyService;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await _companyService.GetRoles();
                await _companyService.GetBuildingTypes();
                await _companyService.GetFloorMaterials();
                await _companyService.GetWallMaterials();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            }
#if DEBUG
            ComboBox userDebugChooser = new ComboBox()
            {
                ItemsSource = TemporaryDataStorage.Roles.Select(r => r.Role).ToList()
            };

            userDebugChooser.SelectionChanged += (sender, e) =>
            {
                var selectedString = e.AddedItems[0]?.ToString() ?? string.Empty;
                if (selectedString == "Администратор")
                {
                    LoginInp.Text = "alex.dyakov@example.com";
                    Passwordnp.Password = "qw*rTy228";
                }
                else if (selectedString == "Житель" && LoginInp.Text == "i")
                {
                    LoginInp.Text = "i.frolova.home@yandex.ru";
                    Passwordnp.Password = "TempPass123!";
                }
                else if (selectedString == "Житель")
                {
                    LoginInp.Text = "a.smirnov.home@mail.ru";
                    Passwordnp.Password = "TempPass123!";
                }
                else if (selectedString == "Диспетчер")
                {
                    LoginInp.Text = "a.petrova.uk@mail.ru";
                    Passwordnp.Password = "TempPass123!";
                }
                else if (selectedString == "Исполнитель" && LoginInp.Text == "e2")
                {
                    LoginInp.Text = "executor2.jkh@yandex.ru";
                    Passwordnp.Password = "TempPass123!";
                }
                else if (selectedString == "Исполнитель")
                {
                    LoginInp.Text = "executor1.jkh@mail.ru";
                    Passwordnp.Password = "TempPass123!";
                }
            };

            InpPanel.Children.Add(userDebugChooser);
#endif
        }

        private void RegistrationBtn_Click(object sender, RoutedEventArgs e)
        {
            var registrationWindow = _navigationService.GetWindow<RegistrationModalWindow>();
            registrationWindow.ShowDialog();
        }

        private async void LogInBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckFields()) return;

            var cmd = new LogInCommand(LoginInp.Text, Passwordnp.Password);

            var response = await _authorizationService.LogInAsync(cmd);

            if (response.errors != null)
                MessageBox.Show(string.Join('\n', response.errors), "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {

                switch (response.roleId)
                {
                    case (int)RoleEnum.Admin:
                        var adminWindow = _navigationService.GetWindow<AdminWindow>();
                        adminWindow.Show();
                        Close();
                        break;
                    case (int)RoleEnum.Dispatcher:
                        var dispatcherWindow = _navigationService.GetWindow<DispatcherWindow>();
                        dispatcherWindow.Show();
                        Close();
                        break;
                    case (int)RoleEnum.Resident:
                        var residentWindow = _navigationService.GetWindow<ResidentWindow>();
                        residentWindow.Show();
                        Close();
                        break;
                    case (int)RoleEnum.Executor:
                        var executorWindow = _navigationService.GetWindow<ExecutorWindow>();
                        executorWindow.Show();
                        Close();
                        break;
                    default:
                        break;
                }
            }
        }

        private void LoginInp_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ValidateFieldsService.ValidateTextBoxFilled(LoginInp, "Логин", _borderBrush.Color);
        }

        private void Passwordnp_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ValidateFieldsService.ValidatePasswordBoxFilled(Passwordnp, "Пароль", _borderBrush.Color);

        }

        private bool CheckFields()
        {
            var checkings = new List<bool>()
            {
                ValidateFieldsService.ValidateTextBoxFilled(LoginInp, "Логин", _borderBrush.Color),
                ValidateFieldsService.ValidatePasswordBoxFilled(Passwordnp, "Пароль", _borderBrush.Color),
            };
            return checkings.All(x => x == true);
        }
    }
}
