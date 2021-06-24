using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Modele;

namespace Appli
{
    /// <summary>
    /// Logique d'interaction pour Connexion.xaml
    /// </summary>
    public partial class Connexion : MetroWindow
    {
        private static Manager Man => (Application.Current as App)?.Man;

        public Connexion()
        {
            InitializeComponent();
        }

        private void Pseudo_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(Pseudo.Text))
            {
                PseudoHeader.Foreground = new SolidColorBrush { Color = Colors.Gray, Opacity = 0.3};
                PseudoClear.IsEnabled = false;
            }
            else
            {
                PseudoHeader.Foreground = new SolidColorBrush { Color = Colors.Gray, Opacity = 0.0};
                PseudoClear.IsEnabled = true;
            }
        }

        private void PseudoClear_OnClick(object sender, RoutedEventArgs e)
        {
            Pseudo.Text = "";
        }

        private void Password_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (Password.Password.Equals(""))
            {
                PasswordHeader.Foreground = new SolidColorBrush { Color = Colors.Gray, Opacity = 0.3};
                PasswordClear.IsEnabled = false;
            }
            else
            {
                PasswordHeader.Foreground = new SolidColorBrush { Color = Colors.Gray, Opacity = 0.0};
                PasswordClear.IsEnabled = true;
            }
        }
        private void PasswordClear_OnClick(object sender, RoutedEventArgs e)
        {
            Password.Password = "";
        }

        private void Return_OnClick(object sender, RoutedEventArgs e) => Close();

        private void TryConnexion_OnClick(object sender, RoutedEventArgs e)
        {
            var test  = Man.Connexion(Pseudo.Text, Password.Password, out _);
            if (test)
            {
                Close();
                return;
            }

            this.ShowModalMessageExternal("Erreur de connexion", "L'utilisateur n'existe pas ou votre mot de passe est incorrect veuillez réessayer !");
        }
    }
}
