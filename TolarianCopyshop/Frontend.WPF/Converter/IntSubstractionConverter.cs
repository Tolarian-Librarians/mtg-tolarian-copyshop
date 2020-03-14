using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Tolarian.Copyshop.ScreenPresenter.Converter
{
    public class IntSubstractionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double numValue && double.TryParse(parameter as string, out double numPara))
            {
                return numValue - numPara;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double numValue && double.TryParse(parameter as string, out double numPara))
            {
                return numValue + numPara;
            }
            return value;
        }
    }
}
