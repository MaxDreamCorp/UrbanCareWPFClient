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
    /// Логика взаимодействия для ApartmentCardWindow.xaml
    /// </summary>
    public partial class ApartmentCardWindow : Window
    {
        private readonly AdministrationService _administrationService;
        private readonly WindowOperations _windowOperation;
        private readonly GetterDIServices _getterDIServices;
        private ApartmentViewDTO? _apartmentViewDTO;
        private readonly SingleChoiceViewModel<BuildingViewDTO> _buildingViewModel;
        private readonly SingleChoiceControl _buildingChoice;

        public ApartmentCardWindow(AdministrationService administrationService, GetterDIServices getterDIServices, WindowOperations windowOperation, ApartmentViewDTO? apartmentViewDTO = null)
        {
            InitializeComponent();
            _administrationService = administrationService;
            _windowOperation = windowOperation;
            _getterDIServices = getterDIServices;
            _apartmentViewDTO = apartmentViewDTO;

            _buildingViewModel = new SingleChoiceViewModel<BuildingViewDTO>();
            _buildingChoice = _getterDIServices.GetService<SingleChoiceControl>();
            _buildingChoice.ViewModel = _buildingViewModel;
            _buildingChoice.Type = typeof(BuildingViewDTO);
            BuidingPanel.Children.Add(_buildingChoice);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_windowOperation == WindowOperations.Create)
            {
                CardHeaderTxt.Text = "Создать квартиру";
                DeleteBtn.IsEnabled = false;
                IdInp.IsReadOnly = true;

                if (TemporaryDataStorage.Apartments != null)
                    IdInp.Text = (TemporaryDataStorage.Apartments.Count > 0 ?
                            TemporaryDataStorage.Apartments.Max(r => r.Id) + 1
                            : 1).ToString();
            }
            else if (_windowOperation == WindowOperations.Edit && _apartmentViewDTO != null)
            {
                CardHeaderTxt.Text = $"Квартира #{_apartmentViewDTO.Number}";
                IdInp.IsReadOnly = true;

                SetFields(_apartmentViewDTO);
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

                if (_buildingChoice.SelectedItem is BuildingViewDTO buildingViewDTO)
                {
                    var response = await _administrationService.CreateApartment(new(
                        int.Parse(NumberInp.Text),
                        buildingViewDTO.Id,
                        int.TryParse(EntranceInp.Text, out int entrance) ? entrance : null,
                        int.Parse(FloorInp.Text),
                        int.Parse(RoomCountInp.Text)));

                    if (response == null)
                    {
                        MessageBox.Show($"Квартира \"{NumberInp.Text}\" создана", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                    else
                        MessageBox.Show(string.Join("\n", response), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Не выбрано здание", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
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

                if (_buildingChoice.SelectedItem is BuildingViewDTO buildingViewDTO)
                {
                    var response = await _administrationService.UpdateApartment(new(
                        int.Parse(IdInp.Text),
                        int.Parse(NumberInp.Text),
                        buildingViewDTO.Id,
                        int.TryParse(EntranceInp.Text, out int entrance) ? entrance : null,
                        int.Parse(FloorInp.Text),
                        int.Parse(RoomCountInp.Text)));

                    if (response == null)
                    {
                        MessageBox.Show($"Квартира \"{NumberInp.Text}\" изменена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                    else
                        MessageBox.Show(string.Join("\n", response), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Не выбрано здание", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
        }

        private async void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(IdInp.Text);

            if (MessageBox.Show($"Вы уверены, что хотите удалить квартиру с Id {id}?",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var response = await _administrationService.DeleteApartment(id);

                if (response.isDeleted)
                {
                    MessageBox.Show($"Квартира с Id {id} удален", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
                int.TryParse(NumberInp.Text, out var _),
                int.TryParse(FloorInp.Text, out var _),
                int.TryParse(RoomCountInp.Text, out var _)
            };
            return checkings.All(x => x);
        }

        private void SetFields(ApartmentViewDTO apartmentViewDTO)
        {
            var apartmentDto = TemporaryDataStorage.Apartments?.Find(a => a.Id == apartmentViewDTO.Id);
            if (apartmentDto == null) throw new ArgumentNullException(nameof(apartmentDto));

            var buildingDTO = apartmentDto.Building;

            IdInp.Text = apartmentDto.Id.ToString();
            NumberInp.Text = apartmentDto.Number.ToString();

            var buildingView = ConverterService.BuildingToViewDTO(buildingDTO);
            _buildingChoice.SelectedItem = buildingView;
            _buildingChoice.SelectedItemTxt.Text = buildingDTO.Number;

            EntranceInp.Text = apartmentDto.Entrance.ToString();
            FloorInp.Text = apartmentDto.Floor.ToString();
            RoomCountInp.Text = apartmentDto.RoomCount.ToString();

        }
    }
}
