using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Web.WebView2.Wpf;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Modele;
using Appli.Utils;
using static Appli.Utils.ConstanteApp;
using static Modele.Constante;

namespace Appli
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private static Manager Man => (Application.Current as App)?.Man;
        private static WebView2 Web => (Application.Current as App)?.Web;

        public string Arg1 { get; set; }

        public string Arg2 { get; set; }

        public string Arg3 { get; set; }

        public MainWindow()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Images");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(Path.Combine(path, "OeuvresPhotos"));
                Directory.CreateDirectory(Path.Combine(path, "PersonnesPhotos"));
                Directory.CreateDirectory(Path.Combine(path, "UsersPhotos"));
            }

            InitializeComponent();
            DataContext = Man;
            WebCross.DataContext = Man.CurrentStream;

            var webPath = Path.Combine(Directory.GetCurrentDirectory(),
                "Ressources\\MicrosoftEdgeWebView2RuntimeInstallerX64.exe");
            if (Directory.Exists("C:\\Program Files (x86)\\Microsoft\\EdgeWebView")) return;

            this.ShowMessageAsync("Installation de WebView",
                "WebView2 n'as pas été trouvé sur votre appareil, l’installation va se lancer juste après ce message !");
            Process.Start(webPath);
        }
        private void Accueil_OnClick(object sender, RoutedEventArgs e)
        {
            Theme.Content = null;
            ContentControl.Content = new Accueil();
            WebCross.Visibility = Visibility.Hidden;
            Web?.Dispose();
        }
        private void Themes_OnClick(object sender, RoutedEventArgs e)
        {
            Theme.Content = null;
            ContentControl.Content = new TousThemes();
            WebCross.Visibility = Visibility.Hidden;
            Web?.Dispose();
        }
        private void Oeuvres_OnClick(object sender, RoutedEventArgs e)
        {
            Theme.Content = null;
            ContentControl.Content = new ToutesOeuvres();
            WebCross.Visibility = Visibility.Hidden;
            Web?.Dispose();
        }
        private void Personnes_OnClick(object sender, RoutedEventArgs e)
        {
            Theme.Content = null;
            ContentControl.Content = new ToutesPersonnes();
            WebCross.Visibility = Visibility.Hidden;
            Web?.Dispose();
        }
        private void User_OnClick(object sender, RoutedEventArgs e)
        {
            ResetRadioButton();
            Theme.Content = null;
            ContentControl.Content = new User();
            WebCross.Visibility = Visibility.Hidden;
            Web?.Dispose();
        }

        public void ResetRadioButton()
        {
            Accueil.IsChecked = false;
            Themes.IsChecked = false;
            Films.IsChecked = false;
            Personnes.IsChecked = false;
        }

        private void SingIn_Register_OnClick(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button)?.Name)
            {
                case nameof(Connexion) or nameof(Profil):
                    new Connexion().ShowDialog();
                    break;
                case nameof(Inscription):
                    new Inscription().ShowDialog();
                    break;
            }

            if (Man.ConnectedUser == null) return;
            
           Connexion.Click -= SingIn_Register_OnClick;
           Connexion.Click += User_OnClick;

           Inscription.Click -= SingIn_Register_OnClick;
           Inscription.Click += Disconnection_OnClick;
           Inscription.Content = DECONNEXION;

           Profil.Click -= SingIn_Register_OnClick;
           Profil.Click += User_OnClick;

           ContentControl.Content = new Accueil();
           Accueil.IsChecked = true;
           Theme.Content = Titre;
        }

        private void Disconnection_OnClick(object sender, RoutedEventArgs e)
        {
            Man.ConnectedUser = null;
            Man.CurrentOeuvre = null;
            Man.CurrentPersonne = null;
            Man.CurrentUser = null;
            Theme.Content = Titre;

            Connexion.Click -= User_OnClick;
            Connexion.Click += SingIn_Register_OnClick;

            Inscription.Click -= Disconnection_OnClick;
            Inscription.Click += SingIn_Register_OnClick;
            Inscription.Content = INSCRIPTION;

            Profil.Click -= User_OnClick;
            Profil.Click += SingIn_Register_OnClick;

            ContentControl.Content = new Accueil();
            Accueil.IsChecked = true;
        }


        private void Search_OnTextChanged(object sender, TextChangedEventArgs e)
        { 
            if (Search.Text.Equals(""))
            {
                SearchHeader.Foreground = new SolidColorBrush { Color = Colors.Gray, Opacity = 0.3};
                SearchClear.IsEnabled = false;
            }
            else
            {
                SearchHeader.Foreground = new SolidColorBrush { Color = Colors.Gray, Opacity = 0.0};
                SearchClear.IsEnabled = true;
            }
        }

        private void SearchClear_OnClick(object sender, RoutedEventArgs e) => Search.Text = "";

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e) => SearchTest();

        private void Search_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Return)) SearchTest();
        }

        private void SearchTest()
        {
            var searching = Search.Text;

            if (Selection.Text.Equals(ACTEUR) || Selection.Text.Equals(REALISATEUR))
            {
                var args = UpperLetters.UpperFirstAfterSpace(PERSONNE_PARAM, searching).ToList();
                Arg1 = Selection.Text;
                Arg2 = args.FirstOrDefault();
                Arg3 = args.LastOrDefault();

                ResetRadioButton();
                ContentControl.Content = new ToutesPersonnes();
            }
            else
            {
                var args = UpperLetters.UpperFirstAfterSpace(OEUVRE_PARAM, searching).ToList();
                Arg1 = Selection.Text;
                Arg2 = args.FirstOrDefault();

                ResetRadioButton();
                ContentControl.Content = new ToutesOeuvres();
            }
        }

        private void WebCross_OnClick(object sender, RoutedEventArgs e)
        {
            WebCross.Visibility = Visibility.Hidden;
            Web?.Dispose();
            ContentControl.Content = new Leaf();
        }

        private void MainWindow_OnClosed(object sender, EventArgs e) => Man.Sauvegarder();
    }
}
