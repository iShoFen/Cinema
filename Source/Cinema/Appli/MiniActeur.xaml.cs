using System;
using System.Windows;
using System.Windows.Controls;

namespace Appli
{
    /// <summary>
    /// Logique d'interaction pour MiniActeur.xaml
    /// </summary>
    public partial class MiniActeur : UserControl
    {
        public MiniActeur()
        {
            InitializeComponent();
        }

        public string Nom
        {
            get => (string) GetValue(NomProperty);
            set => SetValue(NomProperty, value);
        }

        public static readonly DependencyProperty NomProperty = DependencyProperty.Register(
            nameof(Nom), typeof(string), typeof(MiniActeur), new PropertyMetadata(default(string)));

        public string Prenom
        {
            get => (string) GetValue(PrenomProperty);
            set => SetValue(PrenomProperty, value);
        }

        public static readonly DependencyProperty PrenomProperty = DependencyProperty.Register(
            nameof(Prenom), typeof(string), typeof(MiniActeur), new PropertyMetadata(default(string)));

        public DateTime Date
        {
            get => (DateTime) GetValue(DateProperty);
            set => SetValue(DateProperty, value);
        }

        public static readonly DependencyProperty DateProperty = DependencyProperty.Register(
            nameof(Date), typeof(DateTime), typeof(MiniActeur), new PropertyMetadata(default(DateTime)));

        public string Image
        {
            get => (string) GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
            nameof(Image), typeof(string), typeof(MiniActeur), new PropertyMetadata(default(string)));
    }
}