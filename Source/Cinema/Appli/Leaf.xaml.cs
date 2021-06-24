using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Appli.Navigator;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Modele;
using static Modele.Constante;
using static Appli.Utils.ConstanteApp;

namespace Appli
{
    /// <summary>
    /// Logique d'interaction pour Film.xaml
    /// </summary>
    public partial class Leaf : UserControl
    {
        private static Manager Man => (Application.Current as App)?.Man;

        private readonly Dictionary<string, List<Modele.Streaming>> _streams = new();

        public Leaf()
        {
            InitializeComponent();
            ((MainWindow)(Application.Current as App)?.MainWindow)!.Theme.Content = new ThemeOeuvres();
            DataContext = Man.CurrentOeuvre;
            Acteur.DataContext = Man.CurrentOeuvre.Personnes[ACTEUR];
            Realisateur.DataContext = Man.CurrentOeuvre.Personnes[REALISATEUR];
            NAvis.DataContext = Man.ConnectedUser;
            Avis.DataContext = Man.ConnectedUser;

            if (Man.ConnectedUser is null)
            {
                StreamsR.DataContext = (Man.CurrentOeuvre as Modele.Leaf)?.ListeStream;
                return;
            }

            _streams[REGARDER] =
                (Man.CurrentOeuvre as Modele.Leaf)?.ListeStream.Where(s => Man.ConnectedUser.ListePlateformes.Contains(s.Plateforme)).ToList();
            
            _streams[DISPONIBLE] =
                (Man.CurrentOeuvre as Modele.Leaf)?.ListeStream.Except(_streams[REGARDER]).ToList();

            if (_streams[REGARDER].Count == LISTE_MIN)
                StreamsR.DataContext = _streams[DISPONIBLE];
            else
            {
                StreamsR.DataContext = _streams[REGARDER];
                if (_streams[DISPONIBLE].Count == LISTE_MIN) return;

                Disponible.Visibility = Visibility.Visible;
                StreamsD.Visibility = Visibility.Visible;
                StreamsD.DataContext = _streams[DISPONIBLE];
            }
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

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is not Button but) return;

            var user = (but.DataContext as KeyValuePair<Modele.User, Modele.Avis>? ?? default).Key;
            Manager.SupprimerAvis(Man.ConnectedUser, user, Man.CurrentOeuvre);
        }

        private void Stream_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button) return;
            if (Man.ConnectedUser is null)
            {
                (Window.GetWindow(this) as MetroWindow).ShowModalMessageExternal("Problème de connexion",
                    "Vous devez être connecté pour regarder en streaming !");
                return;
            }

            Man.CurrentStream = (button.DataContext as Modele.Streaming)?.Lien;
            ((MainWindow) (Application.Current as App)?.MainWindow)!.WebCross.Visibility = Visibility.Visible;
            GeneralNavigator.Streaming();
        }
    }
}
