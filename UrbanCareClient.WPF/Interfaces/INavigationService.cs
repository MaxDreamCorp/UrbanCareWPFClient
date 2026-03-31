using System.Windows;

namespace UrbanCareClient.WPF.Interfaces
{
    public interface INavigationService
    {
        T GetWindow<T>() where T : Window;
    }
}
