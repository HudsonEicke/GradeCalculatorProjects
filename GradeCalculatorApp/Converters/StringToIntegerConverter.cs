using System;
using System.Globalization;
using Avalonia.Data.Converters;


namespace GradeCalculatorApp.Converters
{
    public class StringToIntegerConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int i)
                return i.ToString(culture);
            else
                return "";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (int.TryParse(value?.ToString(), NumberStyles.Float, culture, out int result))
                return result;

            return null;
        }
    }
}
