using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Modele;
using static Appli.Utils.ConstanteApp;

namespace Appli.Converter
{
    [ValueConversion(typeof(object), typeof(BitmapImage))]
    public class CompositeReConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                ObservableCollection<Oeuvre> or ReadOnlyObservableCollection<Oeuvre> => new BitmapImage(new Uri(CROSS)),
                List<Oeuvre> => new BitmapImage(new Uri(ADD)),
                _ => null
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
