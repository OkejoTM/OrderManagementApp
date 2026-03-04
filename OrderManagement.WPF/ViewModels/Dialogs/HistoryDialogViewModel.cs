using System.Windows;
using OrderManagement.Domain.Enums;
using OrderManagement.WPF.ViewModels.Base;

namespace OrderManagement.WPF.ViewModels.Dialogs;

public class HistoryDialogViewModel : ViewModelBase
{
    private DateTime _pumpingDate = DateTime.Today;
    public DateTime PumpingDate
    {
        get => _pumpingDate;
        set => SetField(ref _pumpingDate, value);
    }

    private int _cubeAmount;
    public int CubeAmount
    {
        get => _cubeAmount;
        set => SetField(ref _cubeAmount, value);
    }

    private PaymentType _paymentType;
    public PaymentType PaymentType
    {
        get => _paymentType;
        set => SetField(ref _paymentType, value);
    }

    private double _price;
    public double Price
    {
        get => _price;
        set => SetField(ref _price, value);
    }

    public string Title { get; }
    public bool IsEdit { get; }

    public IReadOnlyList<PaymentType> PaymentTypes { get; } =
        Enum.GetValues<PaymentType>().ToList().AsReadOnly();

    public RelayCommand SaveCommand { get; }
    public RelayCommand CancelCommand { get; }

    public HistoryDialogViewModel(
        DateTime? pumpingDate = null,
        int? cubeAmount = null,
        PaymentType? paymentType = null,
        double? price = null)
    {
        IsEdit = pumpingDate is not null;
        Title = IsEdit ? "Редактировать заказ" : "Новый заказ";

        if (IsEdit)
        {
            PumpingDate = pumpingDate!.Value;
            CubeAmount = cubeAmount!.Value;
            PaymentType = paymentType!.Value;
            Price = price!.Value;
        }

        SaveCommand = new RelayCommand(Save, _ => CubeAmount > 0 && Price > 0);
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