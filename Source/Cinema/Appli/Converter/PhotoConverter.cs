using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using static Appli.Utils.ConstanteApp;

namespace Appli.Converter
{ 
    [ValueConversion(typeof(string),typeof(BitmapImage))]
    public  class PhotoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            FileStream stream;
            try { stream =  new FileStream(Path.Combine(Directory.GetCurrentDirectory(), (value as string)!), FileMode.Open); }
            catch (Exception)
            {
                return parameter switch
                {
                    USER => new BitmapImage(new Uri(PROFIL, UriKind.Absolute)),
                    OEUVRE_PARAM => new BitmapImage(new Uri(OEUVRE, UriKind.Absolute)),
                    PERSONNE_PARAM => new BitmapImage(new Uri(PERSONNE, UriKind.Absolute)),
                    _ => null
                };
            }

            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = stream;
            image.EndInit();
            stream.Dispose();

            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
