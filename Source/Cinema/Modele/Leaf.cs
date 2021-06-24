using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Modele
{
    /// <summary>
    /// Cette classe représente une feuille
    /// </summary>
    public abstract class Leaf : Oeuvre
    {
        public ReadOnlyCollection<Streaming> ListeStream { get; }
        private readonly List<Streaming> _listeStream = new();

        /// <summary>
        /// Utilise le constructeur parent pour initialiser les propriétés et initialise ListeStream
        /// </summary>
        /// <param name="titre">Le titre de la Leaf</param>
        /// <param name="dateDeSortie">La date de sortie de la Leaf</param>
        /// <param name="lienImage">Le chemin de l'image de la Leaf</param>
        /// <param name="synopsis">Le synopsis de la Leaf</param>
        /// <param name="theme">Le theme de la Leaf</param>
        /// <param name="isFamilleF">La valeur pour savoir si le film est pour la famille ou non</param>
        /// <see cref="Oeuvre"/>
        /// <seealso cref="Streaming"/>
        protected Leaf(string titre, DateTime dateDeSortie, string lienImage, string synopsis,
            Themes theme, bool isFamilleF)
            : base(titre, dateDeSortie, lienImage, synopsis, theme, isFamilleF) =>
            ListeStream = new ReadOnlyCollection<Streaming>(_listeStream);

        /// <summary>
        /// Utilise le précédant constructeur pour initialiser les propriétés et remplit les listes avec celles fournies
        /// </summary>
        /// <param name="titre">Le titre de la Leaf</param>
        /// <param name="dateDeSortie">La date de sortie de la Leaf</param>
        /// <param name="lienImage">Le chemin de l'image de la Leaf</param>
        /// <param name="synopsis">Le synopsis de la Leaf</param>
        /// <param name="theme">Le theme de la Leaf</param>
        /// <param name="isFamilleF">La valeur pour savoir si le film est pour la famille ou non</param>
        /// <param name="listePersonnes">La listes des personnes à rajouter</param>
        /// <param name="listeStream">La liste des liens de streaming du Film</param>
        /// <seealso cref="Streaming"/>
        protected Leaf(string titre, DateTime dateDeSortie, string lienImage, string synopsis,
            Themes theme, bool isFamilleF, IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<Personne, string>>>> listePersonnes,
            IEnumerable<Streaming> listeStream)
            : this(titre, dateDeSortie, lienImage, synopsis, theme, isFamilleF)
        {
            AjouterPersonnes(listePersonnes);

            AjouterStreams(listeStream);
        }

        /// <summary>
        /// Permet d'ajouter une liste de Streaming
        /// </summary>
        /// <param name="streams">La liste de Streaming</param>
        /// <seealso cref="Streaming"/>
        private void AjouterStreams(IEnumerable<Streaming> streams)
        {
            _listeStream.Clear();
            _listeStream.AddRange(streams.Distinct());
        }

        /// <summary>
        /// Permet de modifier les propriétés d'une Feuille
        /// </summary>
        /// <param name="titre">Le titre de la Leaf</param>
        /// <param name="dateDeSortie">La date de sortie de la Leaf</param>
        /// <param name="lienImage">Le chemin de l'image de la Leaf</param>
        /// <param name="synopsis">Le synopsis de la Leaf</param>
        /// <param name="theme">Le theme de la Leaf</param>
        /// <param name="familleF">La valeur pour savoir si le film est pour la famille ou non</param>
        /// <param name="listePersonnes">La liste des personnes a rajouter</param>
        /// <param name="listeStream">La liste des liens de streaming de la Leaf</param>
        internal void ModifierLeaf(string titre, DateTime dateDeSortie, string lienImage, string synopsis,
            Themes theme, bool familleF, IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<Personne, string>>>> listePersonnes,
            IEnumerable<Streaming> listeStream)
        {
            ModifierOeuvre(titre, dateDeSortie, lienImage, synopsis, theme, familleF, listePersonnes);

            AjouterStreams(listeStream);

            foreach (var str in _listeStream) str.Titre = Titre;
        }
    }
}