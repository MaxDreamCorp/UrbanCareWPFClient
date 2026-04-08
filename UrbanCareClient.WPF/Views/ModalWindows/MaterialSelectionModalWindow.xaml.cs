using System.Windows;
using System.Windows.Controls;
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
    /// Логика взаимодействия для MaterialSelectionModalWindow.xaml
    /// </summary>
    public partial class MaterialSelectionModalWindow : Window
    {
        private readonly GetterDIServices _getterDIServices;
        private readonly OrderResponseDTO _orderResponseDTO;
        private readonly CompanyService _companyService;
        private readonly OrderService _orderService;
        private readonly ExecutorService _executorService;
        public event EventHandler? OrderUpdated;

        public MaterialSelectionModalWindow(GetterDIServices getterDIServices, OrderResponseDTO orderResponseDTO)
        {
            InitializeComponent();
            _getterDIServices = getterDIServices;
            _orderResponseDTO = orderResponseDTO;
            _companyService = _getterDIServices.GetService<CompanyService>();
            _orderService = _getterDIServices.GetService<OrderService>();
            _executorService = _getterDIServices.GetService<ExecutorService>();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var orderControl = new MiniOrderControl(_getterDIServices, _orderService);
            orderControl.ViewModel = new OrderControlViewModel { Order = _orderResponseDTO };
            orderControl.ButtonsPanel.Visibility = Visibility.Collapsed;
            orderControl.PaymentPanel.Visibility = Visibility.Collapsed;
            OrderPanel.Children.Add(orderControl);
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

                    bool isExist = false;
                    if (_orderResponseDTO.OrderMaterials != null && _orderResponseDTO.OrderMaterials.Any(om => om.Material.Id == material.Id))
                    {
                        var mat = _orderResponseDTO.OrderMaterials.Find(om => om.Material.Id == material.Id);
                        if (mat != null)
                        {
                            materialControlViewModel.Quantity = mat.Quantity;
                            isExist = true;
                        }
                    }

                    var materialControl = new MaterialControl();
                    materialControl.ViewModel = materialControlViewModel;
                    CheckBox checkBox = new CheckBox
                    {
                        Content = materialControl,
                        Margin = new Thickness(5),
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
                            if (mc.ViewModel.Quantity == 0)
                            {
                                mc.ViewModel.Quantity = 1; // Устанавливаем количество в 1 при выборе
                                mc.QuantityTxt.Text = "1";
                            }

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

                    checkBox.IsChecked = isExist;

                    MaterialsPanel.Children.Add(checkBox);
                }
            }
        }

        private async void SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            var msgboxResult = MessageBox.Show("Вы уверены, что хотите выбрать эти материалы?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (msgboxResult != MessageBoxResult.Yes)
                return;

            var selectedMaterials = new Dictionary<int, int>();

            foreach (var child in MaterialsPanel.Children)
            {
                if (child is CheckBox checkBox && checkBox.IsChecked == true && checkBox.Content is MaterialControl materialControl)
                {
                    var materialId = materialControl.ViewModel.Material.Id;
                    var quantity = materialControl.ViewModel.Quantity;

                    if (quantity > 0)
                    {
                        selectedMaterials[materialId] = quantity;
                    }
                }
            }

            if (selectedMaterials.Count == 0)
                return;

            var cmd = new AddMaterialsToOrderCommand(
                0,
                _orderResponseDTO.Id,
                selectedMaterials);

            var response = await _executorService.AddMaterialsToOrder(cmd);

            if (response != null && response.Count > 0)
            {
                MessageBox.Show(string.Join("\n", response), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                MessageBox.Show("Материалы успешно добавлены к заказу.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                OrderUpdated?.Invoke(this, EventArgs.Empty);
                Close();
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
