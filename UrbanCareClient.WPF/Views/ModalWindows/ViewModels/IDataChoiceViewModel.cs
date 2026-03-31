using System.Collections.ObjectModel;

namespace UrbanCareClient.WPF.Views.ModalWindows.ViewModels
{
    public interface IDataChoiceViewModel
    {
        void SetSelectedItem(object item);
        object? GetSelectedItem();
        ObservableCollection<object> GetItems();
    }
}
