using System;
using System.Globalization;
using System.Windows.Data;
using static Appli.Utils.ConstanteApp;
namespace Appli.Converter
{
    [ValueConversion(typeof(float), typeof(string))]
    public class NoteAvisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                > ZERO_STAR and <= ZERO_H_STAR => $"{HALF}{EMPTY}{EMPTY}{EMPTY}{EMPTY}",

                > ZERO_H_STAR and <= ONE_STAR => $"{STAR}{EMPTY}{EMPTY}{EMPTY}{EMPTY}",

                > ONE_STAR and <= ONE_H_STAR => $"{STAR}{HALF}{EMPTY}{EMPTY}{EMPTY}",

                > ONE_H_STAR and <= TWO_STAR => $"{STAR}{STAR}{EMPTY}{EMPTY}{EMPTY}",

                > TWO_STAR and <= TWO_H_STAR => $"{STAR}{STAR}{HALF}{EMPTY}{EMPTY}",

                > TWO_H_STAR and <= THREE_STAR => $"{STAR}{STAR}{STAR}{EMPTY}{EMPTY}",
                
                > THREE_STAR and <= THREE_H_STAR => $"{STAR}{STAR}{STAR}{HALF}{EMPTY}",
                
                > THREE_H_STAR and <= FOUR_STAR => $"{STAR}{STAR}{STAR}{STAR}{EMPTY}",
                
                > FOUR_STAR and <= FOUR_H_STAR => $"{STAR}{STAR}{STAR}{STAR}{HALF}",
                
                > FOUR_H_STAR and <= FIVE_STAR => $"{STAR}{STAR}{STAR}{STAR}{STAR}",
                
                _ => $"{EMPTY}{EMPTY}{EMPTY}{EMPTY}{EMPTY}",
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}