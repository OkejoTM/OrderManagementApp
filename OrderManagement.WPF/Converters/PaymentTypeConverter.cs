using System.Globalization;
using System.Windows.Data;
using OrderManagement.Domain.Enums;

namespace OrderManagement.WPF.Converters;

public class PaymentTypeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is PaymentType paymentType
            ? paymentType switch
            {
                PaymentType.Cash => "Наличные",
                PaymentType.Transfer => "Перевод",
                _ => value.ToString()!
            }
            : string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is string str
            ? str switch
            {
                "Наличные" => PaymentType.Cash,
                "Перевод" => PaymentType.Transfer,
                _ => PaymentType.Cash
            }
            : PaymentType.Cash;
    }
}