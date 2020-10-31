using System;
using System.Globalization;
using System.Windows.Data;

namespace Tolarian.Copyshop.Fontend.WPF.Converter
{
    public class NumericMultiplicationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int multiplier && double.TryParse(parameter as string, out double multiplicand))
            {
                return multiplier * multiplicand;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int multiplier && double.TryParse(parameter as string, out double multiplicand))
            {
                return multiplier / multiplicand;
            }
            return value;
        }
    }
}
