using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Tolarian.Copyshop.Fontend.WPF.Converter
{
    class LeftMarginPercentageConverter : IValueConverter
    {

        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double leftMargin && double.TryParse(parameter as string, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double percentage))
            {
                return new Thickness(leftMargin * percentage, 0, 0, 0);
            }
            return value;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
            => null;
    }
}