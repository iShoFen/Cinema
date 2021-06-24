using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using static Appli.Utils.ConstanteApp;

namespace Appli.Converter
{
    [ValueConversion(typeof(object), typeof(Visibility))]
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter switch
            {
                USER => value is not null ? Visibility.Visible : Visibility.Hidden,
                OEUVRE_PARAM => value is null ? Visibility.Visible : Visibility.Hidden,
                _ => value is true ? Visibility.Visible : Visibility.Hidden
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
