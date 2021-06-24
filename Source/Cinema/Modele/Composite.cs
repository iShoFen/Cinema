using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace Modele
{
    public abstract class Composite : Oeuvre
    {
        /// <summary>
        /// Cette classe représente un composite
        /// </summary>
        public ReadOnlyObservableCollection<Oeuvre> ReOeuvres { get; }
        protected readonly ObservableCollection<Oeuvre> Oeuvres = new();

        /// <summary>
        /// Utilise le constructeur parent et initialise la liste d'Oeuvres
        /// </summary>
        /// <param name="titre">Le titre du Composite</param>
        /// <param name="dateDeSortie">La date de sortie du Composite</param>
        /// <param name="lienImage">Le chemin de l'image du Composite</param>
        /// <param name="synopsis">Le synopsis du Composite</param>
        /// <param name="theme">Le theme du Composite</param>
        /// <param name="isFamilleF">La valeur pour savoir si le film est pour la famille ou non</param>
        /// <param name="listePersonnes">La liste des personnes à rajouter</param>
        protected Composite(string titre, DateTime dateDeSortie, string lienImage, string synopsis,
            Themes theme, bool isFamilleF, IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<Personne, string>>>> listePersonnes)
            : base(titre, dateDeSortie, lienImage, synopsis, theme, isFamilleF)
        {
            ReOeuvres = new ReadOnlyObservableCollection<Oeuvre>(Oeuvres);
            AjouterPersonnes(listePersonnes);
        }

        /// <summary>
        /// Modifie les informations d'une instance de Composite
        /// </summary>
        /// <param name="titre">Le titre du Composites</param>
        /// <param name="dateDeSortie">La date de sortie du Composite</param>
        /// <param name="lienImage">Le chemin de l'image du Composite</param>
        /// <param name="synopsis">Le synopsis du Composite</param>
        /// <param name="theme">Le theme du Composite</param>
        /// <param name="familleF">La valeur pour savoir si le film est pour la famille ou non</param>
        /// <param name="listePersonnes">L listes des personnes à rajouter</param>
        internal void ModifierComposite(string titre, DateTime dateDeSortie, string lienImage, string synopsis,
            Themes theme, bool familleF, 
            IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<Personne, string>>>> listePersonnes) =>
            ModifierOeuvre(titre, dateDeSortie, lienImage, synopsis, theme, familleF, listePersonnes);

        internal abstract override void AjouterOeuvres(IEnumerable<Oeuvre> oeuvres);

        /// <summary>
        /// Permet de retirer une Oeuvre de la liste
        /// </summary>
        /// <param name="oeuvre">L'Oeuvre à retirer</param>
        internal sealed override void RetirerOeuvre(Oeuvre oeuvre) => Oeuvres.Remove(oeuvre);
    }
}