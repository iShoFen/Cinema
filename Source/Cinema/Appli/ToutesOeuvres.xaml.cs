using System;
using System.Windows;
using System.Windows.Controls;
using Appli.Navigator;
using Modele;

namespace Appli
{
    /// <summary>
    /// Logique d'interaction pour TousFilms.xaml
    /// </summary>
    public partial class ToutesOeuvres : UserControl
    {
        private static Manager Man => (Application.Current as App)?.Man;


        private static MainWindow Window => ((Application.Current as App)?.MainWindow) as MainWindow;

        public ToutesOeuvres()
        {
            InitializeComponent();


            var arg1 = Window.Arg1;
            var arg2 = Window.Arg2;

            if (!string.IsNullOrEmpty(Window.Titre.Text) && Window.Theme.Content is not null)
                DataContext = Man.RendreListeOeuvresTheme(Enum.Parse<Themes>(Window.Titre.Text), Man.ConnectedUser);

            else if (arg1 is null)
                DataContext = Man.RendreListeOeuvres(Man.ConnectedUser);

            else
            {
                DataContext = Man.RendreListeOeuvresSearch(arg1, arg2, Man.ConnectedUser);

                Window.Arg1 = null;
            }

            Add.DataContext = Man.ConnectedUser;
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e) => GeneralNavigator.ListSelected(sender);


        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            Man.CurrentOeuvre = null;
            GeneralNavigator.NewOeuvre();
        }
    }
}
