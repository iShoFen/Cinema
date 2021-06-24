using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Appli.Navigator;
using Modele;
using static Modele.Constante;

namespace Appli
{
    public partial class Composite : UserControl
    {
        private static Manager Man => (Application.Current as App)?.Man;

        public Composite()
        {
            InitializeComponent();
            ((MainWindow)(Application.Current as App)?.MainWindow)!.Theme.Content = new ThemeOeuvres();
            DataContext = Man.CurrentOeuvre;
            Oeuvres.DataContext = Man;
            NAvis.DataContext = Man.ConnectedUser;
            Avis.DataContext = Man.ConnectedUser;
            Realisateur.DataContext = Man.CurrentOeuvre.Personnes[REALISATEUR];
            Acteur.DataContext = Man.CurrentOeuvre.Personnes[ACTEUR];
        }

        private void Acteur_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is not Button {DataContext: KeyValuePair<Personne, string>} button) return;

            Man.CurrentPersonne = ((KeyValuePair<Personne, string>) button.DataContext).Key;
            GeneralNavigator.Acteur();
        }

        private void NewAvis_OnClick(object sender, RoutedEventArgs e)
        {
            var avis = new NewAvis();
            avis.ShowDialog();
        }

        private void Avis_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Man.CurrentUser = (Avis.SelectedItem as KeyValuePair<Modele.User, Modele.Avis>? ?? default).Key;
            ((MainWindow) (Application.Current as App)?.MainWindow)!.ContentControl.Content = new RandUser();
        }

        private void AvisDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is not Button but) return;

            var user = (but.DataContext as KeyValuePair<Modele.User, Modele.Avis>? ?? default).Key;
            Manager.SupprimerAvis(Man.ConnectedUser, user, Man.CurrentOeuvre);
        }
 

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e) =>
            GeneralNavigator.ListSelected(sender);

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            if(sender is not Button but) return;

            if (but.DataContext is Episode episode)
            {
                Man.SupprimerOeuvre(Man.ConnectedUser, episode);
            }
            else
            {
                Man.RetirerOeuvreComp(Man.ConnectedUser, Man.CurrentOeuvre as Modele.Composite,
                    but.DataContext as Oeuvre);
            }

            if (Man.CurrentOeuvre is not null) return;

            ((MainWindow) (Application.Current as App)?.MainWindow)!.Theme.Content = null;
            ((MainWindow) (Application.Current as App)?.MainWindow)!.ContentControl.Content = new Accueil();
        }
    }
}
