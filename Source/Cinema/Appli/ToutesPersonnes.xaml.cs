using System.Windows;
using System.Windows.Controls;
using Appli.Navigator;
using Modele;

namespace Appli
{
    /// <summary>
    /// Logique d'interaction pour ToutesPersonnes.xaml
    /// </summary>
    public partial class ToutesPersonnes : UserControl
    {
        private static Manager Man => (Application.Current as App)?.Man;
        private static MainWindow MainWindow => ((Application.Current as App)?.MainWindow) as MainWindow;


        public ToutesPersonnes()
        {
            InitializeComponent();

            var arg1 = MainWindow.Arg1;
            var arg2 = MainWindow.Arg2;
            var arg3 = MainWindow.Arg3;

            if (arg1 is null)
                DataContext = Man.RendreListeActeurs();

            else if (arg3 is null)
            {
                DataContext = Man.RendreListeActeursSearch(arg1, arg2);
                MainWindow.Arg1 = null;
                MainWindow.Arg2 = null;
            }
            else
            {
                DataContext = Man.RendreListeActeursSearch(arg1, arg2, arg3);
                MainWindow.Arg1 = null;
                MainWindow.Arg2 = null;
                MainWindow.Arg3 = null;
            }
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e) =>
            GeneralNavigator.ListSelected(sender);
    }
}
