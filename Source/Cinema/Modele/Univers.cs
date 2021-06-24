 using System;
using System.Collections.Generic;
using System.Linq;

namespace Modele
{
    public sealed class Univers : Composite, IEquatable<Univers>
    {
        /// <summary>
        /// Utilise le constructeur parent et rajoute des Oeuvres à la liste
        /// </summary>
        /// <param name="titre">Le titre de l'Univers</param>
        /// <param name="dateDeSortie">La date de sortie de l'Univers</param>
        /// <param name="lienImage">Le chemin de l'image de l'Univers</param>
        /// <param name="synopsis">Le synopsis de l'Univers</param>
        /// <param name="theme">Le theme de l'Univers</param>
        /// <param name="isFamilleF">La valeur pour savoir si le film est pour la famille ou non</param>
        /// <param name="listePersonnes">La listes des Personnes à rajouter</param>
        /// <param name="oeuvres">La liste d'Oeuvres à rajouter</param>
        /// <seealso cref="Personne"/>
        /// <seealso cref="Oeuvre"/>
        internal Univers(string titre, DateTime dateDeSortie, string lienImage, string synopsis, Themes theme, bool isFamilleF,
            IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<Personne, string>>>> listePersonnes, IEnumerable<Oeuvre> oeuvres) :
            base(titre, dateDeSortie, lienImage, synopsis, theme, isFamilleF, listePersonnes) => AjouterOeuvres(oeuvres);

        /// <summary>
        /// Permet d'ajouter une Oeuvre de type Trilogie, Serie ou Film à la liste
        /// </summary>
        /// <param name="oeuvres">La liste d'Oeuvres</param>
        /// <see cref="Film"/>
        /// <seealso cref="Oeuvre"/>
        internal override void AjouterOeuvres(IEnumerable<Oeuvre> oeuvres)
        {
            var oeuvresL = oeuvres.ToList();

            foreach (var oeuvre in oeuvresL.Where(o => !Oeuvres.Contains(o) && o is Trilogie or Serie or Film ))
                Oeuvres.Add(oeuvre);
            
        }

        
        /// <summary>
        /// Définis la méthode equals de IEquatable
        /// </summary>
        /// <param name="other">Prend un objet de type Film</param>
        /// <returns>Rend true si égale et false s'il ne l'est pas</returns>
        /// <seealso cref="IEquatable{T}"/>
        public bool Equals(Univers other) => Titre == other?.Titre && DateDeSortie == other?.DateDeSortie;

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
            return obj.GetType() == GetType() && Equals(obj as Univers);
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