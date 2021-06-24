using System;
using System.Collections.Generic;

namespace Modele
{
    /// <summary>
    /// Cette classe représente un Episode
    /// </summary>
    public sealed class Episode : Leaf, IEquatable<Episode>
    {
        /// <summary>
        /// Utilise le constructeur parent pour initialiser les propriétés
        /// </summary>
        /// <param name="titre">Le titre de l'Episode</param>
        /// <param name="dateDeSortie">La date de sortie de l'Episode</param>
        /// <param name="lienImage">Le chemin de l'image de l'Episode</param>
        /// <param name="synopsis">Le synopsis de l'Episode</param>
        /// <param name="theme">Le theme du Film</param>
        /// <param name="isFamilleF">La valeur pour savoir si le film est pour la famille ou non</param>
        internal Episode(string titre, DateTime dateDeSortie, string lienImage, string synopsis,
            Themes theme, bool isFamilleF) : base(titre, dateDeSortie, lienImage, synopsis, theme, isFamilleF) {}

        /// <summary>
        /// Utilise le constructeur  parent pour initialiser les propriétés et remplit les listes avec celles fournies
        /// </summary>
        /// <param name="titre">Le titre de l'Episode</param>
        /// <param name="dateDeSortie">La date de sortie de l'Episode</param>
        /// <param name="lienImage">Le chemin de l'image de l'Episode</param>
        /// <param name="synopsis">Le synopsis de l'Episode</param>
        /// <param name="theme">Le theme de l'Episode</param>
        /// <param name="isFamilleF">La valeur pour savoir si le film est pour la famille ou non</param>
        /// <param name="listePersonnes">La liste des personnes à rajouter</param>
        /// <param name="listeStream">La liste des liens de streaming de l'Episode</param>
        /// <see cref="Leaf"/>
        /// <seealso cref="Personne"/>
        /// <seealso cref="Avis"/>
        /// <seealso cref="Streaming"/>
        internal Episode(string titre, DateTime dateDeSortie, string lienImage, string synopsis,
            Themes theme, bool isFamilleF, IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<Personne, string>>>> listePersonnes, IEnumerable<Streaming> listeStream)
            :base(titre, dateDeSortie, lienImage, synopsis, theme, isFamilleF, listePersonnes, listeStream) {}

        
        /// <summary>
        /// Définis la méthode equals de IEquatable
        /// </summary>
        /// <param name="other">Prend un objet de type Episode</param>
        /// <returns>Rend true si égale et false s'il ne l'est pas</returns>
        /// <seealso cref="IEquatable{T}"/>
        public bool Equals(Episode other) => Titre == other?.Titre && DateDeSortie == other?.DateDeSortie;

        /// <summary>
        /// Redéfinis la méthode Equals de object
        /// </summary>
        /// <param name="obj">Prend un object</param>
        /// <returns> Rend true si égale et false s'il ne l'est pas </returns>
        /// <seealso cref="object"/>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals(obj as Episode);
        }

        /// <summary>
        /// Redéfinis la méthode GetHasCode de object
        /// </summary>
        /// <returns>Retourne un entier qui varie en fonction de la valeurs de différentes propriétés testées</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Titre != null ? Titre.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ DateDeSortie.GetHashCode();
                return hashCode;
            }
        }
    }
}