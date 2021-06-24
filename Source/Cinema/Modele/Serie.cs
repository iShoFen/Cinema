using System;
using System.Collections.Generic;
using System.Linq;

namespace Modele
{
    public sealed class Serie : Composite, IEquatable<Serie>
    {

        /// <summary>
        /// Utilise le constructeur parent et rajoute des Oeuvres à la liste
        /// </summary>
        /// <param name="titre">Le titre de la Serie</param>
        /// <param name="dateDeSortie">La date de sortie de la Serie</param>
        /// <param name="lienImage">Le chemin de l'image de la Serie</param>
        /// <param name="synopsis">Le synopsis de la Serie</param>
        /// <param name="theme">Le theme de la Serie</param>
        /// <param name="isFamilleF">La valeur pour savoir si le film est pour la famille ou non</param>
        /// <param name="listePersonnes">La listes des Personnes à rajouter</param>
        /// <seealso cref="Personne"/>
        /// <seealso cref="Oeuvre"/>
        internal Serie(string titre, DateTime dateDeSortie, string lienImage, string synopsis, Themes theme, bool isFamilleF,
            IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<Personne, string>>>> listePersonnes) :
            base(titre, dateDeSortie, lienImage, synopsis, theme, isFamilleF, listePersonnes){}
        
        /// <summary>
        /// Permet d'ajouter une Oeuvre de type Episode à la liste
        /// </summary>
        /// <param name="oeuvres">La liste d'Oeuvres</param>
        /// <see cref="Film"/>
        /// <seealso cref="Oeuvre"/>
        internal override void AjouterOeuvres(IEnumerable<Oeuvre> oeuvres)
        {
            foreach (var oeuvre in oeuvres.Where(oeuvre => oeuvre is Episode && !Oeuvres.Contains(oeuvre)))
                Oeuvres.Add(oeuvre);
        }

        
        /// <summary>
        /// Définis la méthode equals de IEquatable
        /// </summary>
        /// <param name="other">Prend un objet de type Serie</param>
        /// <returns>Rend true si égale et false s'il ne l'est pas</returns>
        /// <seealso cref="IEquatable{T}"/>
        public bool Equals(Serie other) => Titre == other?.Titre && DateDeSortie == other?.DateDeSortie;

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
            return obj.GetType() == GetType() && Equals(obj as Serie);
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