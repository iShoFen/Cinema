using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Appli.Utils;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.VisualBasic;
using Modele;
using static Appli.Utils.ConstanteApp;

namespace Appli
{
    /// <summary>
    /// Logique d'interaction pour Save.xaml
    /// </summary>
    public partial class Save : MetroWindow
    {
        private static Manager Man => (Application.Current as App)?.Man;
        private static BitmapImage Cache => (Application.Current as App)?.Cache;

        public Save()
        {
            InitializeComponent();
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
        private void PasswordClear_OnClick(object sender, RoutedEventArgs e)
        {
            Password.Password = "";
        }

        private void Return_OnClick(object sender, RoutedEventArgs e) => Close();

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            var oldPhoto = Man.ConnectedUser.ImageProfil;
            var newPhoto = Man.ConnectedUserPhoto;

            string oldPhotoBack = null;
            string finalPath = null;

            bool test;

            if (string.IsNullOrWhiteSpace(oldPhoto) && string.IsNullOrWhiteSpace(newPhoto))
            {
                test = Man.ChangerInfos(Man.ConnectedUser, Password.Password, Man.ConnectedUserPseudo, null,
                    Man.ConnectedUserFamille, Man.ConnectedUserPlateformes);
            }
            else if(oldPhoto is not null && newPhoto.Equals(oldPhoto))
            {
                test = Man.ChangerInfos(Man.ConnectedUser, Password.Password, Man.ConnectedUserPseudo, oldPhoto,
                    Man.ConnectedUserFamille, Man.ConnectedUserPlateformes);
            }
            else
            {
                (finalPath, oldPhotoBack) = ImageSaver.Sauvegarder(USERS_PATH, oldPhoto, newPhoto, Man.ConnectedUserPseudo, Cache);

                if (string.IsNullOrEmpty(finalPath))
                {
                    this.ShowModalMessageExternal("Erreur de copie", "L'image que vous avez choisi n'est pas prise en charge par l'application");
                    Close();
                }

                test = Man.ChangerInfos(Man.ConnectedUser, Password.Password, Man.ConnectedUserPseudo, finalPath, 
                    Man.ConnectedUserFamille, Man.ConnectedUserPlateformes);
            }

            if (test)
            {
                if (oldPhotoBack is not null) File.Delete(oldPhotoBack);
                Close();
            }
            else
            {
                if (finalPath is not null) File.Delete(finalPath);
                if (oldPhotoBack is not null) FileSystem.Rename(oldPhotoBack, oldPhoto!);
                this.ShowModalMessageExternal("Erreur de sauvegarde", "Le mot de passe est incorrect");
            }
        }
    }
}
