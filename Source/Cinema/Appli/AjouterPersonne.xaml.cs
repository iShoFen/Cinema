using System;
using System.Globalization;
using System.IO;
using System.Windows;
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
    /// Logique d'interaction pour AjouterPersonne.xaml
    /// </summary>
    public partial class AjouterPersonne : MetroWindow
    {
        private static Manager Man => (Application.Current as App)?.Man;

        private string _fileName;

        private BitmapImage _image;

        public AjouterPersonne()
        {
            InitializeComponent();
            DataContext = Man.CurrentPersonne;

            Save.Content = Man.CurrentPersonne is null ? "Créer" : "Modifier";
        }

        private void Uploader_OnClick(object sender, RoutedEventArgs e)
        {
            var (path, error) = ImageSaver.Uploader(ref _image);

            if (error == false)
            {
                (GetWindow(this) as MetroWindow).ShowModalMessageExternal("Erreur d'Image", "Votre image proviens d'une source inconnue et ne peut pas être ouverte ! Veuillez la débloquer ou changer d'image.");
                return;
            }
            _fileName = path;
            ImgPersonne.ImageSource = new BitmapImage(new Uri(_fileName, UriKind.Absolute));
        }

        private void TryPersonne_OnClick(object sender, RoutedEventArgs e)
        {
            if (Man.CurrentPersonne is null && string.IsNullOrWhiteSpace(_fileName))
            {
                this.ShowModalMessageExternal("Erreur de création", "L'image n'a pas été trouvée veuillez réessayer !");
                return;
            }

            DateTime.TryParse(Ddn.Text, new CultureInfo("fr-fr"), DateTimeStyles.AdjustToUniversal, out var ddn);
            if (ddn > DateTime.Now)
            {
                this.ShowModalMessageExternal("Erreur de création", "Date incorrecte !");
                return;
            }

            var nom = Nom.Text.Replace(Nom.Text[0], char.ToUpper(Nom.Text[0]));
            var prenom = Prenom.Text.Replace(Prenom.Text[0], char.ToUpper(Prenom.Text[0]));;
            var natio = Nationalite.Text;
            var bio = Bio.Text;

            if (string.IsNullOrWhiteSpace(nom) || string.IsNullOrWhiteSpace(prenom) || string.IsNullOrWhiteSpace(natio) || string.IsNullOrWhiteSpace(bio))
            {
                this.ShowModalMessageExternal("Erreur de création", "Tous les champs doivent être remplis !");
                return;
            }
            
            string path = null;
            string back = null;
            Personne pe = null;

            if (Man.CurrentPersonne is not null && _fileName is null)
            {
                if(!Man.ModifPersonne(Man.ConnectedUser, Man.CurrentPersonne,nom, prenom, bio, natio, Man.CurrentPersonne.LienImage, ddn))
                    this.ShowModalMessageExternal("Erreur de modification", "Vous n'êtes pas administrateur ou ce nom et prénom sont déjà utilisé par une autres personnes !");

                pe = Man.CurrentPersonne;
                Man.CurrentPersonne = Man.CurrentPersonne;
            }

            else
            {
                (path, back) = ImageSaver.Sauvegarder(PERSONNES_PATH, Man.CurrentPersonne?.LienImage, _fileName,
                    $"{prenom}_{nom}", _image);

                if (string.IsNullOrWhiteSpace(path))
                {
                    this.ShowModalMessageExternal("Erreur de création", "L'image n'existe pas !");
                    return;
                }

                if (Man.CurrentPersonne is null)
                    pe = Man.CreerPersonne(Man.ConnectedUser, prenom, nom, bio, natio, path, ddn);

                else
                {
                    if(!Man.ModifPersonne(Man.ConnectedUser, Man.CurrentPersonne, nom, prenom, bio, natio, path, ddn))
                        this.ShowModalMessageExternal("Erreur de modification", "Vous n'êtes pas administrateur ou ce nom et prénom sont déjà utilisé par une autres personnes !");

                    pe = Man.CurrentPersonne;
                    Man.CurrentPersonne = Man.CurrentPersonne;
                }
            }

            if (pe is null)
            { 
                if (path is not null) File.Delete(path);
                if (back is not null) FileSystem.Rename(back, Man.CurrentPersonne.LienImage);
                this.ShowModalMessageExternal("Erreur de création", "Veuillez bien vérifier vos données !");
                return;
            }

            if (back is not null) File.Delete(back);
            Close();
        }
    }
}
