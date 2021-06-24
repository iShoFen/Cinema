using System.Collections.Generic;
using System.Linq;

namespace Modele
{
    /// <summary>
    /// La partie concernant les User du Manager
    /// </summary>
    /// <seealso cref="User"/>
    public partial class Manager
    { 
        /// <summary>
        /// Permet de créer un User
        /// </summary>
        /// <param name="pseudo">Le pseudo du User</param>
        /// <param name="mdp">Le mot de passe du User</param>
        /// <param name="user">Le User si il a bien été créer</param>
        /// <returns>Rend vrai si le User s'est créer si non rend faux</returns>
        /// <seealso cref="User"/>
        public bool CreerUser(string pseudo, string mdp, out User user)
        {
            user = null;

            if (!IsAvailable(pseudo)) return false;

            user = _factory.CreerUser(pseudo, mdp);
            _users.Add(user);

            ConnectedUser = user;
            return true;
        }

        /// <summary>
        /// Permet de se connecter si le User existe
        /// </summary>
        /// <param name="pseudo">Le pseudo du User</param>
        /// <param name="mdp">Le mot de passe du User</param>
        /// <param name="user">Le user si il existe</param>
        /// <returns>Rend vraie si le User a été trouvé et que le mot de passe est correct si non rend faux</returns>
        /// <seealso cref="User"/>
        public bool Connexion(string pseudo, string mdp, out User user)
        {
            user = null;
            var u = _users.Find(us => us.Pseudo.Equals(pseudo));
            if (u == null || !u.Mdp.Equals(mdp)) return false;

            user = u;

            ConnectedUser = user;
            return true;
        }

        /// <summary>
        /// Permet de changer de mot de passe
        /// </summary>
        /// <param name="user">Le User qui veut changer son mot de passe</param>
        /// <param name="pass">L'ancien mot de passe</param>
        /// <param name="newPass">Le nouveau mot de passe</param>
        /// <returns>Rend vraie si l'ancien mot de passe correspond à l'actuel si non rend faux</returns>
        /// <seealso cref="User"/>
        public static bool ChangerPassword(User user, string pass, string newPass)
        {
            if (user.Mdp != pass) return false;
            user.Mdp = newPass;
            return true;
        }

        /// <summary>
        /// Permet de savoir si un Pseudo est disponible ou non
        /// </summary>
        /// <param name="pseudo">Le pseudo à tester</param>
        /// <returns>Rend vraie si le pseudo est disponible si non faux</returns>
        /// <seealso cref="User"/>
        public bool IsAvailable(string pseudo)
        {
            var user = _users.Find(us => us.Pseudo.Equals(pseudo));

            return user == null;
        }

        /// <summary>
        /// Permet d'ajouter un avis à une Oeuvre
        /// </summary>
        /// <param name="user">L'User qui veut ajouter un avis</param>
        /// <param name="oe">L'Oeuvre sur laquelle l'avis veut être ajouté</param>
        /// <param name="note">La note de l'avis</param>
        /// <param name="commentaire">Le commentaire de l'avis</param>
        ///<seealso cref="Oeuvre"/>
        /// <seealso cref="User"/>
        public void AjouterAvis(User user, Oeuvre oe, float note, string commentaire)
        {
            var avis = _factory.CreerAvis(note, commentaire);
            oe.AjouterAvis(user, avis);
        }

        /// <summary>
        /// Permet de supprimer un Avis
        /// </summary>
        /// <param name="userA">Le User Admin</param>
        /// <param name="userS">Le User de l'avis</param>
        /// <param name="oeuvre">L'Oeuvre concerné</param>
        /// <seealso cref="Avis"/>
        /// <seealso cref="User"/>
        /// <seealso cref="Oeuvre"/>
        public static void SupprimerAvis(User userA, User userS, Oeuvre oeuvre)
        {
            if (!userA.IsAdmin) return;
            
            oeuvre.RetirerAvis(userS);
        }

        /// <summary>
        /// Permet de modifier les infos d'un User
        /// </summary>
        /// <param name="user">L'User qui veut changer ses infos</param>
        /// <param name="pass">Le mot de passe du User</param>
        /// <param name="pseudo">Le nouveau pseudo du User</param>
        /// <param name="lienImage">Le nouveau lien d'image du User</param>
        /// <param name="famille">La nouvelle valeur du mode famille du User</param>
        /// <param name="plateformes">La nouvelle liste de plateforme du User</param>
        /// <returns>Rend vraie si le mot de passe est correct si non rend faux</returns>
        /// <seealso cref="User"/>
        public bool ChangerInfos(User user, string pass, string pseudo, string lienImage, bool famille,
            IEnumerable<Plateformes> plateformes)
        {
            if (user.Mdp != pass) return false;

            if (famille)
            {
                foreach (var oe in _oeuvres.Where(oe => oe.IsFamilleF == false && oe.ListeAvis
                                                 .ToDictionary(pair => pair.Key, pair => pair.Value)
                                                 .ContainsKey(user)))
                {
                    oe.RetirerAvis(user);
                }

                foreach (var oe in user.ListeEnvie.Where(oe => !oe.IsFamilleF).ToList()) 
                    user.RetirerEnvie(oe);

                foreach (var ob in user.RecemmentConsulte.ToList())
                    if (ob is Oeuvre {IsFamilleF: false} oe)
                        user.RetirerConsulte(oe);
            }

            user.ChangerProfil(pseudo, lienImage, famille, plateformes);
            ConnectedUser = user;
            return true;
        }

        /// <summary>
        /// Permet d'ajouter un objet récemment consulté
        /// </summary>
        /// <param name="user">Le User qui vient de consulter</param>
        /// <param name="obj">L'objet à rajouter</param>
        /// <seealso cref="User"/>
        /// <seealso cref="Personne"/>
        /// <seealso cref="Oeuvre"/>
        public static void AjouterAuRecemmentConsulte(User user, object obj)
        {
            user?.AjouterConsulte(obj);
        }

        /// <summary>
        /// Permet d'ajouter une Oeuvre à sa liste d'envies
        /// </summary>
        /// <param name="user">Le User qui veut ajouter une Oeuvre</param>
        /// <param name="oeuvre">L'Oeuvre à rajouter</param>
        /// <seealso cref="Oeuvre"/>
        /// <seealso cref="User"/>
        public static void AjouterALaListeEnvies(User user, Oeuvre oeuvre)
        {
            if(!user.IsModeFamille || oeuvre.IsFamilleF)
                user.AjouterEnvie(oeuvre);
        }

        /// <summary>
        /// Permet de retirer une Oeuvre de sa liste d'envies
        /// </summary>
        /// <param name="user">Le User qui veut retirer une Oeuvre</param>
        /// <param name="oeuvre">L'Oeuvre à retirer</param>
        /// <seealso cref="Oeuvre"/>
        /// <seealso cref="User"/>
        public static void RetirerDeLaListeEnvies(User user, Oeuvre oeuvre) => user.RetirerEnvie(oeuvre);

        /// <summary>
        /// Permet de passer un User normal en  Administrateur
        /// </summary>
        /// <param name="userA">Le User Admin</param>
        /// <param name="userG">Le User à grader</param>
        /// <seealso cref="User"/>
        public static void GraderUser(User userA, User userG)
        {
            if (!userA.IsAdmin) return;
            userG.IsAdmin = true;
        }

        /// <summary>
        /// Permet de supprimer un User
        /// </summary>
        /// <param name="userA">Le User Admin</param>
        /// <param name="userS">Le User à supprimer</param>
        /// <seealso cref="User"/>
        public void SupprimerUser(User userA, User userS)
        {
            if (!userA.IsAdmin) return;
            foreach (var oe in _oeuvres.Where(oe => oe.ListeAvis
                                             .ToDictionary(pair => pair.Key, pair => pair.Value)
                                             .ContainsKey(userS)))
            {
                oe.RetirerAvis(userS);
            }
       
            _users.Remove(userS);
        }
    }
}