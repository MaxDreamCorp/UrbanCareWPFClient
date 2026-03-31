using System.Collections.ObjectModel;

namespace UrbanCareClient.WPF.Views.ModalWindows.ViewModels
{
    public class DataChoiceViewModel<T> : IDataChoiceViewModel
    {
        public T? SelectedItem { get; set; }
        public ObservableCollection<T> Items { get; set; } = new ObservableCollection<T>();

        public void SetSelectedItem(object item)
        {
            if (item is T typedItem)
            {
                SelectedItem = typedItem;
            }
        }

        public object? GetSelectedItem() => SelectedItem;

        public ObservableCollection<object> GetItems()
        {
            var collection = new ObservableCollection<object>();
            foreach (var item in Items)
            {
                if (item != null)
                    collection.Add(item);
            }
            return collection;
        }
    }
}
