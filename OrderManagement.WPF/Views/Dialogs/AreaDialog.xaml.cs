using System.Windows;

namespace OrderManagement.WPF.Views.Dialogs;

public partial class AreaDialog : Window
{
    public AreaDialog()
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