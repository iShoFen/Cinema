using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Modele;
using static Modele.Constante;

namespace Appli.Converter
{
    [ValueConversion(typeof(Dictionary<Personne, string>), typeof(string))]
    public class PersNamesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not Dictionary<Personne, string> personnes || personnes.Count == LISTE_MIN) return null;

            if (personnes.Count == 1)
            {
                return parameter switch
                {
                    ACTEUR => $"{personnes.First().Key.Prenom} {personnes.First().Key.Nom} : {personnes.First().Value}",
                    REALISATEUR => $"{personnes.First().Key.Prenom} {personnes.First().Key.Nom}",
                    _ => null
                };
            }

            var builder = new StringBuilder();

            switch (parameter)
            {
                case ACTEUR:
                {
                    builder.Append($"{personnes.First().Key.Prenom} " +
                                   $"{personnes.First().Key.Nom} : " +
                                   $"{personnes.First().Value}");

                    foreach (var (pers, role) in personnes.Skip(1))
                        builder.Append($"\n{pers.Prenom} {pers.Nom} : {role}");
                            
                    break;
                }

                case REALISATEUR:
                {
                    builder.Append($"{personnes.First().Key.Prenom} " +
                                   $"{personnes.First().Key.Nom}");

                    foreach (var pers in personnes.Keys.Skip(1))
                        builder.Append($"\n{pers.Prenom} {pers.Nom}");

                    break;
                }
            }

            return builder.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
