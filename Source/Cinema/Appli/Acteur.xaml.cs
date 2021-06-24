using System.Windows;
using System.Windows.Controls;
using Appli.Navigator;
using MahApps.Metro.Controls;
using Modele;

namespace Appli
{
    /// <summary>
    /// Logique d'interaction pour Acteur.xaml
    /// </summary>
    public partial class Acteur : UserControl
    {
        private static Manager Man => (Application.Current as App)?.Man;

        public Acteur()
        {
            InitializeComponent();
            DataContext = Man;
            Setting.DataContext = Man.ConnectedUser;
            Oeuvres.DataContext = Manager.RendreListeOeuvresActeur(Man.ConnectedUser, Man.CurrentPersonne);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e) => new AjouterPersonne().ShowDialog();

        private void Oeuvres_OnSelectionChanged(object sender, SelectionChangedEventArgs e) =>
            GeneralNavigator.ListSelected(sender);
    }
}
