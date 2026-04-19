using System.Windows;

namespace OrderManagement.WPF.Views.Dialogs;

public partial class AddressDialog : Window
{
    public AddressDialog()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        NameTextBox.SelectAll();
        NameTextBox.Focus();
    }
}