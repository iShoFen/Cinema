using System;

namespace Modele
{
    /// <summary>
    /// Streaming contient un lien et une plateforme pour regarder le film sur internet
    /// </summary>
    public class Streaming : IEquatable<Streaming>
    {
        public string Titre { get; internal set; }

        public string Lien { get; }

        public Plateformes Plateforme{ get; }

        /// <summary>
        /// Constructeur de la classe Streaming
        /// </summary>
        /// <param name="titre"> Le titre du Film</param>
        /// <param name="lien"> Le lien vers le site internet</param>
        /// <param name="plateformes"> La plateformes du Streaming</param>
        /// <seealso cref="Plateforme"/>
        internal Streaming(string titre, string lien, Plateformes plateformes)
        {
            Titre = titre;
            Lien = lien;
            Plateforme = plateformes;
        }

        /// <summary>
        /// Définis la méthode equals de IEquatable
        /// </summary>
        /// <param name="other"> prend un objet de type Streaming</param>
        /// <returns> Rend true si égale et false s'il ne l'est pas</returns>
        /// <seealso cref="IEquatable{T}"/>
        public bool Equals(Streaming other) => Plateforme == other?.Plateforme;

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
            return obj.GetType() == GetType() && Equals((Streaming) obj);
        }
        /// <summary>
        /// Redéfinis la méthode GetHasCode de object
        /// </summary>
        /// <returns>Retourne un entier qui varie en fonction de la valeurs de différentes propriétés testées</returns>
        public override int GetHashCode() => 31 * (int) Plateforme;
    }
}