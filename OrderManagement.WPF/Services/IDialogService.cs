using OrderManagement.WPF.ViewModels.Base;

namespace OrderManagement.WPF.Services;

public interface IDialogService
{
    bool? ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : ViewModelBase;
    bool Confirm(string message, string title = "Подтверждение");
}