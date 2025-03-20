using System.Globalization;
using System.Windows.Data;

namespace TheCatApp.Presentation.Converters;

class IntToStarsConverter : IValueConverter
{
    private const int MinValue = 0;
    private const int MaxValue = 5;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is int rating && rating >= MinValue && rating <= MaxValue
            ? new string('★', rating) + new string('☆', MaxValue - rating)
            : new string('☆', MaxValue);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
