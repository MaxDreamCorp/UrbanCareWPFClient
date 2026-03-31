using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UrbanCareClient.Application.Services.ApiServices;
using UrbanCareClient.Application.Services.OtherServices;
using UrbanCareClient.Domain.DTOs;
using UrbanCareClient.WPF.Services;
using UrbanCareClient.WPF.ViewDTOs;
using UrbanCareClient.WPF.Views.ModalWindows.ViewModels;
using UrbanCareClient.WPF.Views.UserControls;

namespace UrbanCareClient.WPF.Views.ModalWindows
{
    /// <summary>
    /// Логика взаимодействия для DataChoiceModalWindow.xaml
    /// </summary>
    public partial class DataChoiceModalWindow : Window
    {
        private readonly UserService _userService;
        private readonly CompanyService _companyService;

        public Type? Type { get; set; }

        public string SelectedItemName { get; private set; } = string.Empty;
        public object? SelectedItem { get; set; }
        public ObservableCollection<object>? SourceItems { get; set; } = null;


        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(object),
                typeof(DataChoiceModalWindow), new PropertyMetadata(null, OnViewModelChanged));

        public object ViewModel
        {
            get { return GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SingleChoiceControl control)
                control.DataContext = e.NewValue;
        }

        public DataChoiceModalWindow(UserService userService, CompanyService companyService)
        {
            InitializeComponent();
            _userService = userService;
            _companyService = companyService;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
                DataContext = ViewModel;

            if (SourceItems != null)
            {
                dg.ItemsSource = SourceItems;
                return;
            }

            if (Type == typeof(ManagementCompanyResponseDTO))
            {
                var companies = await _userService.GetAllManagementCompanies();
                dg.ItemsSource = companies;
            }
            else if (Type == typeof(RegionViewDTO))
            {
                if (TemporaryDataStorage.ManagementCompany != null)
                {
                    var regionsDTOs = await _companyService.GetRegionsByManagementCompany(TemporaryDataStorage.ManagementCompany.Id);
                    var regions = ConverterService.RegionsToViewDTOs(regionsDTOs.regions);
                    dg.ItemsSource = regions;

                }
            }
            else if (Type == typeof(BuildingViewDTO))
            {
                if (TemporaryDataStorage.ManagementCompany != null)
                {
                    var buildingsDTOs = await _companyService.GetBuildingsByManagementCompany(TemporaryDataStorage.ManagementCompany.Id);
                    var buildings = ConverterService.BuildingsToViewDTOs(buildingsDTOs.buildings);
                    dg.ItemsSource = buildings;

                }
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ChooseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel is IDataChoiceViewModel choiceViewModel && this.Type != null)
            {
                SelectedItem = dg.SelectedItem;
                if (SelectedItem is BuildingViewDTO buildingViewDTO)
                    SelectedItemName = buildingViewDTO.Address;
                else
                    SelectedItemName = GetCellText(dg, dg.SelectedIndex, 1);

                Close();
            }
        }

        private void dg_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        public string GetCellText(DataGrid dg, int rowIndex, int columnIndex)
        {
            if (dg == null || rowIndex < 0 || columnIndex < 0 || dg.Columns.Count <= columnIndex)
                return string.Empty;

            string result = string.Empty;

            result = GetCellTextFromVisualTree(dg, rowIndex, columnIndex);
            if (!string.IsNullOrEmpty(result))
                return result;


            return result;
        }

        private T? FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild)
                    return typedChild;

                var descendant = FindVisualChild<T>(child);
                if (descendant != null)
                    return descendant;
            }
            return null;
        }

        private List<T> GetVisualChildCollection<T>(DependencyObject parent) where T : DependencyObject
        {
            var children = new List<T>();

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild)
                {
                    children.Add(typedChild);
                }

                children.AddRange(GetVisualChildCollection<T>(child));
            }

            return children;
        }
        public string GetCellTextFromVisualTree(DataGrid dg, int rowIndex, int columnIndex)
        {
            try
            {
                var row = GetDataGridRow(dg, rowIndex);
                if (row == null) return string.Empty;

                var cell = GetDataGridCell(row, columnIndex);
                if (cell == null) return string.Empty;

                var textBlock = FindVisualChild<TextBlock>(cell);
                return textBlock?.Text ?? string.Empty;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка: {ex.Message}");
                return string.Empty;
            }
        }

        private DataGridRow GetDataGridRow(DataGrid dg, int index)
        {
            var itemsControl = dg;
            var itemContainerGenerator = itemsControl.ItemContainerGenerator;

            var row = itemContainerGenerator.ContainerFromIndex(index) as DataGridRow;

            if (row == null)
            {
                var rows = GetVisualChildCollection<DataGridRow>(dg);
                if (index < rows.Count)
                    row = rows[index];
            }

            return row ?? throw new Exception();
        }

        private DataGridCell? GetDataGridCell(DataGridRow row, int columnIndex)
        {
            if (row == null) return null;

            var cells = GetVisualChildCollection<DataGridCell>(row);
            if (columnIndex < cells.Count)
                return cells[columnIndex];

            return null;
        }
    }
}
