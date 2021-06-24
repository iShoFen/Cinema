using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Modele;
using static Appli.Utils.ConstanteApp;

namespace Appli.Converter
{
    [ValueConversion(typeof(object), typeof(BitmapImage))]
    public class ListeEnvieConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) =>
            values[1] is Oeuvre currentOeuvre && values[0] is ReadOnlyCollection<Oeuvre> list && list.Contains(currentOeuvre)
                ? new BitmapImage(new Uri(CHECK))
                : new BitmapImage(new Uri(ADD));

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
