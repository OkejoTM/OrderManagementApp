using System.Windows;
using OrderManagement.WPF.ViewModels.Base;

namespace OrderManagement.WPF.ViewModels.Dialogs;

public class AddressDialogViewModel : ViewModelBase
{
    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set => SetField(ref _name, value);
    }

    public string Title { get; }
    public bool IsEdit { get; }

    public RelayCommand SaveCommand { get; }
    public RelayCommand CancelCommand { get; }

    public AddressDialogViewModel(string? existingName = null)
    {
        IsEdit = existingName is not null;
        Title = IsEdit ? "Редактировать адрес" : "Новый адрес";
        Name = existingName ?? string.Empty;

        SaveCommand = new RelayCommand(Save, _ => !string.IsNullOrWhiteSpace(Name));
        CancelCommand = new RelayCommand(Cancel);
    }

    private void Save(object? parameter)
    {
        if (parameter is Window window)
        {
            window.DialogResult = true;
            window.Close();
        }
    }

    private void Cancel(object? parameter)
    {
        if (parameter is Window window)
        {
            window.DialogResult = false;
            window.Close();
        }
    }
}