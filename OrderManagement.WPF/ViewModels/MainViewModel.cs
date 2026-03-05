using OrderManagement.WPF.Services;
using OrderManagement.WPF.ViewModels.Base;

namespace OrderManagement.WPF.ViewModels;

public class MainViewModel : ViewModelBase
{
    public NavigationService NavigationService { get; }

    public MainViewModel(NavigationService navigationService)
    {
        NavigationService = navigationService;
        NavigationService.NavigateTo<AreaListViewModel>();
    }
}