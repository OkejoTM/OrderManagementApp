using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace OrderManagement.WPF.Converters;

public class DoubleInputConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is double d and not 0
            ? d.ToString(CultureInfo.CurrentCulture)
            : string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string str || string.IsNullOrWhiteSpace(str))
            return 0.0;

        var normalized = str.Replace(',', '.');

        // Промежуточный ввод: заканчивается на точку или несколько точек — не трогаем VM
        if (normalized.EndsWith('.') || normalized.Split('.').Length > 2)
            return DependencyProperty.UnsetValue;

        return double.TryParse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture, out var result)
            ? result
            : DependencyProperty.UnsetValue;
    }
}