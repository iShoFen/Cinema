using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Modele;

namespace Appli
{
    /// <summary>
    /// Logique d'interaction pour NewAvis.xaml
    /// </summary>
    public partial class NewAvis : MetroWindow
    {
        private static Manager Man => (Application.Current as App)?.Man;

        public NewAvis()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (!float.TryParse(Note.Text, out var note))
            {
                this.ShowModalMessageExternal("Erreur de note", "Veuillez entrez une Note !");
                return;
            }


            Man.AjouterAvis(Man.ConnectedUser, Man.CurrentOeuvre, note, Commentaire.Text);
            Close();
        }
    }
}
