using System.Windows;
using UrbanCareClient.Application.Services.ApiServices;
using UrbanCareClient.Application.Services.OtherServices;
using UrbanCareClient.WPF.Enums;
using UrbanCareClient.WPF.ViewDTOs;

namespace UrbanCareClient.WPF.Views.ModalWindows.CardWindows
{
    /// <summary>
    /// Логика взаимодействия для RegionCardWindow.xaml
    /// </summary>
    public partial class RegionCardWindow : Window
    {
        private readonly AdministrationService _administrationService;
        private readonly WindowOperations _windowOperation;
        private RegionViewDTO? _regionViewDTO;

        public RegionCardWindow(AdministrationService administrationService, WindowOperations windowOperation, RegionViewDTO? regionViewDTO = null)
        {
            InitializeComponent();
            _windowOperation = windowOperation;
            _administrationService = administrationService;
            _regionViewDTO = regionViewDTO;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (TemporaryDataStorage.ManagementCompany != null)
                MCInp.Text = TemporaryDataStorage.ManagementCompany.Name;

            if (_windowOperation == WindowOperations.Create)
            {
                CardHeaderTxt.Text = "Создать регион";
                DeleteBtn.IsEnabled = false;
                IdInp.IsReadOnly = true;

                if (TemporaryDataStorage.Regions != null)
                    IdInp.Text = (TemporaryDataStorage.Regions.Count > 0 ?
                            TemporaryDataStorage.Regions.Max(r => r.Id) + 1
                            : 1).ToString();
            }
            else if (_windowOperation == WindowOperations.Edit && _regionViewDTO != null)
            {
                CardHeaderTxt.Text = $"Регион \"{_regionViewDTO.Name}\"";
                IdInp.IsReadOnly = true;

                SetFields(_regionViewDTO);
            }
        }


        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TemporaryDataStorage.ManagementCompany == null) return;

            if (_windowOperation == WindowOperations.Create)
            {
                if (!CheckFields())
                {
                    MessageBox.Show("Не все поля заполнены", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var response = await _administrationService.CreateRegion(new(
                    TemporaryDataStorage.ManagementCompany.Id,
                    NameInp.Text,
                    CommonAddressInp.Text));

                if (response == null)
                {
                    MessageBox.Show($"Регион \"{NameInp.Text}\" создан", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                else
                    MessageBox.Show(string.Join("\n", response), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (_windowOperation == WindowOperations.Edit)
            {
                if (!CheckFields())
                {
                    MessageBox.Show("Не все поля заполнены", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var response = await _administrationService.UpdateRegion(new(
                    int.Parse(IdInp.Text),
                    NameInp.Text,
                    CommonAddressInp.Text,
                    TemporaryDataStorage.ManagementCompany.Id));

                if (response == null)
                {
                    Close();
                }
                else
                    MessageBox.Show(string.Join("\n", response), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(IdInp.Text);

            if (MessageBox.Show($"Вы уверены, что хотите удалить регион с Id {id}?",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var response = await _administrationService.DeleteRegion(id);

                if (response.isDeleted)
                {
                    MessageBox.Show($"Регион с Id {id} удален", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                else if (response.error != null)
                    MessageBox.Show(response.error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CheckFields()
        {
            var checkings = new List<bool>
            {
                !string.IsNullOrEmpty(NameInp.Text),
                !string.IsNullOrEmpty(CommonAddressInp.Text),
                !string.IsNullOrEmpty(MCInp.Text),
            };
            return checkings.All(x => x);
        }

        private void SetFields(RegionViewDTO regionViewDTO)
        {
            IdInp.Text = regionViewDTO.Id.ToString();
            NameInp.Text = regionViewDTO.Name;
            CommonAddressInp.Text = regionViewDTO.CommonAddress;
        }
    }
}
