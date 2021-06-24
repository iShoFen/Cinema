using System.Windows;
using System.Windows.Controls;

namespace Appli
{
    /// <summary>
    /// Logique d'interaction pour Avis.xaml
    /// </summary>
    public partial class Avis : UserControl
    {
        public Avis()
        {
            InitializeComponent();
        }

        public event RoutedEventHandler ButtonOnClick
        {
            add => DeleteButton.Click += value;
            remove => DeleteButton.Click -= value;
        }

        public string Pseudo
        {
            get => (string) GetValue(PseudoProperty);
            set => SetValue(PseudoProperty, value);
        }

        public static readonly DependencyProperty PseudoProperty = DependencyProperty.Register(
            nameof(Pseudo), typeof(string), typeof(Avis), new PropertyMetadata(default(string)));

        public float Note
        {
            get => (float) GetValue(NoteProperty);
            set => SetValue(NoteProperty, value);
        }

        public static readonly DependencyProperty NoteProperty = DependencyProperty.Register(
            nameof(Note), typeof(float), typeof(Avis), new PropertyMetadata(default(float)));

        public string Image
        {
            get => (string) GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
            nameof(Image), typeof(string), typeof(Avis), new PropertyMetadata(default(string)));
        
        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text), typeof(string), typeof(Avis), new PropertyMetadata(default(string)));

        public new Visibility Visibility
        {
            get => (Visibility) GetValue(VisibilityProperty);
            set => SetValue(VisibilityProperty, value);
        }

        public new static readonly DependencyProperty VisibilityProperty = DependencyProperty.Register(
            nameof(Visibility), typeof(Visibility), typeof(Avis), new PropertyMetadata(default(Visibility)));
    }
}
