using System.Windows;
using System.Windows.Media.Imaging;
using DataEncryption;
using LINQ_to_Data;
using Modele;
using StubLib;
using Microsoft.Web.WebView2.Wpf;

namespace Appli
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public Manager Man { get; } = new(new LinqToXml(new XmlRsa()));
        public BitmapImage Cache { get; set; }
        public WebView2 Web { get; set; }
    }
}