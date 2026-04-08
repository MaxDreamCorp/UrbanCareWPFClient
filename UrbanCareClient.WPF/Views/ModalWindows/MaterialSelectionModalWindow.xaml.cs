using System.Windows;
using System.Windows.Controls;
using UrbanCareClient.Application.Services.ApiServices;
using UrbanCareClient.Application.Services.OtherServices;
using UrbanCareClient.WPF.Services;
using UrbanCareClient.WPF.Views.UserControls;

namespace UrbanCareClient.WPF.Views.ModalWindows
{
    /// <summary>
    /// Логика взаимодействия для MaterialSelectionModalWindow.xaml
    /// </summary>
    public partial class MaterialSelectionModalWindow : Window
    {
        private readonly GetterDIServices _getterDIServices;
        private readonly CompanyService _companyService;

        public MaterialSelectionModalWindow(GetterDIServices getterDIServices)
        {
            InitializeComponent();
            _getterDIServices = getterDIServices;
            _companyService = _getterDIServices.GetService<CompanyService>();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (TemporaryDataStorage.ManagementCompany == null)
                return;

            var response = await _companyService.GetMaterialsByManagementCompany(TemporaryDataStorage.ManagementCompany.Id);
            if (response.errors != null && response.errors.Count > 0)
            {
                MessageBox.Show(string.Join("\n", response.errors), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (response.materials != null)
            {
                foreach (var material in response.materials)
                {
                    var materialControlViewModel = new UserControls.ViewModels.MaterialControlViewModel
                    {
                        Material = material,
                        Quantity = 0
                    };
                    var materialControl = new MaterialControl();
                    materialControl.ViewModel = materialControlViewModel;
                    CheckBox checkBox = new CheckBox
                    {
                        Content = materialControl,
                        Margin = new Thickness(5)
                    };

                    checkBox.SizeChanged += (s, args) =>
                    {
                        if (checkBox.Content is MaterialControl mc)
                        {
                            mc.Width = checkBox.ActualWidth - 5; // Учитываем отступы
                        }
                    };

                    checkBox.Checked += (s, args) =>
                    {
                        if (checkBox.Content is MaterialControl mc)
                        {
                            mc.ViewModel.Quantity = 1; // Устанавливаем количество в 1 при выборе
                            mc.QuantityTxt.Text = "1";

                            mc.AddBtn.IsEnabled = true;
                            mc.RemoveBtn.IsEnabled = true;
                        }
                    };

                    checkBox.Unchecked += (s, args) =>
                    {
                        if (checkBox.Content is MaterialControl mc)
                        {
                            mc.ViewModel.Quantity = 0; // Сбрас��ваем количество при снятии выбора
                            mc.QuantityTxt.Text = "0";

                            mc.AddBtn.IsEnabled = false;
                            mc.RemoveBtn.IsEnabled = false;
                        }
                    };

                    MaterialsPanel.Children.Add(checkBox);
                }
            }
        }

        private void SelectBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
