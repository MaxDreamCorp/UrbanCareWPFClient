namespace UrbanCareClient.WPF.Views.UserControls.ViewModels
{
    public class SingleChoiceViewModel<T>
    {
        public T? SelectedItem { get; set; }
        public string DisplayMemberPath { get; set; } = string.Empty;
        public string LabelText { get; set; } = string.Empty;
    }
}