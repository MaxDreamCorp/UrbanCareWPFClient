using System.Windows;
using System.Windows.Media;
using UrbanCareClient.Application.Services.ApiServices;
using UrbanCareClient.Application.Services.OtherServices;
using UrbanCareClient.Domain.Commands;
using UrbanCareClient.Domain.DTOs;
using UrbanCareClient.WPF.Services;
using UrbanCareClient.WPF.Views.UserControls;
using UrbanCareClient.WPF.Views.UserControls.ViewModels;

namespace UrbanCareClient.WPF.Views.ModalWindows
{
    /// <summary>
    /// Логика взаимодействия для EmployeeCreatingModalWindow.xaml
    /// </summary>
    public partial class EmployeeCreatingModalWindow : Window
    {
        public bool IsReady { get; private set; }
        private readonly GetterDIServices _getterDIServices;
        private readonly EmployeeService _employeeService;
        private readonly SingleChoiceViewModel<ManagementCompanyResponseDTO> _managementCompanyChoiceVM;
        private readonly SingleChoiceControl _managementCompanyChoice;
        private readonly SolidColorBrush _borderBrush;

        public EmployeeCreatingModalWindow(GetterDIServices getterDIServices, EmployeeService employeeService)
        {
            InitializeComponent();
            _getterDIServices = getterDIServices;
            _borderBrush = (SolidColorBrush)System.Windows.Application.Current.Resources["BorderBrush"];


            _managementCompanyChoiceVM = new SingleChoiceViewModel<ManagementCompanyResponseDTO>
            {
                LabelText = "Управляющая компания:"
            };
            _managementCompanyChoice = _getterDIServices.GetService<SingleChoiceControl>();
            _managementCompanyChoice.ViewModel = _managementCompanyChoiceVM;
            _managementCompanyChoice.Type = typeof(ManagementCompanyResponseDTO);
            _managementCompanyChoice.Margin = new(0, 10, 0, 0);
            MCPanel.Children.Add(_managementCompanyChoice);
            _employeeService = employeeService;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var positions = await _employeeService.GetEmployeePositions();
            var qualCategories = await _employeeService.GetQualificationCategoriesNames();

            PositionInp.ItemsSource = positions.Select(x => x.Name);
            QualCategoryInp.ItemsSource = qualCategories.Select(x => x.Name);
        }

        private void ExpInp_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CheckExp();
        }

        private void SalaryInp_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CheckSalary();
        }

        private void EmploymentDateInp_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            CheckEmploymentDate();
        }

        private async void RegistrateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckAllFields() || EmploymentDateInp.SelectedDate == null || TemporaryDataStorage.CurrentUserId < 0) return;
            if (_managementCompanyChoice.SelectedItem is ManagementCompanyResponseDTO mc)
            {
                DateOnly employmentDate = DateOnly.FromDateTime(EmploymentDateInp.SelectedDate.Value);
                int expYear = int.Parse(ExpInp.Text);
                int salary = int.Parse(SalaryInp.Text);

                var employeeCreatingDTO = new EmployeeCreateRequestDTO(
                        TemporaryDataStorage.CurrentUserId,
                        mc.Id,
                        PositionInp.SelectedIndex + 1,
                        QualCategoryInp.SelectedIndex + 1,
                        employmentDate,
                        expYear,
                        salary,
                        NotesInp.Text);

                if (PositionInp.SelectedIndex == 0)
                {
                    var response = await _employeeService.CreateAdmin(new CreateAdminCommand(employeeCreatingDTO));

                    if (response != null)
                        MessageBox.Show(string.Join('\n', response), "Ошибка регистрации работника", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        MessageBox.Show("Регистрация работника прошла успешно", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        IsReady = true;
                        Close();
                    }
                }
                else if (PositionInp.SelectedIndex == 2)
                {
                    var response = await _employeeService.CreateDispatcher(new CreateDispatcherCommand(employeeCreatingDTO));

                    if (response != null)
                        MessageBox.Show(string.Join('\n', response), "Ошибка регистрации работника", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        MessageBox.Show("Регистрация работника прошла успешно", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        IsReady = true;
                        Close();
                    }
                }
            }
        }
        private bool CheckAllFields()
        {
            var checkings = new List<bool>
            {
                CheckMC(),
                CheckEmployeePosition(),
                CheckQualCategory(),
                CheckEmploymentDate(),
                CheckExp(),
                CheckSalary()
            };

            return checkings.All(x => x);
        }

        private bool CheckMC()
        {
            return ValidateFieldsService.ValidateTextBoxFilled(_managementCompanyChoice.SelectedItemTxt, "Управляющая компания", _borderBrush.Color);
        }

        private bool CheckEmployeePosition()
        {
            return ValidateFieldsService.ValidateComboBoxFilled(PositionInp, "Должность", _borderBrush.Color);
        }

        private bool CheckQualCategory()
        {
            return ValidateFieldsService.ValidateComboBoxFilled(QualCategoryInp, "Категория", _borderBrush.Color);
        }

        private bool CheckEmploymentDate()
        {
            var checkings = new List<bool>()
            {
                ValidateFieldsService.ValidateDatePickerFilled(EmploymentDateInp, "Дата найма", _borderBrush.Color),
                ValidateFieldsService.ValidateDatePickerRange(EmploymentDateInp, "Дата найма", DateTime.UtcNow.AddYears(-100), DateTime.UtcNow, _borderBrush.Color)
            };

            return checkings.All(x => x);
        }

        private bool CheckExp()
        {
            return ValidateFieldsService.ValidateTextBoxRegex(ExpInp, @"^(?!0+(\.0+)?$)\d+(\.\d+)?$", "Опыт работы (годы) может содержать только положительное число", _borderBrush.Color);
        }

        private bool CheckSalary()
        {
            return ValidateFieldsService.ValidateTextBoxRegex(SalaryInp, @"^(?!0+(\.0+)?$)\d+(\.\d+)?$", "Опыт работы (годы) может содержать только положительное число", _borderBrush.Color);
        }
    }
}
