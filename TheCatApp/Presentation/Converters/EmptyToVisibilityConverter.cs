using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TheCatApp.Presentation.Converters;

class EmptyToVisibilityConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length == 2 && values[0] is string text && values[1] is bool isFocused)
        {
            return string.IsNullOrEmpty(text) && isFocused == false
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
        return Visibility.Visible;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
