using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using UrbanCareClient.WPF.Services;
using UrbanCareClient.WPF.Views.ModalWindows;
using UrbanCareClient.WPF.Views.ModalWindows.ViewModels;

namespace UrbanCareClient.WPF.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для SingleChoiceControl.xaml
    /// </summary>
    public partial class SingleChoiceControl : UserControl
    {
        private readonly GetterDIServices _getterDIServices;

        public string LabelText
        {
            get => Label.Text;
            set => Label.Text = value;
        }
        public string SelectedItemName { get; private set; } = string.Empty;
        public object? SelectedItem { get; set; } = null;
        public ObservableCollection<object>? SourceItems { get; set; } = null;
        public Type? Type { get; set; }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(object),
                typeof(SingleChoiceControl), new PropertyMetadata(null, OnViewModelChanged));

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

        public SingleChoiceControl(GetterDIServices getterDIServices)
        {
            InitializeComponent();
            _getterDIServices = getterDIServices;

            this.Loaded += (s, e) =>
            {
                if (ViewModel != null)
                    DataContext = ViewModel;
            };
        }

        private void ChooseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Type == null) return;
            var dataChoiceVM = new DataChoiceViewModel<Type>();
            var dataChoiceModalWindow = _getterDIServices.GetService<DataChoiceModalWindow>();
            if (dataChoiceModalWindow == null) return;

            if (SourceItems != null)
                dataChoiceModalWindow.SourceItems = SourceItems;

            dataChoiceModalWindow.Type = Type;
            dataChoiceModalWindow.ViewModel = dataChoiceVM;
            dataChoiceModalWindow.ShowDialog();

            SelectedItem = dataChoiceModalWindow.SelectedItem;
            SelectedItemName = dataChoiceModalWindow.SelectedItemName;
            SelectedItemTxt.Text = SelectedItemName;
        }
    }
}
