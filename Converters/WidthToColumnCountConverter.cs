using System.Globalization;
using System.Reflection.Metadata;
using System.Windows.Data;

namespace VintageStoryModManager.Converters
{
    public class WidthToColumnCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not double width || parameter is not string param)
                return 1;

            var parameters = ParseParameters(param);

            if (parameters.ContainsKey("minColumnWidth"))
            {
                return Math.Ceiling(width / int.Parse(parameters["minColumnWidth"]));
            }

            return 1;
        }

        private Dictionary<string, string> ParseParameters(string parametersString)
        {
            return parametersString
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Split('=', 2))
                .Where(p => p.Length == 2)
                .ToDictionary(p => p[0].Trim(), p => p[1].Trim())
            ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
