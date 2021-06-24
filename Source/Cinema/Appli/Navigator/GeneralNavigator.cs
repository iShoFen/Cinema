using System.Collections.Generic;
using System.Windows;
using Modele;
using System.Windows.Controls;

namespace Appli.Navigator
{
    public static class GeneralNavigator
    {
        private static Manager Man => (Application.Current as App)?.Man;

        private static MainWindow Window => (Application.Current as App)?.MainWindow as MainWindow;

        public static void ListSelected(object sender)
        {

            var item = sender switch
            {
                ListBox list => list.SelectedItem,
                TreeView tree => tree.SelectedItem,
                _ => null
            };

            if (item is null) return;

            Window.ResetRadioButton();
            switch (item)
            {
                case Modele.Leaf leaf:
                    Man.CurrentOeuvre = leaf;
                    Manager.AjouterAuRecemmentConsulte(Man.ConnectedUser, Man.CurrentOeuvre);
                    Window.ContentControl.Content = new Leaf();
                    break;

                case Modele.Composite comp:
                    Man.CurrentOeuvre = comp;
                    Manager.AjouterAuRecemmentConsulte(Man.ConnectedUser, Man.CurrentOeuvre);
                    Window.ContentControl.Content = new Composite();
                    break;

                case Personne pers:
                    Man.CurrentPersonne = pers;
                    Manager.AjouterAuRecemmentConsulte(Man.ConnectedUser, Man.CurrentPersonne);
                    Window.ContentControl.Content = new Acteur();
                    break;
            }
        }

        public static void ThemeSelected(object sender)
        {
            if (sender is not Button button) return;

            Window.ResetRadioButton();
            Window.Theme.Content = Window.Titre;
            Window.Titre.Text = (string) button.Content;
            Window.ContentControl.Content = new ToutesOeuvres();
        }

        public static void NewOeuvre()
        {
            Window.ResetRadioButton();
            Window.ContentControl.Content = new CreerLeaf();
        }

        public static void AvisUser(object sender)
        {
            Man.CurrentOeuvre = ((sender as ListBox)?.SelectedItem as KeyValuePair<Oeuvre, Modele.Avis>? ?? default).Key;
            Window.ContentControl.Content = Man.CurrentOeuvre switch
            {
                Modele.Leaf => new Leaf(),
                Modele.Composite => new Composite(),
                _ => Window.ContentControl
            };
        }


        public static void OeuvresDeleted()
        {
            Man.SupprimerOeuvre(Man.ConnectedUser, Man.CurrentOeuvre);
            Window.Theme.Content = null;
            Man.CurrentOeuvre = null;
            Window.ContentControl.Content = new Accueil();
        }


        public static void OeuvresModify()
        {
            Window.ContentControl.Content = Man.CurrentOeuvre switch
            {
                Modele.Leaf => new CreerLeaf(),
                Modele.Composite => new CreerComposite(),
                _ => Window.ContentControl
            };
        }

        public static void Acteur() => Window.ContentControl.Content = new Acteur();

        public static void Streaming() => Window.ContentControl.Content = new Streaming();
    }
}
