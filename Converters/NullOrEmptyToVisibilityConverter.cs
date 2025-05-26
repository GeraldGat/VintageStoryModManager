using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace VintageStoryModManager.Converters
{
    public class NullOrEmptyToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string strValue)
            {
                return string.IsNullOrEmpty(strValue) ? Visibility.Collapsed : Visibility.Visible;
            }
            return Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
