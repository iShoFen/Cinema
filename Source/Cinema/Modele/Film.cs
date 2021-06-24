using System;
using System.Collections.Generic;

namespace Modele
{
    /// <summary>
    /// Représente un film contient les meme propriétés qu'une feuille
    /// </summary>
    public sealed class Film : Leaf, IEquatable<Film>
    {
        /// <summary>
        /// Utilise le constructeur parent pour initialiser les propriétés
        /// </summary>
        /// <param name="titre">Le titre du Film</param>
        /// <param name="dateDeSortie">La date de sortie du Film</param>
        /// <param name="lienImage">Le chemin de l'image du Film</param>
        /// <param name="synopsis">Le synopsis du Film</param>
        /// <param name="theme">Le theme du Film</param>
        /// <param name="isFamilleF">La valeur pour savoir si le film est pour la famille ou non</param>
        internal Film(string titre, DateTime dateDeSortie, string lienImage, string synopsis,
            Themes theme, bool isFamilleF) : base(titre, dateDeSortie, lienImage, synopsis, theme, isFamilleF) {}

        /// <summary>
        /// Utilise le constructeur  parent pour initialiser les propriétés et remplit les listes avec celles fournies
        /// </summary>
        /// <param name="titre">Le titre du Film</param>
        /// <param name="dateDeSortie">La date de sortie du Film</param>
        /// <param name="lienImage">Le chemin de l'image du Film</param>
        /// <param name="synopsis">Le synopsis du Film</param>
        /// <param name="theme">Le theme du Film</param>
        /// <param name="isFamilleF">La valeur pour savoir si le film est pour la famille ou non</param>
        /// <param name="listePersonnes">La liste des personnes à rajouter</param>
        /// <param name="listeStream">La liste des liens de streaming du Film</param>
        /// <see cref="Leaf"/>
        /// <seealso cref="Personne"/>
        /// <seealso cref="Avis"/>
        /// <seealso cref="Streaming"/>
        internal Film(string titre, DateTime dateDeSortie, string lienImage, string synopsis,
            Themes theme, bool isFamilleF, IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<Personne, string>>>> listePersonnes, IEnumerable<Streaming> listeStream)
            :base(titre, dateDeSortie, lienImage, synopsis, theme, isFamilleF, listePersonnes, listeStream) {}

        
        /// <summary>
        /// Définis la méthode equals de IEquatable
        /// </summary>
        /// <param name="other">Prend un objet de type Film</param>
        /// <returns>Rend true si égale et false s'il ne l'est pas</returns>
        /// <seealso cref="IEquatable{T}"/>
        public bool Equals(Film other) => Titre == other?.Titre && DateDeSortie == other?.DateDeSortie;

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
            return obj.GetType() == GetType() && Equals(obj as Film);
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