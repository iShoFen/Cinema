using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Appli.Navigator;
using Appli.Utils;
using Microsoft.VisualBasic;
using Modele;
using static Modele.Constante;
using static Appli.Utils.ConstanteApp;

namespace Appli
{
    /// <summary>
    /// Logique d'interaction pour CreerComposite.xaml
    /// </summary>
    public partial class CreerComposite : UserControl
    {
        private static Manager Man => (Application.Current as App)?.Man;

        private MetroWindow MWindow => Window.GetWindow(this) as MetroWindow;

        private string _fileName;

        private BitmapImage _image;

        private readonly ObservableCollection<Oeuvre> _oeuvres = new();

        private readonly ReadOnlyObservableCollection<Oeuvre> _reOeuvres;

        private Dictionary<Personne, string> _acts = new();

        private Dictionary<Personne, string> _reals = new();

        private List<Oeuvre> _searchOeuvres = new();

        public CreerComposite()
        {
            InitializeComponent();
            CreateModif.Content = Man.CurrentOeuvre is null ? "Créer" : "Modifier";
            DataContext = Man.CurrentOeuvre;

            Theme.DataContext = Manager.RendreTousThemes();
            Select.SelectedIndex = OeuvreNavigator.TypeIndex;
            Real.DataContext = Man.CurrentOeuvre?.Personnes[REALISATEUR];
            Acteur.DataContext = Man.CurrentOeuvre?.Personnes[ACTEUR];

            _reOeuvres = (Man.CurrentOeuvre as Modele.Composite)?.ReOeuvres;
            if (_reOeuvres is not null)
                ReOeuvres.DataContext = _reOeuvres;
            else
                ReOeuvres.DataContext = _oeuvres;
        }

        private void DisplayInfo_OnLoaded(object sender, RoutedEventArgs e)
        {
            MWindow.ShowModalMessageExternal("Information de création",
                "Pour pouvoir correctement créer un Composite veuillez rentrer " +
                "les Réalisateurs sous la forme suivante => (Prénom Nom), " +
                "et les Acteurs sous cette forme => (Prénom Nom : Rôle). " +
                "Vous devrez au minimum remplir tous les champs sauf " +
                "Acteurs et Réalisateurs si vous ne le souhaitez pas. " +
                "Vous devrez aussi avoir au moins une oeuvre de sélectionner dans la liste " +
                "sauf pour ce qui est des Séries ou vous devrez y revenir plus tard. " +
                "Merci de votre compréhension !");
        }

        private void Sub_TypeSwitch(object sender, RoutedEventArgs e) => OeuvreNavigator.TypeSwitch(sender, this);

        private void Uploader_OnClick(object sender, RoutedEventArgs e)
        {
             var (path, error) = ImageSaver.Uploader(ref _image);

             if (error == false)
             {
                 MWindow.ShowModalMessageExternal("Erreur d'Image",
                     "Votre image proviens d'une source inconnue et ne peut pas être ouverte ! Veuillez la débloquer ou changer d'image.");
                 return;
             }

             _fileName = path;
             Image.ImageSource = _image;
        }

        private void Search_OnTextChanged(object sender, TextChangedEventArgs e)
        { 
            if (string.IsNullOrEmpty(Search.Text))
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

        private void SearchClear_OnClick(object sender, RoutedEventArgs e)
        {
            Search.Text = "";
            if (Man.CurrentOeuvre is not null)
                ReOeuvres.DataContext = _reOeuvres;
            else
                ReOeuvres.DataContext = _oeuvres;
        }

        private void Search_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (!e.Key.Equals(Key.Return)) return;


            var result = UpperLetters.UpperFirstAfterSpace(OEUVRE_PARAM, Search.Text);
            if (string.IsNullOrEmpty(result.First()))return;

            var search = result .First();

            if (Man.CurrentOeuvre is null) 
                _searchOeuvres = Select.Text switch
                {
                    TRILOGIE => Man.RendreListeOeuvresSearch(FILM, search).Except(_oeuvres).ToList(),
                    SERIE => Man.RendreListeOeuvresSearch(EPISODE, search).Except(_oeuvres).ToList(),
                    UNIVERS => Man.RendreListeOeuvresSearch("" ,search).Except(_oeuvres).ToList(),
                    _ => _searchOeuvres
                };

            else
            {
                _searchOeuvres = Man.CurrentOeuvre switch
                {
                    Trilogie => Man.RendreListeOeuvresSearch(FILM, search).Except(_reOeuvres).ToList(),
                    Serie => Man.RendreListeOeuvresSearch(EPISODE, search).Except(_reOeuvres).ToList(),
                    Univers => Man.RendreListeOeuvresSearch("", search).Except(_reOeuvres).ToList(),
                    _ => _searchOeuvres
                };
            }

            ReOeuvres.DataContext = _searchOeuvres;
        }

        private void Oeuvre_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is not Button but) return;
            if (Man.CurrentOeuvre is null)
            {
                if (ReOeuvres.DataContext == _oeuvres)
                    _oeuvres.Remove(but.DataContext as Oeuvre);
                else if (ReOeuvres.DataContext == _searchOeuvres)
                {
                    _oeuvres.Add(but.DataContext as Oeuvre);
                    ReOeuvres.DataContext = _oeuvres;
                }
            }
            else
            {
                if (ReOeuvres.DataContext == _reOeuvres)
                {
                    Man.RetirerOeuvreComp(Man.ConnectedUser, Man.CurrentOeuvre as Modele.Composite, but.DataContext as Oeuvre);
                    if (Man.CurrentOeuvre is null)
                        ((MainWindow) (Application.Current as App)?.MainWindow)!.ContentControl.Content = new Accueil();
                }
                else if (ReOeuvres.DataContext == _searchOeuvres)
                {
                    Manager.AjouterOeuvreComp(Man.ConnectedUser, Man.CurrentOeuvre as Modele.Composite, but.DataContext as Oeuvre);
                    ReOeuvres.DataContext = _reOeuvres;
                }
            }
        }

        private void CreerComposite_OnClick(object sender, RoutedEventArgs e)
        {
            var acteurs = Acteur.Text;
            var realisateur = Real.Text;

            var testAct = CreerPersonne.ActeurUtil(MWindow, acteurs, out var actsTmp);
            var testReal = CreerPersonne.RealUtil(MWindow, realisateur, out var realsTmp);

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
            var boxitem = Select.Text;
            if (!Man.HasOeuvre(boxitem, titre, dds))
            {
                MWindow.ShowModalMessageExternal("Erreur de création", "Une oeuvre avec la même date de sortie et le même tire existe déjà veuillez changer l'un des 2 !");
                return;
            }

            
            if (Man.CurrentOeuvre is not null && !Man.CurrentOeuvre.Titre.Equals(titre) && !Man.CurrentOeuvre.DateDeSortie.Equals(dds) && Man.HasOeuvre(boxitem, titre, dds))
            {
                MWindow.ShowModalMessageExternal("Erreur de création", "Une oeuvre avec la même date de sortie et le même tire existe déjà veuillez changer l'un des 2 !");
                return;
            }


            var theme = Theme.Text;
            var synopsis = Synopsis.Text;

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
                Man.ModifierComposite(Man.ConnectedUser, Man.CurrentOeuvre as Modele.Composite, titre, dds,
                    Man.CurrentOeuvre.LienImage, synopsis, th, ff, pers);

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
                    Man.ModifierComposite(Man.ConnectedUser, Man.CurrentOeuvre as Modele.Composite, titre, dds,
                        path, synopsis, th, ff, pers);
                    
                    _ = Man.CurrentOeuvre.LienImage.Equals(path) ? oe = Man.CurrentOeuvre : oe = null;
                }
                else
                    oe = Man.CreerComposite(Man.ConnectedUser, boxitem, titre, dds, path, synopsis, th,
                        ff, _oeuvres, pers);
            }

            if (oe is null)
            {
                if (path is not null) File.Delete(path);
                if (back is not null) FileSystem.Rename(back, Man.CurrentOeuvre.LienImage!);
                MWindow.ShowModalMessageExternal("Erreur de création", "Erreur lors de la création du Composite.");
                return;
            }

            if (back is not null) File.Delete(back);
                OeuvreNavigator.OeuvreOnCreated(Man.CurrentOeuvre);
        }
    }
}
