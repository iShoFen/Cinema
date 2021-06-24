using System;

namespace Modele
{
    /// <summary>
    /// Un Avis comprend une (note et un commentaire)
    /// </summary>
    public class Avis : IEquatable<Avis>
    {
        private const int TROP_PETIT = 0;

        private const int TROP_GRAND = 5;

        public float Note
        {
            get => _note;
            private init
            {
                _note = value switch
                {
                    < TROP_PETIT => TROP_PETIT,
                    > TROP_GRAND => TROP_GRAND,
                    _ => value
                };
            }
        }

        private readonly float _note;

        public string Commentaire { get; }

        /// <summary>
        /// Constructeur prenant comment paramètre une note et un commentaire
        /// </summary>
        /// <param name="note"> la Note de l'Avis</param>
        /// <param name="commentaire"> le Commentaire de l'Avis</param>
        internal Avis(float note, string commentaire)
        {
            Note = note;
            Commentaire = commentaire;
        }

        /// <summary>
        /// Définis la méthode equals de IEquatable
        /// </summary>
        /// <param name="other"> prend un objet de type Avis</param>
        /// <returns> Rend true si égale et false s'il ne l'est pas</returns>
        /// <seealso cref="IEquatable{T}"/>
        public bool Equals(Avis other) => _note.Equals(other?._note) && Commentaire == other?.Commentaire;

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
            return obj.GetType() == GetType() && Equals(obj as Avis);
        }

        /// <summary>
        /// Redéfinis la méthode GetHasCode de object
        /// </summary>
        /// <returns>Retourne un entier qui varie en fonction de la valeurs de différentes propriétés testées</returns>
        public override int GetHashCode() =>
            unchecked((_note.GetHashCode() * 397) ^ (Commentaire != null ? Commentaire.GetHashCode() : 0));
    }
}