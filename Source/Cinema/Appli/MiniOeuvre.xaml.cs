using System.Windows;
using System.Windows.Controls;

namespace Appli
{
    /// <summary>
    /// Logique d'interaction pour MiniOeuvre.xaml
    /// </summary>
    public partial class MiniOeuvre : UserControl
    {
        public MiniOeuvre()
        {
            InitializeComponent();
        }

        public string Titre
        {
            get => (string) GetValue(TitreProperty);
            set => SetValue(TitreProperty, value);
        }

        public static readonly DependencyProperty TitreProperty = DependencyProperty.Register(
            nameof(Titre), typeof(string), typeof(MiniOeuvre), new PropertyMetadata(default(string)));

        public string Texte
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Texte), typeof(string), typeof(MiniOeuvre), new PropertyMetadata(default(string)));

        public string Image
        {
            get => (string) GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
            nameof(Image), typeof(string), typeof(MiniOeuvre), new PropertyMetadata(default(string)));

    }
}
