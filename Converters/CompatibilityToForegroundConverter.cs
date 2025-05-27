using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using VintageStoryModManager.Constants;

namespace VintageStoryModManager.Converters
{
    public class CompatibilityToForegroundConverter : IValueConverter
    {
        public Brush ExactBrush { get; set; } = Brushes.Gold;
        public Brush HighBrush { get; set; } = Brushes.Green;
        public Brush MediumBrush { get; set; } = Brushes.Orange;
        public Brush LowBrush { get; set; } = Brushes.Red;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                switch (value)
                {
                    case ModVersionCompatibility.Exact:
                        return ExactBrush;
                    case ModVersionCompatibility.High:
                        return HighBrush;
                    case ModVersionCompatibility.Medium:
                        return MediumBrush;
                    case ModVersionCompatibility.Low:
                        return LowBrush;
                }
            }

            return Application.Current.Resources["ForegroundColor"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
