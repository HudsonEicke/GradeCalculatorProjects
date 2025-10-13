using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace GradeCalculatorApp.Converters
{
    public class StringToDoubleConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if(value is double d)
                return d.ToString(culture);
            else
                return "";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (double.TryParse(value?.ToString(), NumberStyles.Float, culture, out double result))
                return result;

            return null;
        }
    }
}
