using System.Windows;
using System.Windows.Media;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Modele;

namespace Appli
{
    /// <summary>
    /// Logique d'interaction pour PasswordChange.xaml
    /// </summary>
    public partial class PasswordChange : MetroWindow
    {
        private static Manager Man => (Application.Current as App)?.Man;

        public PasswordChange()
        {
            InitializeComponent();
            DataContext = Man.ConnectedUser;
        }

        private void OldPassword_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(OldPassword.Password))
            {
                OldPasswordHeader.Foreground = new SolidColorBrush { Color = Colors.Gray, Opacity = 0.3};
                OldPasswordClear.IsEnabled = false;
            }
            else
            {
                OldPasswordHeader.Foreground = new SolidColorBrush { Color = Colors.Gray, Opacity = 0.0};
                OldPasswordClear.IsEnabled = true;
            }
        }
        private void OldPasswordClear_OnClick(object sender, RoutedEventArgs e) => OldPassword.Password = "";
        
        private void NewPasswordCheck_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (NewPassword.Password.Equals(""))
            {
                NewPasswordHeader.Foreground = new SolidColorBrush { Color = Colors.Gray, Opacity = 0.3};
                NewPasswordClear.IsEnabled = false;
            }
            else
            {
                NewPasswordHeader.Foreground = new SolidColorBrush { Color = Colors.Gray, Opacity = 0.0};
                NewPasswordClear.IsEnabled = true;
            }
        }
        private void NewPasswordCheckClear_OnClick(object sender, RoutedEventArgs e) => NewPassword.Password = "";

        private void ChangePassword_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(OldPassword.Password) || string.IsNullOrWhiteSpace(NewPassword.Password))
            {
                this.ShowModalMessageExternal("Erreur de changement de mot de passe", "Veuillez renseigner l'ancien puis le nouveau mot de passe.");
                return;
            }

            var test = Manager.ChangerPassword(Man.ConnectedUser, OldPassword.Password, NewPassword.Password);

            if (test)
            {
                Close();
                return;
            }

            this.ShowModalMessageExternal("Erreur de changement de mot de passe", "Votre ancien mot de passe ne correspond pas, veuillez ressaisir");
        }
    }
}
