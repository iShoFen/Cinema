using System.Windows;
using System.Windows.Controls;
using Appli.Navigator;
using Modele;

namespace Appli
{
    /// <summary>
    /// Logique d'interaction pour TousThemes.xaml
    /// </summary>
    public partial class TousThemes : UserControl
    {
        private static Manager Man => (Application.Current as App)?.Man;

        public TousThemes()
        {
            InitializeComponent();
            DataContext = Manager.RendreTousThemes();
        }

        private void Oeuvres_OnClick(object sender, RoutedEventArgs e) => GeneralNavigator.ThemeSelected(sender);
    }
}
