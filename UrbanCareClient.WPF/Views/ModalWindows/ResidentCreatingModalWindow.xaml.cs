using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UrbanCareClient.Application.Services.ApiServices;
using UrbanCareClient.Application.Services.OtherServices;
using UrbanCareClient.Domain.Commands;
using UrbanCareClient.WPF.Services;
using UrbanCareClient.WPF.ViewDTOs;
using UrbanCareClient.WPF.Views.UserControls;
using UrbanCareClient.WPF.Views.UserControls.ViewModels;

namespace UrbanCareClient.WPF.Views.ModalWindows
{
    /// <summary>
    /// Логика взаимодействия для ResidentCreatingModalWindow.xaml
    /// </summary>
    public partial class ResidentCreatingModalWindow : Window
    {
        public bool IsReady { get; private set; }
        private readonly GetterDIServices _getterDIServices;
        private readonly CompanyService _companyService;
        private readonly ResidentService _residentService;
        private readonly SingleChoiceViewModel<RegionViewDTO> _regionChoiceVM;
        private readonly SingleChoiceControl _regionChoice;
        private readonly SingleChoiceViewModel<BuildingViewDTO> _buildingChoiceVM;
        private readonly SingleChoiceControl _buildingChoice;
        private readonly SingleChoiceViewModel<ApartmentViewDTO> _apartmentChoiceVM;
        private readonly SingleChoiceControl _apartmentChoice;
        private readonly SolidColorBrush _borderBrush;

        public ResidentCreatingModalWindow(GetterDIServices getterDIServices, CompanyService companyService, ResidentService residentService)
        {
            InitializeComponent();
            _companyService = companyService;
            _getterDIServices = getterDIServices;
            _residentService = residentService;
            _borderBrush = (SolidColorBrush)System.Windows.Application.Current.Resources["BorderBrush"];

            _regionChoiceVM = new SingleChoiceViewModel<RegionViewDTO>
            {
                LabelText = "Регион:"
            };
            _buildingChoiceVM = new SingleChoiceViewModel<BuildingViewDTO>
            {
                LabelText = "Здание:"
            };
            _apartmentChoiceVM = new SingleChoiceViewModel<ApartmentViewDTO>
            {
                LabelText = "Квартира:"
            };

            _regionChoice = _getterDIServices.GetService<SingleChoiceControl>();
            _buildingChoice = _getterDIServices.GetService<SingleChoiceControl>();
            _apartmentChoice = _getterDIServices.GetService<SingleChoiceControl>();

            _regionChoice.Type = typeof(RegionViewDTO);
            _buildingChoice.Type = typeof(BuildingViewDTO);
            _apartmentChoice.Type = typeof(ApartmentViewDTO);

            RegionPanel.Children.Add(_regionChoice);
            BuildingPanel.Children.Add(_buildingChoice);
            ApartmentPanel.Children.Add(_apartmentChoice);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var response = await _companyService.GetAllRegions();

            if (response.errors != null)
            {
                MessageBox.Show(string.Join("\n", response.errors), "", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }

            List<RegionViewDTO> regions = ConverterService.RegionsToViewDTOs(response.regions);

            _regionChoice.SourceItems = new(regions);

            _regionChoice.SelectedItemTxt.TextChanged += async (sender, e) =>
            {
                if (sender is TextBox tb)
                {
                    if (string.IsNullOrEmpty(tb.Text))
                    {
                        _buildingChoice.SelectedItem = null;
                        _buildingChoice.SelectedItemTxt.Text = string.Empty;
                        _buildingChoice.SourceItems = new(new List<BuildingViewDTO>());
                        return;
                    }

                    if (_regionChoice.SelectedItem != null && _regionChoice.SelectedItem is RegionViewDTO regionViewDTO)
                    {
                        var response = await _companyService.GetBuildingsByRegion(regionViewDTO.Id);

                        if (response.errors != null)
                        {
                            MessageBox.Show(string.Join("\n", response.errors), "", MessageBoxButton.OK, MessageBoxImage.Error);
                            Close();
                        }

                        List<BuildingViewDTO> buildings = ConverterService.BuildingsToViewDTOs(response.buildings);

                        _buildingChoice.SelectedItem = null;
                        _buildingChoice.SelectedItemTxt.Text = string.Empty;
                        _buildingChoice.SourceItems = new(buildings);
                    }
                }
            };

            _buildingChoice.SourceItems = new(new List<BuildingViewDTO>());

            _buildingChoice.SelectedItemTxt.TextChanged += async (sender, e) =>
            {
                if (sender is TextBox tb)
                {
                    if (string.IsNullOrEmpty(tb.Text))
                    {
                        _apartmentChoice.SelectedItem = null;
                        _apartmentChoice.SelectedItemTxt.Text = string.Empty;
                        _apartmentChoice.SourceItems = new(new List<ApartmentViewDTO>());
                        return;
                    }

                    if (_buildingChoice.SelectedItem != null && _buildingChoice.SelectedItem is BuildingViewDTO buildingViewDTO)
                    {
                        var response = await _companyService.GetApartmentsByBuilding(buildingViewDTO.Id);

                        if (response.errors != null)
                        {
                            MessageBox.Show(string.Join("\n", response.errors), "", MessageBoxButton.OK, MessageBoxImage.Error);
                            Close();
                        }

                        List<ApartmentViewDTO> apartments = ConverterService.ApartmentsToViewDTOs(response.apartments);

                        _apartmentChoice.SelectedItem = null;
                        _apartmentChoice.SelectedItemTxt.Text = string.Empty;
                        _apartmentChoice.SourceItems = new(apartments);
                    }
                }
            };

            _apartmentChoice.SourceItems = new(new List<ApartmentViewDTO>());

            MovingIntoDateInp.DisplayDateEnd = DateTime.UtcNow;
        }

        private void MovingIntoDateInp_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void RegistrateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TemporaryDataStorage.CurrentUserId == 0)
                return;

            if (MovingIntoDateInp.SelectedDate == null)
            {
                MessageBox.Show("Необходимо заполнить дату въезда", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_regionChoice.SelectedItem is RegionViewDTO regionViewDTO)
            {
                if (_buildingChoice.SelectedItem is BuildingViewDTO buildingViewDTO)
                {
                    if (_apartmentChoice.SelectedItem is ApartmentViewDTO apartmentViewDTO)
                    {
                        DateOnly movingIntoDate = DateOnly.FromDateTime(MovingIntoDateInp.SelectedDate.Value);

                        var cmd = new CreateResidentCommand(TemporaryDataStorage.CurrentUserId, apartmentViewDTO.Id, movingIntoDate);

                        var response = await _residentService.CreateResident(cmd);

                        if (response != null)
                        {
                            MessageBox.Show(string.Join('\n', response), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        MessageBox.Show("Регистрация жителя прошла успешно", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        IsReady = true;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Необходимо выбрать квартиру", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Необходимо выбрать здание", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Необходимо выбрать регион", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }
    }
}
