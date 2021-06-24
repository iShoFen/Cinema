using System;
using System.Globalization;
using System.Windows.Data;
namespace Appli.Converter
{
    [ValueConversion(typeof(double), typeof(double))]
    public class SingleSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not double size || !float.TryParse((string) parameter, out var f)) return null;

            var result = (size - 80) * f;
            return result > 0 ? result : 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
