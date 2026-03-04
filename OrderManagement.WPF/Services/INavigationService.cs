using OrderManagement.WPF.ViewModels.Base;

namespace OrderManagement.WPF.Services;

public interface INavigationService
{
    void NavigateTo<TViewModel>(object? parameter = null) where TViewModel : ViewModelBase;
    void GoBack();
    bool CanGoBack { get; }
}