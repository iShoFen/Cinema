using System.Windows;
using System.Windows.Controls;
using Appli.Navigator;
using Modele;

namespace Appli
{
    /// <summary>
    /// Logique d'interaction pour RandUser.xaml
    /// </summary>
    public partial class RandUser : UserControl
    {
        private static Manager Man => (Application.Current as App)?.Man;

        public RandUser()
        {
            InitializeComponent();
            DataContext = Man.CurrentUser;
            Admin.DataContext = Man.ConnectedUser;
            Avis.DataContext = Man.RendreListeAvis(Man.CurrentUser, out var nb);
            NbAvis.DataContext = nb;

            if (Man.CurrentUser?.IsAdmin ?? default)
                Admin.Visibility = Visibility.Hidden;
        }

        private void Avis_OnSelectionChanged(object sender, SelectionChangedEventArgs e) =>
            GeneralNavigator.AvisUser(sender);

        private void Upgrade_OnClick(object sender, RoutedEventArgs e) =>
            Manager.GraderUser(Man.ConnectedUser, Man.CurrentUser);

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            Man.SupprimerUser(Man.ConnectedUser, Man.CurrentUser);
            ((MainWindow) (Application.Current as App)?.MainWindow)!.ContentControl.Content = new Accueil();
        }
    }
}
