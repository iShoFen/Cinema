using System;
using System.Globalization;
using System.Windows.Data;

namespace Appli.Converter
{
    [ValueConversion(typeof(object), typeof(double))]
    public class MultiSizeConverter : IMultiValueConverter

    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is not int nb || values[1] is not double size || values[2] is not double min ||
                parameter is not string str || !double.TryParse(str, out var r)) return null;

            var result = (size - 60) / Math.Ceiling(nb * r);
            return result < min ? min : result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
