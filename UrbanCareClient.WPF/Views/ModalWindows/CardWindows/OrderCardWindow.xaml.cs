using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UrbanCareClient.Application.Services.ApiServices;
using UrbanCareClient.Application.Services.OtherServices;
using UrbanCareClient.Domain.Commands;
using UrbanCareClient.Domain.DTOs;
using UrbanCareClient.WPF.Enums;
using UrbanCareClient.WPF.Services;
using UrbanCareClient.WPF.ViewDTOs;
using UrbanCareClient.WPF.Views.UserControls;
using UrbanCareClient.WPF.Views.UserControls.ViewModels;

namespace UrbanCareClient.WPF.Views.ModalWindows.CardWindows
{
    /// <summary>
    /// Логика взаимодействия для OrderCardWindow.xaml
    /// </summary>
    public partial class OrderCardWindow : Window
    {
        private readonly GetterDIServices _getterDIServices;
        private readonly OrderService _orderService;
        private readonly SolidColorBrush _borderBrush;
        private readonly WindowOperations _windowOperation;
        public OrderResponseDTO? OrderResponseDTO { get; private set; }
        private readonly SingleChoiceViewModel<BuildingViewDTO> _buildingChoiceVM;
        private readonly SingleChoiceControl _buildingChoice;
        private readonly SingleChoiceViewModel<ApartmentViewDTO> _apartmentChoiceVM;
        private readonly SingleChoiceControl _apartmentChoice;

        public OrderCardWindow(GetterDIServices getterDIServices, OrderService orderService, WindowOperations windowOperation, OrderResponseDTO? orderResponseDTO = null)
        {
            InitializeComponent();
            _getterDIServices = getterDIServices;
            _orderService = orderService;
            _windowOperation = windowOperation;
            OrderResponseDTO = orderResponseDTO;
            _borderBrush = (SolidColorBrush)System.Windows.Application.Current.Resources["BorderBrush"];


            _buildingChoiceVM = new SingleChoiceViewModel<BuildingViewDTO>
            {
                LabelText = "Здание:"
            };
            _apartmentChoiceVM = new SingleChoiceViewModel<ApartmentViewDTO>
            {
                LabelText = "Квартира:"
            };

            _buildingChoice = _getterDIServices.GetService<SingleChoiceControl>();
            _apartmentChoice = _getterDIServices.GetService<SingleChoiceControl>();

            _buildingChoice.ViewModel = _buildingChoiceVM;
            _buildingChoice.Type = typeof(BuildingViewDTO);

            _apartmentChoice.ViewModel = _apartmentChoiceVM;
            _apartmentChoice.Type = typeof(ApartmentViewDTO);

            if (TemporaryDataStorage.ResidentData != null)
            {
                var myBuildingViewDTO = ConverterService.BuildingToViewDTO(TemporaryDataStorage.ResidentData.Apartment.Building);
                _buildingChoice.SelectedItem = myBuildingViewDTO;
            }

            BuildingPanel.Children.Add(_buildingChoice);
            ApartmentPanel.Children.Add(_apartmentChoice);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TypeInp.ItemsSource = TemporaryDataStorage.OrderTypes.Select(x => x.Type).ToList();
            PriorityyInp.ItemsSource = TemporaryDataStorage.Priorities.Select(x => x.Priority).ToList();

            if (_windowOperation == WindowOperations.Create)
                SetForCreating();
            else if (_windowOperation == WindowOperations.Edit)
                SetFieldsForEditing();
            else if (_windowOperation == WindowOperations.Read)
                SetFieldsForReadOnly();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TypeInp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TypeInp.SelectedItem == null)
                CategoryInp.ItemsSource = null;
            else
                CategoryInp.ItemsSource = TemporaryDataStorage.OrderCategories
                    .Where(oc => oc.OrderType.Type == TypeInp.SelectedItem.ToString())
                    .Select(x => x.Category).ToList();
        }

        private async void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_windowOperation == WindowOperations.Create)
                await CreateOrder();
            else if (_windowOperation == WindowOperations.Edit)
                await UpdateOrder();
        }

        private async Task CreateOrder()
        {
            if (!CheckAllFields() || TemporaryDataStorage.ResidentData == null)
            {
                MessageBox.Show("Не все поля заполнены", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_buildingChoice.SelectedItem is BuildingViewDTO buildingViewDTO)
            {
                ApartmentViewDTO? apartmentViewDTO = null;
                if (_apartmentChoice.SelectedItem is ApartmentViewDTO avd)
                    apartmentViewDTO = avd;

                var cmd = new CreateOrderCommand(
                    TemporaryDataStorage.ResidentData.Id,
                    DescriptionInp.Text,
                    TemporaryDataStorage.OrderCategories.First(oc => oc.Category == CategoryInp.Text).Id,
                    buildingViewDTO.Id,
                    apartmentViewDTO?.Id,
                    TemporaryDataStorage.Priorities.First(p => p.Priority == PriorityyInp.Text).Id,
                    ContactPhoneInp.Text,
                    ContactEmailInp.Text);

                var response = await _orderService.CreateOrder(cmd);

                if (response == null)
                {
                    MessageBox.Show($"Заказ #{IdInp.Text} создан", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                else
                    MessageBox.Show(string.Join("\n", response), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task UpdateOrder()
        {
            if (!CheckAllFields() || TemporaryDataStorage.ResidentData == null || OrderResponseDTO == null)
            {
                MessageBox.Show("Не все поля заполнены", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_buildingChoice.SelectedItem is BuildingViewDTO buildingViewDTO)
            {
                ApartmentViewDTO? apartmentViewDTO = null;
                if (_apartmentChoice.SelectedItem is ApartmentViewDTO avd)
                    apartmentViewDTO = avd;

                var cmd = new UpdateOrderFromResident(
                    OrderResponseDTO.Id,
                    DescriptionInp.Text,
                    TemporaryDataStorage.OrderCategories.First(oc => oc.Category == CategoryInp.Text).Id,
                    buildingViewDTO.Id,
                    apartmentViewDTO?.Id,
                    TemporaryDataStorage.Priorities.First(p => p.Priority == PriorityyInp.Text).Id,
                    ContactPhoneInp.Text,
                    ContactEmailInp.Text);

                var response = await _orderService.UpdateOrder(cmd);

                if (response == null)
                {
                    MessageBox.Show($"Заказ #{IdInp.Text} обновлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.OrderResponseDTO = new OrderResponseDTO(
                        OrderResponseDTO.Id,
                        OrderResponseDTO.Resident,
                        DescriptionInp.Text,
                        TemporaryDataStorage.OrderCategories.First(oc => oc.Category == CategoryInp.Text),
                        TemporaryDataStorage.ResidentData.Apartment.Building,
                        apartmentViewDTO != null ? TemporaryDataStorage.ResidentData.Apartment : null,
                        TemporaryDataStorage.Priorities.First(p => p.Priority == PriorityyInp.Text),
                        ContactPhoneInp.Text,
                        ContactEmailInp.Text,
                        OrderResponseDTO.OrderStatus,
                        OrderResponseDTO.Dispatcher,
                        OrderResponseDTO.CreatedAt,
                        DateTime.UtcNow,
                        OrderResponseDTO.AcceptedAt,
                        OrderResponseDTO.CompletedAt,
                        OrderResponseDTO.OrderExecutors,
                        OrderResponseDTO.OrderMaterials);
                    Close();
                }
                else
                    MessageBox.Show(string.Join("\n", response), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetForCreating()
        {
            if (TemporaryDataStorage.ResidentData == null) return;

            if (TemporaryDataStorage.MyOrders != null && TemporaryDataStorage.MyOrders.Count > 0)
                IdInp.Text = (TemporaryDataStorage.MyOrders.Max(x => x.Id) + 1).ToString();
            else
                IdInp.Text = "1";

            ResidentInp.Text = TemporaryDataStorage.ResidentData.UserData.Fullname;
            ContactEmailInp.Text = TemporaryDataStorage.ResidentData.UserData.Email;
            ContactPhoneInp.Text = TemporaryDataStorage.ResidentData.UserData.Phone;

            var myBuildingViewDTO = ConverterService.BuildingToViewDTO(TemporaryDataStorage.ResidentData.Apartment.Building);

            _buildingChoice.SourceItems = new() { myBuildingViewDTO };
            _buildingChoice.SelectedItemTxt.Text = myBuildingViewDTO.Address;

            _buildingChoice.SelectedItemTxt.TextChanged += (sender, e) =>
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
                        var apartment = ConverterService.ApartmentToViewDTO(TemporaryDataStorage.ResidentData.Apartment);

                        _apartmentChoice.SelectedItem = null;
                        _apartmentChoice.SelectedItemTxt.Text = string.Empty;
                        _apartmentChoice.SourceItems = new() { apartment };
                    }
                }
            };

            var _myApartmentViewDTO = ConverterService.ApartmentToViewDTO(TemporaryDataStorage.ResidentData.Apartment);
            _apartmentChoice.SourceItems = new() { _myApartmentViewDTO };
        }

        private bool CheckAllFields()
        {
            var checking = new List<bool>
            {
                !string.IsNullOrEmpty(IdInp.Text),
                !string.IsNullOrEmpty(ResidentInp.Text),
                !string.IsNullOrEmpty(TypeInp.Text),
                !string.IsNullOrEmpty(CategoryInp.Text),
                !string.IsNullOrEmpty(PriorityyInp.Text),
                !string.IsNullOrEmpty(_buildingChoice.SelectedItemTxt.Text),
                ValidateFieldsService.ValidateTextBoxRegex(ContactPhoneInp, @"^\+?[1-9]\d{10,14}$", "Некорректный формат телефона", _borderBrush.Color),
                  ValidateFieldsService.ValidateTextBoxRegex(ContactEmailInp, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", "Некорректный формат email", _borderBrush.Color)
            };
            return checking.All(x => x);
        }

        private void SetFieldsForEditing()
        {
            if (OrderResponseDTO == null || TemporaryDataStorage.ResidentData == null) return;

            IdInp.Text = OrderResponseDTO.Id.ToString();
            CardHeaderTxt.Text = $"Заказ #{OrderResponseDTO.Id}";
            ResidentInp.Text = OrderResponseDTO.Resident.UserData.Fullname;
            ContactEmailInp.Text = OrderResponseDTO.ContactEmail;
            ContactPhoneInp.Text = OrderResponseDTO.ContactPhone;
            DescriptionInp.Text = OrderResponseDTO.Description;
            TypeInp.SelectedItem = OrderResponseDTO.OrderCategory.OrderType.Type;
            CategoryInp.SelectedItem = OrderResponseDTO.OrderCategory.Category;
            PriorityyInp.SelectedItem = OrderResponseDTO.Priority.Priority;

            var buildingViewDTO = ConverterService.BuildingToViewDTO(OrderResponseDTO.Building);
            _buildingChoice.SourceItems = new() { buildingViewDTO };
            _buildingChoice.SelectedItem = buildingViewDTO;
            _buildingChoice.SelectedItemTxt.Text = buildingViewDTO.Address;

            _buildingChoice.SelectedItemTxt.TextChanged += (sender, e) =>
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
                        var apartment = ConverterService.ApartmentToViewDTO(TemporaryDataStorage.ResidentData.Apartment);

                        _apartmentChoice.SelectedItem = null;
                        _apartmentChoice.SelectedItemTxt.Text = string.Empty;
                        _apartmentChoice.SourceItems = new() { apartment };
                    }
                }
            };

            if (OrderResponseDTO.Apartment != null)
            {
                var apartmentViewDTO = ConverterService.ApartmentToViewDTO(OrderResponseDTO.Apartment);
                _apartmentChoice.SourceItems = new() { apartmentViewDTO };
                _apartmentChoice.SelectedItem = apartmentViewDTO;
                _apartmentChoice.SelectedItemTxt.Text = $"кв. {apartmentViewDTO.Number}";
            }
        }

        private void SetFieldsForReadOnly()
        {
            if (OrderResponseDTO == null) return;
            CardHeaderTxt.Text = $"Заказ #{OrderResponseDTO.Id}";
            IdInp.Text = OrderResponseDTO.Id.ToString();
            ResidentInp.Text = OrderResponseDTO.Resident.UserData.Fullname;
            ContactEmailInp.Text = OrderResponseDTO.ContactEmail;
            ContactPhoneInp.Text = OrderResponseDTO.ContactPhone;
            DescriptionInp.Text = OrderResponseDTO.Description;
            TypeInp.Text = OrderResponseDTO.OrderCategory.OrderType.Type;
            CategoryInp.Text = OrderResponseDTO.OrderCategory.Category;
            PriorityyInp.Text = OrderResponseDTO.Priority.Priority;

            var buildingViewDTO = ConverterService.BuildingToViewDTO(OrderResponseDTO.Building);
            _buildingChoice.SourceItems = new() { buildingViewDTO };
            _buildingChoice.SelectedItem = buildingViewDTO;
            _buildingChoice.SelectedItemTxt.Text = buildingViewDTO.Address;

            if (OrderResponseDTO.Apartment != null)
            {
                var apartmentViewDTO = ConverterService.ApartmentToViewDTO(OrderResponseDTO.Apartment);
                _apartmentChoice.SourceItems = new() { apartmentViewDTO };
                _apartmentChoice.SelectedItem = apartmentViewDTO;
                _apartmentChoice.SelectedItemTxt.Text = $"кв. {apartmentViewDTO.Number}";
            }

            IdInp.IsEnabled = false;
            ResidentInp.IsEnabled = false;
            ContactEmailInp.IsEnabled = false;
            ContactPhoneInp.IsEnabled = false;
            DescriptionInp.IsEnabled = false;
            TypeInp.IsReadOnly = true;
            CategoryInp.IsReadOnly = true;
            PriorityyInp.IsReadOnly = true;
            _buildingChoice.IsEnabled = false;
            _apartmentChoice.IsEnabled = false;
            SaveBtn.Visibility = Visibility.Collapsed;
        }
    }
}