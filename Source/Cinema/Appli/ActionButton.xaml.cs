using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Appli.Navigator;
using Modele;
using static Appli.Utils.ConstanteApp;

namespace Appli
{
    /// <summary>
    /// Logique d'interaction pour ActionButton.xaml
    /// </summary>
    public partial class ActionButton : UserControl
    {
        private static Manager Man => (Application.Current as App)?.Man;

        public ActionButton()
        {
            InitializeComponent();
            DataContext = Man.ConnectedUser;
            Envie.DataContext = Man;

            if (Man.CurrentOeuvre is Episode or Trilogie or Univers)
                EnvieButton.Visibility = Visibility.Hidden;
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e) => GeneralNavigator.OeuvresDeleted();

        private void Modify_OnClick(object sender, RoutedEventArgs e) => GeneralNavigator.OeuvresModify();

        private void Envie_OnClick(object sender, RoutedEventArgs e)
        {
            if (Man.ConnectedUser.ListeEnvie.Contains(Man.CurrentOeuvre))
            {
                Manager.RetirerDeLaListeEnvies(Man.ConnectedUser, Man.CurrentOeuvre);
                Envie.Source = new BitmapImage(new Uri(ADD));
            }
            else
            {
                Manager.AjouterALaListeEnvies(Man.ConnectedUser, Man.CurrentOeuvre);
                Envie.Source = new BitmapImage(new Uri(CHECK));
            }
        }
    }
}
