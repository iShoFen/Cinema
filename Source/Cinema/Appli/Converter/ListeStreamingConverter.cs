using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Modele;

namespace Appli.Converter
{
    [ValueConversion(typeof(object), typeof(string))]
    public class ListeStreamingConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is not string str || values[1] is not IEnumerable<Modele.Streaming> list) return null;

            var stream = list.SingleOrDefault(st => st.Plateforme.Equals(Enum.Parse<Plateformes>(str)));

            return stream?.Lien;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
