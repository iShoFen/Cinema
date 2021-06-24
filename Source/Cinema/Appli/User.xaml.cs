using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Appli.Navigator;
using Appli.Utils;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Modele;
namespace Appli
{
    /// <summary>
    /// Logique d'interaction pour User.xaml
    /// </summary>
    public partial class User : UserControl
    {
        private static Manager Man => (Application.Current as App)?.Man;
        private BitmapImage _image;

        public User()
        {
            InitializeComponent();
            DataContext = Man.ConnectedUser;
            Info.DataContext = Man;
            Plateforme.DataContext = Manager.RendreToutesPlateformes();
            Avis.DataContext = Man.RendreListeAvis(Man.ConnectedUser, out var nb);
            AvisL.DataContext = nb;

            Man.ConnectedUserPseudo = Man.ConnectedUser.Pseudo;
            Man.ConnectedUserPhoto = Man.ConnectedUser.ImageProfil;
            Man.ConnectedUserFamille = Man.ConnectedUser.IsModeFamille;

            Man.ConnectedUserPlateformes.Clear();
            Man.ConnectedUserPlateformes.AddRange(Man.ConnectedUser.ListePlateformes);
        }

        private void Uploader_OnClick(object sender, RoutedEventArgs e)
        {
            var (path, error) = ImageSaver.Uploader(ref _image);

            if (error == false)
            {
                (Window.GetWindow(this) as MetroWindow).ShowModalMessageExternal("Erreur d'Image",
                    "Votre image proviens d'une source inconnue et ne peut pas être ouverte ! Veuillez la débloquer ou changer d'image.");
                return;
            }

            Man.ConnectedUserPhoto = path;
            Profil.ImageSource = _image;
        }

        private void Pseudo_OnClick(object sender, RoutedEventArgs e) => new PseudoChance().ShowDialog();

        private void Password_OnClick(object sender, RoutedEventArgs e) => new PasswordChange().ShowDialog();

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if(sender is not CheckBox check) return;

            if (check.IsChecked ?? default)
                Man.ConnectedUserPlateformes.Add(Enum.Parse<Plateformes>((string) check.Content));
            else
                Man.ConnectedUserPlateformes.Remove(Enum.Parse<Plateformes>((string) check.Content));
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            (Application.Current as App)!.Cache = _image; 
            new Save().ShowDialog();
            Man.ConnectedUserPhoto = Man.ConnectedUser.ImageProfil;
        }

        private void List_OnSelectionChanged(object sender, SelectionChangedEventArgs e) =>
            GeneralNavigator.ListSelected(sender);

        private void Avis_OnSelectionChanged(object sender, SelectionChangedEventArgs e) =>
            GeneralNavigator.AvisUser(sender);

    }
}
