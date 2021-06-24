using System.Windows;
using System.Windows.Controls;
using Modele;

namespace Appli.Navigator
{
    public static class OeuvreNavigator
    {
        private static Manager Man => (Application.Current as App)?.Man;
        private static MainWindow Window => (Application.Current as App)?.MainWindow as MainWindow;

        public static int TypeIndex = 0;

        public static void TypeSwitch(object sender, UserControl uc)
        {
            if (sender is not ComboBox combo) return;

            TypeIndex = combo.SelectedIndex;

            Window.ContentControl.Content = TypeIndex switch
            {
                <= 1 when uc is not CreerLeaf => new CreerLeaf(),
                > 1 when uc is not CreerComposite => new CreerComposite(),
                _ => Window.ContentControl.Content
            };
        }

        public static void OeuvreOnCreated(Oeuvre o)
        {
            TypeIndex = 0;
            Window.ContentControl.Content = Man.CurrentOeuvre switch
            {
                Modele.Leaf => new Leaf(),
                Modele.Composite => new Composite(),
                _ => Window.ContentControl
            };
        }
    }
}
