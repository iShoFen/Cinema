using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using Modele;
using static Modele.Constante;

namespace Appli.Converter
{
    [ValueConversion(typeof(object), typeof(object))]
    public class PersonnesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter switch
            {
                ACTEUR => value is KeyValuePair<Personne, string>(var pers, var role)
                    ? $"{pers.Prenom} {pers.Nom} : {role}"
                    : null,

                REALISATEUR => value is KeyValuePair<Personne, string>(var pers, _)
                    ? $"{pers.Prenom} {pers.Nom}"
                    : null,

                "Liste" => value is IEnumerable<KeyValuePair<Personne, string>> list &&
                           list.ToDictionary(pair => pair.Key, pair => pair.Value).Count == 1
                    ? Orientation.Horizontal
                    : Orientation.Vertical,

                _ => null
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
