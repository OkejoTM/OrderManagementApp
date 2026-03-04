using Microsoft.Extensions.DependencyInjection;
using OrderManagement.WPF.ViewModels.Base;

namespace OrderManagement.WPF.Services;

public class NavigationService(IServiceProvider serviceProvider) : ViewModelBase, INavigationService
{
    private readonly Stack<ViewModelBase> _navigationStack = new();

    private ViewModelBase? _currentView;
    public ViewModelBase? CurrentView
    {
        get => _currentView;
        private set => SetField(ref _currentView, value);
    }

    public bool CanGoBack => _navigationStack.Count > 0;

    public void NavigateTo<TViewModel>(object? parameter = null) where TViewModel : ViewModelBase
    {
        if (CurrentView is not null)
        {
            _navigationStack.Push(CurrentView);
        }

        var viewModel = (TViewModel)serviceProvider.GetRequiredService(typeof(TViewModel));

        if (viewModel is IParameterReceiver receiver)
        {
            receiver.ReceiveParameter(parameter);
        }

        CurrentView = viewModel;
    }

    public void GoBack()
    {
        if (!CanGoBack) return;
        CurrentView = _navigationStack.Pop();
    }
}