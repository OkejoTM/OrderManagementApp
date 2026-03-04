using System.Windows;
using OrderManagement.WPF.ViewModels.Base;
using OrderManagement.WPF.ViewModels.Dialogs;
using OrderManagement.WPF.Views.Dialogs;

namespace OrderManagement.WPF.Services;

public class DialogService : IDialogService
{
    public bool? ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : ViewModelBase
    {
        Window dialog = viewModel switch
        {
            AddressDialogViewModel => new AddressDialog(),
            HistoryDialogViewModel => new HistoryDialog(),
            _ => throw new ArgumentException($"No dialog registered for {typeof(TViewModel).Name}")
        };

        dialog.DataContext = viewModel;
        dialog.Owner = System.Windows.Application.Current.MainWindow;
        dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;

        return dialog.ShowDialog();
    }

    public bool Confirm(string message, string title = "Подтверждение")
    {
        var result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        return result == MessageBoxResult.Yes;
    }
}