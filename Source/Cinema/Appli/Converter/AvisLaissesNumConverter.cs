using System;
using System.Globalization;
using System.Windows.Data;
using static Appli.Utils.ConstanteApp;

namespace Appli.Converter
{
    [ValueConversion(typeof(int), typeof(string))]
    public class AvisLaissesNumConverter : IValueConverter
    {
        

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is int ? $"Avis laissés ({value})" : DEFAULT;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
