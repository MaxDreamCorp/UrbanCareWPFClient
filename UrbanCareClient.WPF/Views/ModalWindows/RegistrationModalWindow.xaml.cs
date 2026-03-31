using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UrbanCareClient.Application.Services.ApiServices;
using UrbanCareClient.Application.Services.OtherServices;
using UrbanCareClient.Domain.Commands;
using UrbanCareClient.Domain.Enums;
using UrbanCareClient.WPF.Services;

namespace UrbanCareClient.WPF.Views.ModalWindows
{
    /// <summary>
    /// Логика взаимодействия для RegistrationModalWindow.xaml
    /// </summary>
    public partial class RegistrationModalWindow : Window
    {
        private readonly AuthorizationService _authorizationService;
        private readonly GetterDIServices _getterDIServices;
        private readonly SolidColorBrush _borderBrush;

        public RegistrationModalWindow(AuthorizationService authorizationService, GetterDIServices getterDIServices)
        {
            InitializeComponent();
            _authorizationService = authorizationService;
            _borderBrush = (SolidColorBrush)System.Windows.Application.Current.Resources["BorderBrush"];
            _getterDIServices = getterDIServices;
        }

        private async void LogInBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckAllFields()) return;
            if (!DateOfBirthInp.SelectedDate.HasValue) return;

            DateTime dateOfBirth = DateOfBirthInp.SelectedDate.Value;
            var role = ConverterService.StringToRoleEnum(RoleInp.Text);
            if (role == null) return;
            var cmd = new RegistrationCommand(
                new Domain.DTOs.UserRequestDTO(
                    FullnameInp.Text,
                    EmailInp.Text,
                    PhoneInp.Text,
                    PasswordInp.Password,
                    (int)role,
                    new Domain.DTOs.UserPersonalDatumRequestDTO(
                        new Domain.DTOs.PassportDatumRequestDTO(
                            PassportDataSeriaInp.Text,
                            PassportDataNumberInp.Text,
                            PassportDataDepartmentInp.Text,
                            PassportDataDepartmentCodeInp.Text),
                        new DateOnly(dateOfBirth.Year, dateOfBirth.Month, dateOfBirth.Day),
                        SnilsInp.Text,
                        InnInp.Text)));

            var response = await _authorizationService.RegistrateAsync(cmd);

            if (response.errors == null)
            {
                MessageBox.Show("Регистрация прошла успешно", "", MessageBoxButton.OK, MessageBoxImage.Information);

                if (cmd.User.RoleId < (int)RoleEnum.Resident)
                {
                    MessageBox.Show("Вам необходимо заполнить данные работника", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    TemporaryDataStorage.CurrentUserId = response.userId;
                    EmployeeCreatingModalWindow employeeCreatingModalWindow = _getterDIServices.GetService<EmployeeCreatingModalWindow>();
                    while (!employeeCreatingModalWindow.IsReady)
                    {
                        employeeCreatingModalWindow = _getterDIServices.GetService<EmployeeCreatingModalWindow>();
                        employeeCreatingModalWindow.ShowDialog();
                    }

                    Close();
                }
                else
                {
                    MessageBox.Show("Вам необходимо заполнить данные жителя", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    TemporaryDataStorage.CurrentUserId = response.userId;
                    ResidentCreatingModalWindow residentCreatingModalWindow = _getterDIServices.GetService<ResidentCreatingModalWindow>();
                    while (!residentCreatingModalWindow.IsReady)
                    {
                        residentCreatingModalWindow = _getterDIServices.GetService<ResidentCreatingModalWindow>();
                        residentCreatingModalWindow.ShowDialog();
                    }

                    Close();
                }
            }
            else
                MessageBox.Show(string.Join('\n', response.errors), "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var roles = TemporaryDataStorage.Roles.OrderBy(r => r.Id).Select(r => r.Role).ToList();
            roles.RemoveAt(0);
            RoleInp.ItemsSource = roles;


        }

        private void FullnameInp_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckFullname();
        }

        private void EmailInp_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckEmail();
        }

        private void PhoneInp_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckPhone();
        }

        private void DateOfBirthInp_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckDateOfBirth();
        }

        private void SnilsInp_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckSnils();
        }

        private void InnInp_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckInn();
        }

        private void PasswordInp_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckPassword();
        }

        private void ConfirmPasswordInp_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckConfirmPassword();
        }

        private void PassportDataSeriaInp_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckPassportDataSeria();
        }

        private void PassportDataNumberInp_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckPassportDataNumber();
        }

        private void PassportDataDepartmentInp_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckPassportDataDepartment();
        }

        private void PassportDataDepartmentCodeInp_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckPassportDataDepartmentCode();
        }


        private bool CheckAllFields()
        {
            var checkings = new List<bool>() {
                CheckFullname(),
                CheckEmail(),
                CheckPhone(),
                CheckDateOfBirth(),
                CheckRole(),
                CheckSnils(),
                CheckInn(),
                CheckPassword(),
                CheckConfirmPassword(),
                CheckPassportDataSeria(),
                CheckPassportDataNumber(),
                CheckPassportDataDepartment(),
                CheckPassportDataDepartmentCode()
            };
            return checkings.All(x => x == true);
        }

        private bool CheckFullname()
        {
            var checkings = new List<bool>() {
              ValidateFieldsService.ValidateTextBoxFilled(FullnameInp, "ФИО", _borderBrush.Color),
                ValidateFieldsService.ValidateTextBoxRegex(FullnameInp, @"^[a-zA-Zа-яА-ЯёЁ\s\-]+$", "ФИО может содержать только буквы, пробелы и дефисы", _borderBrush.Color)
            };
            return checkings.All(x => x == true);
        }

        private bool CheckEmail()
        {
            var checkings = new List<bool>() {
                 ValidateFieldsService.ValidateTextBoxFilled(EmailInp, "Email", _borderBrush.Color),
                ValidateFieldsService.ValidateTextBoxRegex(EmailInp, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", "Некорректный формат email", _borderBrush.Color)
            };
            return checkings.All(x => x == true);
        }

        private bool CheckPhone()
        {
            var checkings = new List<bool>() {
                ValidateFieldsService.ValidateTextBoxFilled(PhoneInp, "Телефон", _borderBrush.Color),
                ValidateFieldsService.ValidateTextBoxRegex(PhoneInp, @"^\+?[1-9]\d{10,14}$", "Некорректный формат телефона", _borderBrush.Color)
            };
            return checkings.All(x => x == true);
        }

        private bool CheckDateOfBirth()
        {
            var maxDate = DateTime.Now;
            var minDate = DateTime.Now.AddYears(-150);
            var checkings = new List<bool>() {
                ValidateFieldsService.ValidateDatePickerFilled(DateOfBirthInp, "Дата рождения", _borderBrush.Color),
                ValidateFieldsService.ValidateDatePickerRange(DateOfBirthInp, "Дата рождения", minDate, maxDate, _borderBrush.Color)
            };
            return checkings.All(x => x == true);
        }

        private bool CheckRole()
        {
            return ValidateFieldsService.ValidateComboBoxFilled(RoleInp, "Роль", _borderBrush.Color);
        }

        private bool CheckSnils()
        {
            var checkings = new List<bool>() {
                ValidateFieldsService.ValidateTextBoxFilled(SnilsInp, "СНИЛС", _borderBrush.Color),
                ValidateFieldsService.ValidateTextBoxRegex(SnilsInp, @"^\d{3}-\d{3}-\d{3}\s\d{2}$", "СНИЛС должен быть в формате: XXX-XXX-XXX XX", _borderBrush.Color)
            };
            return checkings.All(x => x == true);
        }

        private bool CheckInn()
        {
            var checkings = new List<bool>() {
                ValidateFieldsService.ValidateTextBoxFilled(InnInp, "ИНН", _borderBrush.Color),
                ValidateFieldsService.ValidateTextBoxRegex(InnInp, @"^\d{12}$", "ИНН должен состоять из 12 цифр", _borderBrush.Color)
            };
            return checkings.All(x => x == true);
        }

        private bool CheckPassword()
        {
            var checkings = new List<bool>() {
                ValidateFieldsService.ValidatePasswordBoxFilled(PasswordInp, "Пароль", _borderBrush.Color),
                ValidateFieldsService.ValidatePasswordBoxRegex(PasswordInp, @"[A-Z]", "Пароль должен содержать хотя бы одну заглавную букву", _borderBrush.Color),
                ValidateFieldsService.ValidatePasswordBoxRegex(PasswordInp, @"[a-z]", "Пароль должен содержать хотя бы одну строчную букву", _borderBrush.Color),
                ValidateFieldsService.ValidatePasswordBoxRegex(PasswordInp, @"\d", "Пароль должен содержать хотя бы цифру", _borderBrush.Color),
                ValidateFieldsService.ValidatePasswordBoxRegex(PasswordInp, @"[!@#$%^&*(),.?""':{}|<>]", "Пароль должен содержать хотя бы один специальный символ", _borderBrush.Color),
                ValidateFieldsService.ValidatePasswordBoxConfirm(ConfirmPasswordInp, PasswordInp, _borderBrush.Color),
                ValidateFieldsService.ValidatePasswordBoxMin(PasswordInp, 8, _borderBrush.Color),
            };
            return checkings.All(x => x == true);
        }

        private bool CheckConfirmPassword()
        {
            var checkings = new List<bool>()
            {
                ValidateFieldsService.ValidatePasswordBoxFilled(ConfirmPasswordInp, "Подтверждение пароля", _borderBrush.Color),
                ValidateFieldsService.ValidatePasswordBoxConfirm(ConfirmPasswordInp, PasswordInp, _borderBrush.Color)
            };
            return checkings.All(x => x == true);
        }

        private bool CheckPassportDataSeria()
        {
            var checkings = new List<bool>()
            {
                ValidateFieldsService.ValidateTextBoxFilled(PassportDataSeriaInp, "Серия", _borderBrush.Color),
                ValidateFieldsService.ValidateTextBoxRegex(PassportDataSeriaInp, @"^\d{4}$", "Серия паспорта может содержать только 4 цифры", _borderBrush.Color)
            };
            return checkings.All(x => x == true);
        }

        private bool CheckPassportDataNumber()
        {
            var checkings = new List<bool>()
            {
                ValidateFieldsService.ValidateTextBoxFilled(PassportDataNumberInp, "Номер", _borderBrush.Color),
                ValidateFieldsService.ValidateTextBoxRegex(PassportDataNumberInp, @"^\d{6}$", "Номер паспорта может содержать только 6 цифр", _borderBrush.Color)
            };
            return checkings.All(x => x == true);
        }

        private bool CheckPassportDataDepartment()
        {
            var checkings = new List<bool>()
            {
                ValidateFieldsService.ValidateTextBoxFilled(PassportDataDepartmentInp, "Выдан", _borderBrush.Color)
            };
            return checkings.All(x => x == true);
        }

        private bool CheckPassportDataDepartmentCode()
        {
            var checkings = new List<bool>()
            {
                ValidateFieldsService.ValidateTextBoxFilled(PassportDataDepartmentCodeInp, "Код подразделения", _borderBrush.Color),
                ValidateFieldsService.ValidateTextBoxRegex(PassportDataDepartmentCodeInp, @"^\d{3}-\d{3}$", "Код подразделения должен быть в формате: XXX-XXX", _borderBrush.Color)
            };
            return checkings.All(x => x == true);
        }

        private void RoleInp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
#if DEBUG
            var role = e.AddedItems[0]?.ToString() ?? string.Empty;
            if (role == "Администратор")
            {
                FullnameInp.Text = "Дьяков Алексей Игоревич";
                EmailInp.Text = "alex.dyakov@example.com";
                PhoneInp.Text = "+79001234567";
                DateOfBirthInp.SelectedDate = new(2004, 8, 23);
                SnilsInp.Text = "123-456-789 00";
                InnInp.Text = "123456789012";
                PasswordInp.Password = "qw*rTy228";
                ConfirmPasswordInp.Password = "qw*rTy228";
                PassportDataSeriaInp.Text = "1234";
                PassportDataNumberInp.Text = "123456";
                PassportDataDepartmentInp.Text = "УФМС России по г. Москва";
                PassportDataDepartmentCodeInp.Text = "770-001";
            }
            else if (role == "Житель")
            {
                FullnameInp.Text = "Смирнов Алексей Борисович";
                EmailInp.Text = "a.smirnov.home@mail.ru";
                PhoneInp.Text = "+79001234574";
                DateOfBirthInp.SelectedDate = new(1975, 4, 12);
                SnilsInp.Text = "130-456-789 00";
                InnInp.Text = "823456789012";
                PasswordInp.Password = "TempPass123!";
                ConfirmPasswordInp.Password = "TempPass123!";
                PassportDataSeriaInp.Text = "4517";
                PassportDataNumberInp.Text = "889900";
                PassportDataDepartmentInp.Text = "УФМС России по г. Москва";
                PassportDataDepartmentCodeInp.Text = "770-008";
            }
#endif
        }
    }
}
