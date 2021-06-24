using System.Windows;
using System.Windows.Controls;

namespace Appli
{
    /// <summary>
    /// Logique d'interaction pour AvisUser.xaml
    /// </summary>
    public partial class AvisUser : UserControl
    {
        public AvisUser()
        {
            InitializeComponent();
        }

        public string Pseudo
        {
            get => (string) GetValue(PseudoProperty);
            set => SetValue(PseudoProperty, value);
        }
        public static readonly DependencyProperty PseudoProperty = DependencyProperty.Register(
            nameof(Pseudo), typeof(string), typeof(AvisUser), new PropertyMetadata(default(string)));

        public float Note
        {
            get => (float) GetValue(NoteProperty);
            set => SetValue(NoteProperty, value);
        }
        public static readonly DependencyProperty NoteProperty = DependencyProperty.Register(
            nameof(Note), typeof(float), typeof(AvisUser), new PropertyMetadata(default(float)));

        public string Image
        {
            get => (string) GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
            nameof(Image), typeof(string), typeof(AvisUser), new PropertyMetadata(default(string)));

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text), typeof(string), typeof(AvisUser), new PropertyMetadata(default(string)));
    }
}
