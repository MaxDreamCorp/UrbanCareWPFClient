using System.Windows;
using System.Windows.Controls;
using UrbanCareClient.Domain.Enums;
using UrbanCareClient.WPF.Services;
using UrbanCareClient.WPF.Views.UserControls.ViewModels;

namespace UrbanCareClient.WPF.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для MiniExecutorControl.xaml
    /// </summary>
    public partial class MiniExecutorControl : UserControl
    {
        public static readonly DependencyProperty ViewModalProperty =
            DependencyProperty.Register(
                nameof(ViewModal),
                typeof(MiniExecutorControlViewModal),
                typeof(MiniExecutorControl),
                new PropertyMetadata(null, OnViewModalChanged));



        public MiniExecutorControlViewModal ViewModal
        {
            get => (MiniExecutorControlViewModal)GetValue(ViewModalProperty);
            set => SetValue(ViewModalProperty, value);
        }
        public MiniExecutorControl()
        {
            InitializeComponent();
        }

        private static void OnViewModalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MiniExecutorControl control && e.NewValue is MiniExecutorControlViewModal viewModal)
            {
                control.LayoutRoot.DataContext = viewModal;

                control.FullnameTxt.Text = viewModal.Executor.EmployeeData.UserData.Fullname;
                control.PositionTxt.Text = viewModal.Executor.EmployeeData.EmployeePosition.Name;
                control.PhoneTxt.Text = viewModal.Executor.EmployeeData.UserData.Phone;
                control.EmailTxt.Text = viewModal.Executor.EmployeeData.UserData.Email;
                control.ActiveOrdersCountTxt.Text = viewModal.Executor.ActiveTasksCount.ToString();
                control.CompletedOrdersCountTxt.Text = viewModal.Executor.CompletedTasksCount.ToString();
                control.StatusTxt.Text = viewModal.Executor.EmployeeData.EmployeeStatus.Status;

                switch ((EmployeeStatusEnum)viewModal.Executor.EmployeeData.EmployeeStatus.Id)
                {
                    case EmployeeStatusEnum.OnOrder:
                        control.StatusBdr.Background = StylesService.PriorityHighBgBrush;
                        control.StatusTxt.Foreground = StylesService.PriorityHighBrush;
                        break;
                    case EmployeeStatusEnum.NotWorking:
                        control.StatusBdr.Background = StylesService.PriorityCriticalBgBrush;
                        control.StatusTxt.Foreground = StylesService.PriorityCriticalBrush;
                        break;
                    case EmployeeStatusEnum.Working:
                        control.StatusBdr.Background = StylesService.PriorityLowBgBrush;
                        control.StatusTxt.Foreground = StylesService.PriorityLowBrush;
                        break;
                    case EmployeeStatusEnum.Available:
                        control.StatusBdr.Background = StylesService.PriorityLowBgBrush;
                        control.StatusTxt.Foreground = StylesService.PriorityLowBrush;
                        break;
                }
            }
        }
    }
}
