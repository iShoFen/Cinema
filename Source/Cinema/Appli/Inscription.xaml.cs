using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Modele;

namespace Appli
{
    /// <summary>
    /// Logique d'interaction pour Inscription.xaml
    /// </summary>
    public partial class Inscription : MetroWindow
    {
        private static Manager Man => (Application.Current as App)?.Man;
        private const int LONGUEUR = 8;

        public Inscription()
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
            if (string.IsNullOrEmpty(Password.Password))
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
        private void PasswordClear_OnClick(object sender, RoutedEventArgs e) => Password.Password = "";
        
        private void PasswordCheck_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (PasswordCheck.Password.Equals(""))
            {
                PasswordCheckHeader.Foreground = new SolidColorBrush { Color = Colors.Gray, Opacity = 0.3};
                PasswordCheckClear.IsEnabled = false;
            }
            else
            {
                PasswordCheckHeader.Foreground = new SolidColorBrush { Color = Colors.Gray, Opacity = 0.0};
                PasswordCheckClear.IsEnabled = true;
            }
        }
        private void PasswordCheckClear_OnClick(object sender, RoutedEventArgs e) => PasswordCheck.Password = "";


        private void Return_OnClick(object sender, RoutedEventArgs e) => Close();

        private void TryInscription_OnClick(object sender, RoutedEventArgs e)
        {
            var pseudo = Pseudo.Text;
            var password = Password.Password;
            var passwordCheck = PasswordCheck.Password;
            var upper = false;
            var digit = false;
            var symbol = false;

            if (string.IsNullOrWhiteSpace(pseudo))
            {
                this.ShowModalMessageExternal("Pseudo incorrect", "Le pseudo ne peut être vide ou ne contenir que des espaces !");
                return;
            }

            foreach (var ch in password.ToCharArray())
            {
                if (char.IsUpper(ch))
                {
                    upper = true;
                    continue;
                }

                if (char.IsDigit(ch))
                {
                    digit = true;
                    continue;
                }

                if (char.IsSymbol(ch)) symbol = true;
            }

            if (!upper || !digit || !symbol || password.Length < LONGUEUR)
            {
                this.ShowModalMessageExternal("Mot de passe incorrect",
                    $"Le mot de passe doit contenir au moins {LONGUEUR} caractères, d'on au moins une majuscule, un chiffre et un symbole !");
                return;
            }

            if (!password.Equals(passwordCheck))
            {
                this.ShowModalMessageExternal("Mot de passe incorrect", "Les 2 mots de passe ne corespondent pas, veuillez les ressaisir !");
                return;
            }

            var test  = Man.CreerUser(Pseudo.Text, Password.Password, out _);
            if (test)
            {
                Close();
                return;
            }

            this.ShowModalMessageExternal("Pseudo non disponible", "Le pseudo que vous avez choisi n'est pas disponible veuillez en choisir un autre !");
        }
    }
}
