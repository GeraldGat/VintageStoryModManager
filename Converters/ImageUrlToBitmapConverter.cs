using System.Globalization;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Data;

namespace VintageStoryModManager.Converters
{
    public class ImageUrlToBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string path && !string.IsNullOrWhiteSpace(path))
            {
                try
                {
                    return new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
                }
                catch { }
            }

            if (parameter is string defaultImage)
            {
                try
                {
                    return new BitmapImage(new Uri(defaultImage, UriKind.RelativeOrAbsolute));
                }
                catch { }
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
