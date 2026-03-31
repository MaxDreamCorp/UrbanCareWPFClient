using System.Windows;
using UrbanCareClient.Application.Services.ApiServices;
using UrbanCareClient.Application.Services.OtherServices;
using UrbanCareClient.WPF.Enums;
using UrbanCareClient.WPF.Services;
using UrbanCareClient.WPF.ViewDTOs;
using UrbanCareClient.WPF.Views.UserControls;
using UrbanCareClient.WPF.Views.UserControls.ViewModels;

namespace UrbanCareClient.WPF.Views.ModalWindows.CardWindows
{
    /// <summary>
    /// Логика взаимодействия для BuildingCardWindow.xaml
    /// </summary>
    public partial class BuildingCardWindow : Window
    {
        private readonly AdministrationService _administrationService;
        private readonly WindowOperations _windowOperation;
        private readonly GetterDIServices _getterDIServices;
        private BuildingViewDTO? _buildingViewDTO;
        private readonly SingleChoiceViewModel<RegionViewDTO> _regionChoiceViewModel;
        private readonly SingleChoiceControl _regionChoice;

        public BuildingCardWindow(AdministrationService administrationService, GetterDIServices getterDIServices, WindowOperations windowOperation, BuildingViewDTO? buildingViewDTO = null)
        {
            InitializeComponent();
            _administrationService = administrationService;
            _getterDIServices = getterDIServices;
            _windowOperation = windowOperation;
            _buildingViewDTO = buildingViewDTO;

            _regionChoiceViewModel = new SingleChoiceViewModel<RegionViewDTO>();
            _regionChoice = _getterDIServices.GetService<SingleChoiceControl>();
            _regionChoice.ViewModel = _regionChoiceViewModel;
            _regionChoice.Type = typeof(RegionViewDTO);
            RegionPanel.Children.Add(_regionChoice);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BuildingTypeInp.ItemsSource = TemporaryDataStorage.BuildingTypes.Select(bt => bt.Type);
            FloorMaterialInp.ItemsSource = TemporaryDataStorage.FloorMaterials.Select(fm => fm.Name);
            WallMaterialInp.ItemsSource = TemporaryDataStorage.WallMaterials.Select(wm => wm.Name);

            if (_windowOperation == WindowOperations.Create)
            {
                CardHeaderTxt.Text = "Создать здание";
                DeleteBtn.IsEnabled = false;
                IdInp.IsReadOnly = true;

                if (TemporaryDataStorage.Buildings != null)
                    IdInp.Text = (TemporaryDataStorage.Buildings.Count > 0 ?
                            TemporaryDataStorage.Buildings.Max(r => r.Id) + 1
                            : 1).ToString();
            }
            else if (_windowOperation == WindowOperations.Edit && _buildingViewDTO != null)
            {
                CardHeaderTxt.Text = $"Здание #{_buildingViewDTO.Number}";
                IdInp.IsReadOnly = true;

                SetFields(_buildingViewDTO);
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_windowOperation == WindowOperations.Create)
            {
                if (!CheckFields())
                {
                    MessageBox.Show("Не все поля заполнены верно", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (_regionChoice.SelectedItem is RegionViewDTO regionResponseDTO)
                {
                    var response = await _administrationService.CreateBuilding(new(
                        NumberInp.Text,
                        AddressInp.Text,
                        regionResponseDTO.Id,
                        TemporaryDataStorage.BuildingTypes.Find(bt => bt.Type == BuildingTypeInp.Text)?.Id ?? throw new Exception(),
                        short.Parse(YearBuiltInp.Text),
                        int.Parse(FloorCountInp.Text),
                        TemporaryDataStorage.WallMaterials.Find(wm => wm.Name == WallMaterialInp.Text)?.Id ?? throw new Exception(),
                        TemporaryDataStorage.FloorMaterials.Find(fm => fm.Name == FloorMaterialInp.Text)?.Id ?? throw new Exception()));

                    if (response == null)
                    {
                        MessageBox.Show($"Здание \"{NumberInp.Text}\" создано", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                    else
                        MessageBox.Show(string.Join("\n", response), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Не выбран регион", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            else if (_windowOperation == WindowOperations.Edit)
            {
                if (!CheckFields())
                {
                    MessageBox.Show("Не все поля заполнены верно", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (_regionChoice.SelectedItem is RegionViewDTO regionResponseDTO)
                {
                    var response = await _administrationService.UpdateBuilding(new(
                        int.Parse(IdInp.Text),
                        NumberInp.Text,
                        AddressInp.Text,
                        regionResponseDTO.Id,
                        TemporaryDataStorage.BuildingTypes.Find(bt => bt.Type == BuildingTypeInp.Text)?.Id ?? throw new Exception(),
                        short.Parse(YearBuiltInp.Text),
                        int.Parse(FloorCountInp.Text),
                        TemporaryDataStorage.WallMaterials.Find(wm => wm.Name == WallMaterialInp.Text)?.Id ?? throw new Exception(),
                        TemporaryDataStorage.FloorMaterials.Find(fm => fm.Name == FloorMaterialInp.Text)?.Id ?? throw new Exception()));

                    if (response == null)
                    {
                        MessageBox.Show($"Здание \"{NumberInp.Text}\" изменено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                    else
                        MessageBox.Show(string.Join("\n", response), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Не выбран регион", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
        }

        private async void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(IdInp.Text);

            if (MessageBox.Show($"Вы уверены, что хотите удалить здание с Id {id}?",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var response = await _administrationService.DeleteBuilding(id);

                if (response.isDeleted)
                {
                    MessageBox.Show($"Здание с Id {id} удалено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                else if (response.error != null)
                    MessageBox.Show(response.error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CheckFields()
        {
            var checkings = new List<bool>{
                !string.IsNullOrEmpty(IdInp.Text),
                !string.IsNullOrEmpty(NumberInp.Text),
                !string.IsNullOrEmpty(AddressInp.Text),
                !string.IsNullOrEmpty(YearBuiltInp.Text),
                !string.IsNullOrEmpty(FloorCountInp.Text),
                short.TryParse(YearBuiltInp.Text, out var _),
                int.TryParse(FloorCountInp.Text, out var _),
                BuildingTypeInp.SelectedIndex > -1,
                WallMaterialInp.SelectedIndex > -1,
                FloorMaterialInp.SelectedIndex > -1,
            };
            return checkings.All(x => x);
        }
        private void SetFields(BuildingViewDTO buildingViewDTO)
        {
            var buildingDTO = TemporaryDataStorage.Buildings?.Find(b => b.Id == buildingViewDTO.Id);
            if (buildingDTO == null) throw new ArgumentNullException(nameof(buildingDTO));

            var regionDTO = buildingDTO.Region;
            if (regionDTO == null) throw new ArgumentNullException(nameof(regionDTO));


            IdInp.Text = buildingDTO.Id.ToString();
            NumberInp.Text = buildingDTO.Number;
            AddressInp.Text = buildingDTO.Address;


            var regionView = ConverterService.RegionToViewDTO(regionDTO);
            _regionChoice.SelectedItem = regionView;
            _regionChoice.SelectedItemTxt.Text = regionDTO.Name;

            BuildingTypeInp.Text = buildingViewDTO.BuildingType;
            YearBuiltInp.Text = buildingDTO.YearBuit.ToString();
            FloorCountInp.Text = buildingDTO.FloorCount.ToString();
            WallMaterialInp.Text = buildingViewDTO.WallMaterial;
            FloorMaterialInp.Text = buildingViewDTO.FloorMaterial;
        }
    }
}
