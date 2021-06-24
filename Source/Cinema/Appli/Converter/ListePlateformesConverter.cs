using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using Modele;

namespace Appli.Converter
{
    [ValueConversion(typeof(object), typeof(bool))]
    public class ListePlateformesConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values[0] is string str && values[1] is List<Plateformes> list
                ? list.Contains(Enum.Parse<Plateformes>(str))
                : null;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
