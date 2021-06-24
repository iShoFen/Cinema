using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Modele;

namespace Appli
{
    /// <summary>
    /// Logique d'interaction pour PseudoChance.xaml
    /// </summary>
    public partial class PseudoChance : MetroWindow
    {
        private static Manager Man => (Application.Current as App)?.Man;

        public PseudoChance()
        {
            InitializeComponent();
            DataContext = Man.ConnectedUserPseudo;
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

        private void PseudoClear_OnClick(object sender, RoutedEventArgs e) => Pseudo.Text = "";
        

        private void ChangePseudo_OnClick(object sender, RoutedEventArgs e)
        {
            var pseudo = Pseudo.Text;

            if (string.IsNullOrWhiteSpace(pseudo))
            {
                this.ShowModalMessageExternal("Erreur de changement de pseudo", "Le pseudo ne peut être vide ou ne contenir que des espaces");
                return;
            }
            if(Man.IsAvailable(pseudo))
            {
                Man.ConnectedUserPseudo = pseudo;
                Close();
                return;
            }

            this.ShowModalMessageExternal("Erreur de changement de pseudo", "Le pseudo n'est pas disponible veuillez en choisir un autre");
        }

        private void Retour_OnClick(object sender, RoutedEventArgs e) => Close();
    }
}
