using System;
using System.Globalization;
using System.Windows.Data;

namespace Tolarian.Copyshop.Fontend.WPF.Converter
{
    public class NumericSubstractionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double minuend && double.TryParse(parameter as string, out double subtrahend))
            {
                return minuend - subtrahend;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double minuend && double.TryParse(parameter as string, out double subtrahend))
            {
                return minuend + subtrahend;
            }
            return value;
        }
    }
}