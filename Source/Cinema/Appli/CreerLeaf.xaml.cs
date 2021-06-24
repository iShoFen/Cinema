using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.VisualBasic;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Appli.Navigator;
using Appli.Utils;
using Modele;
using static Modele.Constante;
using static Appli.Utils.ConstanteApp;

namespace Appli
{
    /// <summary>
    /// Logique d'interaction pour CreerLeaf.xaml
    /// </summary>
    public partial class CreerLeaf : UserControl
    {
        private static Manager Man => (Application.Current as App)?.Man;

        private MetroWindow MWindow => Window.GetWindow(this) as MetroWindow;

        private string _fileName;

        private readonly Dictionary<Plateformes, string> _streams = new();

        private Dictionary<Personne, string> _acts = new();

        private Dictionary<Personne, string> _reals = new();

        private BitmapImage _image;

        public CreerLeaf()
        {
            InitializeComponent();
            DataContext = Man.CurrentOeuvre;
            CreateModif.Content = Man.CurrentOeuvre is null ? "Créer" : "Modifier";
            Theme.DataContext = Manager.RendreTousThemes();
            Streams.DataContext = Manager.RendreToutesPlateformes();
            Select.SelectedIndex = OeuvreNavigator.TypeIndex;

            Real.DataContext = Man.CurrentOeuvre?.Personnes[REALISATEUR];
            Acteur.DataContext = Man.CurrentOeuvre?.Personnes[ACTEUR];
        }

        private void DisplayInfo_OnLoaded(object sender, RoutedEventArgs e)
        {
            MWindow.ShowModalMessageExternal("Information de création",
                "Pour pouvoir correctement créer une Leaf veuillez rentrer " +
                "les Réalisateurs sous la forme suivante => (Prénom Nom), " +
                "et les Acteurs sous cette forme => (Prénom Nom : Rôle). " +
                "Vous devrez au minimum remplir tous les champs sauf " +
                "Acteurs et Réalisateurs et les liens de streaming si vous ne le souhaitez pas. " +
                "Nous vous conseillons cependant de quand même ajouter les liens  de streaming " +
                "pour garder une certaines cohérence. " +
                "Merci de votre compréhension !");
        }

        private void Sub_TypeSwitch(object sender, RoutedEventArgs e) => OeuvreNavigator.TypeSwitch(sender, this);

        private void Uploader_OnClick(object sender, RoutedEventArgs e)
        {
            var (path, error) = ImageSaver.Uploader(ref _image);

            if (error == false)
            {
                (Window.GetWindow(this) as MetroWindow).ShowModalMessageExternal("Erreur d'Image", "Votre image proviens d'une source inconnue et ne peut pas être ouverte ! Veuillez la débloquer ou changer d'image.");
                return;
            }

            _fileName = path;
            Image.ImageSource = _image;
        }

        private void Stream_Loaded_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is not TextBox text) return;

            if (string.IsNullOrEmpty(text.Text))
                _streams.Remove(Enum.Parse<Plateformes>((string) text.DataContext));

            else
                _streams[Enum.Parse<Plateformes>((string) text.DataContext)] = text.Text;
        }

        private void CreerLeaf_OnClick(object sender, RoutedEventArgs e)
        {
            var acteurs = Acteur.Text;
            var realisateur = Real.Text;

            var testReal = CreerPersonne.RealUtil(MWindow, realisateur, out var realsTmp);
            var testAct = CreerPersonne.ActeurUtil(MWindow, acteurs, out var actsTmp);

            if (_acts.Count != LISTE_MIN || _reals.Count != LISTE_MIN)
            {
                var actsDelete0 = _acts.Except(actsTmp).Select(act => act.Key)
                    .Where(act => act.Oeuvres.Count == LISTE_MIN);

                var realsDelete0 = _reals.Except(actsTmp).Select(real => real.Key)
                    .Where(real => real.Oeuvres.Count == LISTE_MIN);

                foreach (var act in actsDelete0)
                    Man.SupprimerPersonne(Man.ConnectedUser, act);

                foreach (var real in realsDelete0)
                    Man.SupprimerPersonne(Man.ConnectedUser, real);

                _acts.Clear();
                _reals.Clear();
            }

            _reals = realsTmp;
            _acts = actsTmp;

            if (!testReal || !testAct)
            {
                MWindow.ShowModalMessageExternal("Erreur de création", "Un des acteurs ou réalisateurs n'a pas pu être créé !");
                return;
            }



            if (Man.CurrentOeuvre is null && string.IsNullOrWhiteSpace(_fileName))
            {
                MWindow.ShowModalMessageExternal("Erreur de création", "L'image n'a pas été trouvée veuillez réessayer !");
                return;
            }

            DateTime.TryParse(Dds.Text, new CultureInfo("fr-fr"), DateTimeStyles.AdjustToUniversal, out var dds);
            if (dds > DateTime.Now)
            {
                MWindow.ShowModalMessageExternal("Erreur de création", "Date incorrecte !");
                return;
            }

            var titre = Titre.Text;
            if (Theme.Text.Equals(EPISODE) && Man.RendreListeOeuvresSearch(SERIE, titre.Split(":")[0].Trim()).ToList().Count == 0)
            {
                MWindow.ShowModalMessageExternal("Erreur de création", "Aucune série n'existe en lien avec cette épisode");
                return;
            }

            var boxitem = Select.Text;
            if (Man.CurrentOeuvre is null && !Man.HasOeuvre(boxitem, titre, dds))
            {
                MWindow.ShowModalMessageExternal("Erreur de création", "Une oeuvre avec la même date de sortie et le même tire existe déjà veuillez changer l'un des 2 !");
                return;
            } 
            
            if (Man.CurrentOeuvre is not null && !Man.CurrentOeuvre.Titre.Equals(titre) && !Man.CurrentOeuvre.DateDeSortie.Equals(dds) && Man.HasOeuvre(boxitem, titre, dds))
            {
                MWindow.ShowModalMessageExternal("Erreur de création", "Une oeuvre avec la même date de sortie et le même tire existe déjà veuillez changer l'un des 2 !");
                return;
            }

            var synopsis = Synopsis.Text;
            var theme = Theme.Text;

            if (string.IsNullOrWhiteSpace(titre) || string.IsNullOrWhiteSpace(realisateur) ||
                string.IsNullOrWhiteSpace(acteurs) || string.IsNullOrWhiteSpace(synopsis) ||
                string.IsNullOrWhiteSpace(theme))
            {
                MWindow.ShowModalMessageExternal("Erreur de création", "Tous les champs doivent être remplis !");
                return;
            }
            
            var th = Enum.Parse<Themes>(theme);
            var ff = Famille.IsChecked ?? default;

            var personnes = new Dictionary<string, Dictionary<Personne, string>> {[ACTEUR] = new(), [REALISATEUR] = new()};

            foreach (var (key, value) in _acts)
                personnes[ACTEUR][key] = value;

            foreach (var (key, value) in _reals)
                personnes[REALISATEUR][key] = value;

            var pers = personnes.ToDictionary(pair => pair.Key, pair => pair.Value.ToList()
                as IEnumerable<KeyValuePair<Personne, string>>);

            string path = null;
            string back = null;
            Oeuvre oe;

            if (Man.CurrentOeuvre is not null && _fileName is null)
            {
                Man.ModifierLeaf(Man.ConnectedUser, Man.CurrentOeuvre as Modele.Leaf, titre, dds,
                    Man.CurrentOeuvre.LienImage, synopsis, th, ff, pers, Man.AjouterStreamLeaf(titre, _streams));

                oe = Man.CurrentOeuvre;
            }
            else
            {
                (path, back) = ImageSaver.Sauvegarder(OEUVRES_PATH, Man.CurrentOeuvre?.LienImage, _fileName, titre,
                    _image);

                if (string.IsNullOrEmpty(path))
                {
                    MWindow.ShowModalMessageExternal("Erreur de création", "Votre image n'existe pas !");
                    return;
                }

                if (Man.CurrentOeuvre is not null)
                {
                    Man.ModifierLeaf(Man.ConnectedUser, Man.CurrentOeuvre as Modele.Leaf, titre, dds,
                        path, synopsis, th, ff, pers, Man.AjouterStreamLeaf(titre, _streams));

                    _ = Man.CurrentOeuvre.LienImage.Equals(path) ? oe = Man.CurrentOeuvre : oe = null;
                }
                else
                    oe = Man.CreerLeaf(Man.ConnectedUser, boxitem, titre, dds, path, synopsis, th,
                        ff, pers, Man.AjouterStreamLeaf(titre, _streams));
            }

            if (oe is null)
            {
                if (path is not null) File.Delete(path);
                if (back is not null) FileSystem.Rename(back, Man.CurrentOeuvre.LienImage!);
                MWindow.ShowModalMessageExternal("Erreur de création", "Erreur lors de la création ou de la modifications de la Leaf.");
                return;
            }

            if (back is not null) File.Delete(back);
                OeuvreNavigator.OeuvreOnCreated(Man.CurrentOeuvre);
        }
    }
}
