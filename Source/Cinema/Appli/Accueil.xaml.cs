using System.Windows;
using System.Windows.Controls;
using Appli.Navigator;
using Modele;

namespace Appli
{
    /// <summary>
    /// Logique d'interaction pour Accueil.xaml
    /// </summary>
    public partial class Accueil : UserControl
    {
        private static Manager Man => (Application.Current as App)?.Man;

        public Accueil()
        {
            InitializeComponent();
            Populaires.DataContext = Man.RendreListePopulaire(Man.ConnectedUser);
            Consultes.DataContext = Man;
            Add.DataContext = Man.ConnectedUser;
        }

        private void List_OnSelected(object sender, RoutedEventArgs e) => GeneralNavigator.ListSelected(sender);

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            Man.CurrentOeuvre = null;
            GeneralNavigator.NewOeuvre();
        }
    }
}
