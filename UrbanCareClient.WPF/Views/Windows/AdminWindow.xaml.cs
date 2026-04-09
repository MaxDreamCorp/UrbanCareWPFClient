using System.Windows;
using System.Windows.Controls;
using UrbanCareClient.Application.Services.ApiServices;
using UrbanCareClient.Application.Services.OtherServices;
using UrbanCareClient.WPF.Enums;
using UrbanCareClient.WPF.Interfaces;
using UrbanCareClient.WPF.Services;
using UrbanCareClient.WPF.ViewDTOs;
using UrbanCareClient.WPF.Views.ModalWindows;
using UrbanCareClient.WPF.Views.ModalWindows.CardWindows;

namespace UrbanCareClient.WPF.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private readonly UserService _userService;
        private readonly INavigationService _navigationService;
        private readonly EmployeeService _employeeService;
        private readonly AdministrationService _administrationService;
        private readonly CompanyService _companyService;
        private readonly GetterDIServices _getterDIServices;
        private AdminTabs _currentTab;

        public AdminWindow(UserService userService,
                           INavigationService navigationService,
                           AdministrationService administrationService,
                           EmployeeService employeeService,
                           CompanyService companyService,
                           GetterDIServices getterDIServices)
        {
            InitializeComponent();
            _userService = userService;
            _navigationService = navigationService;
            _administrationService = administrationService;
            _employeeService = employeeService;
            _companyService = companyService;
            _getterDIServices = getterDIServices;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var userData = await _userService.GetMyUserData();
            if (userData == null)
            {
                MessageBox.Show("Ошибка получения данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                var authWindow = _navigationService.GetWindow<LogInModalWindow>();
                authWindow.Show();
                Close();
                return;
            }
            TemporaryDataStorage.CurrentUserId = userData.Id;

            var employeeDataResponse = await _employeeService.GetMyEmployee();
            while (employeeDataResponse == null)
            {
                MessageBox.Show("Вам необходимо заполнить данные работника", "", MessageBoxButton.OK, MessageBoxImage.Information);
                EmployeeCreatingModalWindow employeeCreatingModalWindow = _getterDIServices.GetService<EmployeeCreatingModalWindow>();
                employeeCreatingModalWindow = _getterDIServices.GetService<EmployeeCreatingModalWindow>();
                employeeCreatingModalWindow.ShowDialog();
                employeeDataResponse = await _employeeService.GetMyEmployee();
            }

            TemporaryDataStorage.EmployeeData = employeeDataResponse;
            TemporaryDataStorage.ManagementCompany = employeeDataResponse.ManagementCompany;

            var response = await _employeeService.UpdateStatusToWorking();
            if (response != null)
            {
                MessageBox.Show(string.Join("\n", response), "", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            }

            MCNameTxt.Text = $"УК: {TemporaryDataStorage.EmployeeData.ManagementCompany.Name}";
            FullNameTxt.Text = TemporaryDataStorage.EmployeeData.UserData.Fullname;
            PositionTxt.Text = TemporaryDataStorage.EmployeeData.EmployeePosition.Name;

            var employees = await _administrationService.GetMyManagementCompanyEmployees();
            dg.ItemsSource = employees;
            _currentTab = AdminTabs.Employees;
            AddBtn.IsEnabled = false;
        }

        private async void RegionsSwitcher_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Child is StackPanel stackPanel && TemporaryDataStorage.EmployeeData != null)
            {
                WorkAreaName.Text = string.Empty;
                foreach (var item in stackPanel.Children)
                {
                    if (item is TextBlock textBlock)
                    {
                        WorkAreaName.Text = textBlock.Text;
                        break;
                    }
                }

                AddBtn.IsEnabled = true;

                List<RegionViewDTO> regionViews = await GetRegionViewDTOsFromApiAsync();

                dg.ItemsSource = regionViews.OrderBy(r => r.Id).ToList();

                _currentTab = AdminTabs.Regions;
            }
        }

        private async void BuildingsSwitcher_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Child is StackPanel stackPanel && TemporaryDataStorage.EmployeeData != null)
            {
                WorkAreaName.Text = string.Empty;
                foreach (var item in stackPanel.Children)
                {
                    if (item is TextBlock textBlock)
                    {
                        WorkAreaName.Text = textBlock.Text;
                        break;
                    }
                }

                AddBtn.IsEnabled = true;

                List<BuildingViewDTO> buildingViews = await GetBuildingViewDTOsFromApiAsync();

                dg.ItemsSource = buildingViews.OrderBy(b => b.Id).ToList();

                _currentTab = AdminTabs.Buildings;
            }
        }

        private async void ApartmentsSwitcher_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Child is StackPanel stackPanel && TemporaryDataStorage.EmployeeData != null)
            {
                WorkAreaName.Text = string.Empty;
                foreach (var item in stackPanel.Children)
                {
                    if (item is TextBlock textBlock)
                    {
                        WorkAreaName.Text = textBlock.Text;
                        break;
                    }
                }

                AddBtn.IsEnabled = true;

                List<ApartmentViewDTO> apartmentViews = await GetApartmentViewDTOsFromApiAsync();

                dg.ItemsSource = apartmentViews.OrderBy(a => a.Id).ToList();

                _currentTab = AdminTabs.Apartments;
            }
        }

        private async void MCEmployeesSwitcher_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Child is StackPanel stackPanel)
            {
                WorkAreaName.Text = string.Empty;
                foreach (var item in stackPanel.Children)
                {
                    if (item is TextBlock textBlock)
                    {
                        WorkAreaName.Text = textBlock.Text;
                        break;
                    }
                }

                AddBtn.IsEnabled = false;

                var employees = await _administrationService.GetMyManagementCompanyEmployees();
                dg.ItemsSource = employees;
                _currentTab = AdminTabs.Employees;
            }
        }

        private async void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_currentTab == AdminTabs.Regions)
            {
                var regionCard = new RegionCardWindow(_administrationService, WindowOperations.Create);
                regionCard.ShowDialog();

                List<RegionViewDTO> regionViews = await GetRegionViewDTOsFromApiAsync();

                dg.ItemsSource = regionViews.OrderBy(r => r.Id).ToList();
            }
            else if (_currentTab == AdminTabs.Buildings)
            {
                var buildingCard = new BuildingCardWindow(_administrationService, _getterDIServices, WindowOperations.Create);
                buildingCard.ShowDialog();

                List<BuildingViewDTO> buildingViews = await GetBuildingViewDTOsFromApiAsync();

                dg.ItemsSource = buildingViews.OrderBy(b => b.Id).ToList();
            }
            else if (_currentTab == AdminTabs.Apartments)
            {
                var apartmentCard = new ApartmentCardWindow(_administrationService, _getterDIServices, WindowOperations.Create);
                apartmentCard.ShowDialog();

                List<ApartmentViewDTO> apartmentViews = await GetApartmentViewDTOsFromApiAsync();

                dg.ItemsSource = apartmentViews.OrderBy(b => b.Id).ToList();
            }
        }

        private async void dg_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            var editingItem = e.Row.Item;
            e.Cancel = true;

            if (editingItem is RegionViewDTO regionViewDTO)
            {
                var regionCard = new RegionCardWindow(_administrationService, WindowOperations.Edit, regionViewDTO);
                regionCard.ShowDialog();

                TemporaryDataStorage.Regions = null;
                List<RegionViewDTO> regionViews = await GetRegionViewDTOsFromApiAsync();

                dg.ItemsSource = regionViews.OrderBy(r => r.Id);
            }
            else if (editingItem is BuildingViewDTO buildingViewDTO)
            {
                var buildingCard = new BuildingCardWindow(_administrationService, _getterDIServices, WindowOperations.Edit, buildingViewDTO);
                buildingCard.ShowDialog();

                List<BuildingViewDTO> buildingViews = await GetBuildingViewDTOsFromApiAsync();

                dg.ItemsSource = buildingViews.OrderBy(b => b.Id);
            }
            else if (editingItem is ApartmentViewDTO apartmentViewDTO)
            {
                var apartmentCard = new ApartmentCardWindow(_administrationService, _getterDIServices, WindowOperations.Edit, apartmentViewDTO);
                apartmentCard.ShowDialog();

                List<ApartmentViewDTO> apartmentViews = await GetApartmentViewDTOsFromApiAsync();

                dg.ItemsSource = apartmentViews.OrderBy(a => a.Id).ToList();
            }
        }

        private async Task<List<RegionViewDTO>> GetRegionViewDTOsFromApiAsync()
        {
            if (TemporaryDataStorage.EmployeeData == null) return new();
            var response = await _companyService.GetRegionsByManagementCompany(TemporaryDataStorage.EmployeeData.ManagementCompany.Id);

            if (response.errors != null)
                MessageBox.Show(string.Join("\n", response.errors), "", MessageBoxButton.OK, MessageBoxImage.Error);

            return ConverterService.RegionsToViewDTOs(TemporaryDataStorage.Regions);
        }

        private async Task<List<BuildingViewDTO>> GetBuildingViewDTOsFromApiAsync()
        {
            if (TemporaryDataStorage.EmployeeData == null) return new();
            var response = await _companyService.GetBuildingsByManagementCompany(TemporaryDataStorage.EmployeeData.ManagementCompany.Id);

            if (response.errors != null)
                MessageBox.Show(string.Join("\n", response.errors), "", MessageBoxButton.OK, MessageBoxImage.Error);

            return ConverterService.BuildingsToViewDTOs(TemporaryDataStorage.Buildings);
        }

        private async Task<List<ApartmentViewDTO>> GetApartmentViewDTOsFromApiAsync()
        {
            if (TemporaryDataStorage.EmployeeData == null) return new();
            var response = await _companyService.GetApartmentsByManagmentCompany(TemporaryDataStorage.EmployeeData.ManagementCompany.Id);

            if (response.errors != null)
                MessageBox.Show(string.Join("\n", response.errors), "", MessageBoxButton.OK, MessageBoxImage.Error);

            return ConverterService.ApartmentsToViewDTOs(TemporaryDataStorage.Apartments);
        }

       
    }
}
