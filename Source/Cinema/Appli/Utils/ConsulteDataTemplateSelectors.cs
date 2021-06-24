using System.Windows;
using System.Windows.Controls;
using Modele;

namespace Appli.Utils
{
    public class ConsulteDataTemplateSelectors : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (container is not FrameworkElement element) return null;

            return item switch
            {
                Oeuvre => element.FindResource("Oeuvre") as DataTemplate,
                Personne => element.FindResource("Acteur") as DataTemplate,
                _ => null
            };
        }
    }
}
