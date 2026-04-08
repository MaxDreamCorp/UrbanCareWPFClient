using System.Windows;
using System.Windows.Controls;
using UrbanCareClient.WPF.Views.UserControls.ViewModels;

namespace UrbanCareClient.WPF.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для MaterialControl.xaml
    /// </summary>
    public partial class MaterialControl : UserControl
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                nameof(ViewModel),
                typeof(MaterialControlViewModel),
                typeof(MaterialControl),
                new PropertyMetadata(null, OnViewModelChanged));

        public MaterialControlViewModel ViewModel
        {
            get => (MaterialControlViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public MaterialControl()
        {
            InitializeComponent();
        }

        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MaterialControl control && e.NewValue is MaterialControlViewModel materialControlViewModel)
            {
                control.LayoutRoot.DataContext = e.NewValue;

                control.IdTxt.Text = materialControlViewModel.Material.Id.ToString();
                control.NameTxt.Text = materialControlViewModel.Material.Name;
                control.StorageTxt.Text = materialControlViewModel.Material.Storage.Name;
                control.AmountAtStorageTxt.Text = materialControlViewModel.Material.AmountAtStorage.ToString();
                control.PriceTxt.Text = materialControlViewModel.Material.Price.ToString("C");
                control.UnitTxt.Text = materialControlViewModel.Material.Unit;
                control.QuantityTxt.Text = materialControlViewModel.Quantity.ToString();
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Quantity++;
            QuantityTxt.Text = ViewModel.Quantity.ToString();
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Quantity > 0)
                ViewModel.Quantity--;
            QuantityTxt.Text = ViewModel.Quantity.ToString();
        }
    }
}
