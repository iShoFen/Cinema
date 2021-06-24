using System.Windows;
using System.Windows.Controls;
using Appli.Navigator;
using Modele;

namespace Appli
{
    /// <summary>
    /// Logique d'interaction pour ThemeOeuvres.xaml
    /// </summary>
    public partial class ThemeOeuvres : UserControl
    {
        private static Manager Man => (Application.Current as App)?.Man;

        public ThemeOeuvres()
        {
            InitializeComponent();
            DataContext = Man.RendreListeOeuvresTheme(Man.CurrentOeuvre.Theme, Man.CurrentUser);
        }

        private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) =>
            GeneralNavigator.ListSelected(sender);
    }
}
