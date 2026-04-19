using System.Windows;

namespace OrderManagement.WPF.Views.Dialogs;

public partial class HistoryDialog : Window
{
    public HistoryDialog()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        CubeAmountTextBox.SelectAll();
        CubeAmountTextBox.Focus();
    }
}