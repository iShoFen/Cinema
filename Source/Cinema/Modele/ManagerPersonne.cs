using System;

namespace Modele
{
    /// <summary>
    /// Partie concernant les Personne du Manager
    /// </summary>
    /// <seealso cref="Personne"/>
    public partial class Manager
    { 
        /// <summary>
        /// Permet de créer une Personne pour une Oeuvre
        /// </summary>
        /// <param name="user">L'utilisateur qui crée la Personne</param>
        /// <param name="prenom">Le prénom de la Personne</param>
        /// <param name="nom">Le nom de la Personne</param>
        /// <param name="bio">La biographie de la Personne</param>
        /// <param name="nat">La nationalité de la Personne</param>
        /// <param name="lien">Le lien de l'image de la Personne</param>
        /// <param name="date">La date de naissance de la Personne</param>
        /// <returns>Rend la Personne créer</returns>
        /// <seealso cref="Personne"/>
        ///<seealso cref="Oeuvre"/>
        public Personne CreerPersonne(User user, string prenom, string nom, string bio, string nat, string lien, DateTime date)
        {
            if (user is not {IsAdmin: true} || HasPersonne(prenom, nom, out _)) return null;

            var pers = _factory.CreerPersonne(prenom, nom, bio, nat, lien, date);
            _personnes.Add(pers);

            CurrentPersonne = pers;
            return pers;
        }

        /// <summary>
        /// Permet de savoir si une Personne existe déjà ou non
        /// </summary>
        /// <param name="prenom">Le prénom de la Personne recherché</param>
        /// <param name="nom">Le Nom de la Personne recherché</param>
        /// <param name="pers">La Personne si elle est trouvé</param>
        /// <returns>Rend vraie si une Personne est trouvé si non rend faux</returns>
        /// <seealso cref="Personne"/>
        public bool HasPersonne(string prenom, string nom, out Personne pers)
        {
            pers = _personnes.Find(us => us.Prenom.Equals(prenom) && us.Nom.Equals(nom));
            return pers != null;
        }

        public bool ModifPersonne(User user, Personne pers, string nom, string prenom, string biographie, string nationalite,
            string lienImage,
            DateTime dateDeNaissance)
        {
            if (!user.IsAdmin) return false;

            if(HasPersonne(prenom, nom, out var persTmp))
                if (!pers.Equals(persTmp)) return false;

            pers.ModifierPersonne(nom, prenom, biographie, nationalite, lienImage, dateDeNaissance);
            return true;
        }

        /// <summary>
        /// Permet de supprimer une Personne
        /// </summary>
        /// <param name="user">Le User qui veut supprimer la personne</param>
        /// <param name="pers">La personne à supprimer</param>
        /// <seealso cref="Personne"/>
        public void SupprimerPersonne(User user, Personne pers)
        {
            if (!user.IsAdmin) return;

            _personnes.Remove(pers);
        }
    }
}