using System;
using System.Globalization;
using System.Windows.Data;
using static Appli.Utils.ConstanteApp;

namespace Appli.Converter
{
    [ValueConversion(typeof(string),typeof(string))]
    public class ConnexionNullNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string name) return name;

            return CONNEXION;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
