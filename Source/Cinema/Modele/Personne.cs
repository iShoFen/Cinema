using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Modele
{
    /// <summary>
    /// Représente une Personne avec : un Nom, Prénom, Biographie, Nationalite, Lien d'image et Date de naissance
    /// </summary>
    public class Personne : IEquatable<Personne>
    {
        public string Nom { get; private set; }

        public string Prenom { get; private set; }

        public string Biographie { get; private set; }

        public string Nationalite { get; private set; }
        
        /// <summary>
        /// Contient le chemin de l'image à générer
        /// </summary>
        public string LienImage { get; private set; }

        public DateTime DateDeNaissance { get; private set; }

        public ReadOnlyCollection<Oeuvre> Oeuvres { get; }
        private readonly List<Oeuvre> _oeuvres = new();

        /// <summary>
        /// Ce constructeur initialise toutes les informations de la Personne
        /// </summary>
        /// <param name="nom"> le Nom de la Personne</param>
        /// <param name="prenom"> le Prénom de la Personne</param>
        /// <param name="biographie"> la Biographie de la Personne</param>
        /// <param name="nationalite"> la Nationalité de la Personne</param>
        /// <param name="lienImage"> le chemin de l'image de la Personne</param>
        /// <param name="dateDeNaissance"> la Date de naissance de la Personne</param>
        internal Personne(string nom, string prenom, string biographie, string nationalite, string lienImage, DateTime dateDeNaissance)
        {
            Nom = nom;
            Prenom = prenom;
            Biographie = biographie;
            Nationalite = nationalite;
            LienImage = lienImage;
            DateDeNaissance = dateDeNaissance;
            Oeuvres = new ReadOnlyCollection<Oeuvre>(_oeuvres);
        }

        /// <summary>
        /// Cette méthode modifie les informations d'une instance de Personne
        /// </summary>
        /// <param name="nom"> le Nom de la Personne</param>
        /// <param name="prenom"> le Prénom de la Personne</param>
        /// <param name="biographie"> la Biographie de la Personne</param>
        /// <param name="nationalite"> la Nationalité de la Personne</param>
        /// <param name="lienImage"> le chemin de l'image de la Personne</param>
        /// <param name="dateDeNaissance"> la Date de naissance de la Personne</param>
        internal void ModifierPersonne(string nom, string prenom, string biographie, string nationalite, string lienImage,
            DateTime dateDeNaissance)
        {
            Nom = nom;
            Prenom = prenom;
            Biographie = biographie;
            Nationalite = nationalite;
            LienImage = lienImage;
            DateDeNaissance = dateDeNaissance;
        }

        /// <summary>
        /// Permet d'ajouter une Oeuvre à notre liste
        /// </summary>
        /// <param name="oeuvre">L'Oeuvre à ajouter</param>
        /// <seealso cref="Oeuvre"/>
        internal void AjouterOeuvre(Oeuvre oeuvre)
        {
            if (!_oeuvres.Contains(oeuvre))
                _oeuvres.Add(oeuvre);
        }

        /// <summary>
        /// Permet de Retirer une Oeuvre de la liste
        /// </summary>
        /// <param name="oeuvre">L'Oeuvre à retirer</param>
        internal void RetirerOeuvre(Oeuvre oeuvre) => _oeuvres.Remove(oeuvre);

        /// <summary>
        /// Définis la méthode equals de IEquatable
        /// </summary>
        /// <param name="other"> prend un objet de type Personne</param>
        /// <returns> Rend true si égale et false s'il ne l'est pas</returns>
        /// <seealso cref="IEquatable{T}"/>
        public bool Equals(Personne other) => Nom == other?.Nom && Prenom == other?.Prenom && DateDeNaissance == other?.DateDeNaissance;

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
            return obj.GetType() == GetType() && Equals((Personne) obj);
        }

        /// <summary>
        /// Redéfinis la méthode GetHasCode de object
        /// </summary>
        /// <returns>Retourne un entier qui varie en fonction de la valeurs de différentes propriétés testées</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Nom != null ? Nom.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Prenom != null ? Prenom.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ DateDeNaissance.GetHashCode();
                return hashCode;
            }
        }
    }
}