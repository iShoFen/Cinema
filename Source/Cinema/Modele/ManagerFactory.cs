using System;
using System.Collections.Generic;
using System.Linq;
using static Modele.Constante;

namespace Modele
{
    /// <summary>
    /// Instancie différents objet pour le manager
    /// </summary>
    public class ManagerFactory : IFactory
    {
        /// <summary>
        /// Permet de créer une Leaf
        /// </summary>
        /// <param name="type">Le type de Leaf</param>
        /// <param name="titre">Le titre de la Leaf</param>
        /// <param name="dateDeSortie">La date de sorite de la Leaf</param>
        /// <param name="lienImage">L'image de la Leaf</param>
        /// <param name="synopsis">Le synopsis de la Leaf</param>
        /// <param name="theme">Le Themes de la Leaf</param>
        /// <param name="familleF">La mode famille de la Leaf</param>
        /// <param name="listePersonnes">La liste de Personne de la Leaf</param>
        /// <param name="listeStream">La liste de Streaming de la Leaf</param>
        /// <param name="listeAvis">La liste d'Avis de la Leaf</param>
        /// <returns>Une Leaf</returns>
        /// <seealso cref="Leaf"/>
        /// <seealso cref="Themes"/>
        /// <seealso cref="Personne"/>
        /// <seealso cref="Streaming"/>
        /// <seealso cref="Avis"/>
        public Leaf CreerLeaf(string type, string titre, DateTime dateDeSortie, string lienImage, string synopsis,
            Themes theme, bool familleF,
            IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<Personne, string>>>> listePersonnes,
            IEnumerable<Streaming> listeStream, IEnumerable<KeyValuePair<User, Avis>> listeAvis = null)
        {
            object leaf = type switch
            {
                FILM when listePersonnes == null && listeStream == null => new Film(titre, dateDeSortie, lienImage,
                    synopsis, theme, familleF),

                FILM when listePersonnes != null && listeStream == null => new Film(titre, dateDeSortie, lienImage,
                    synopsis, theme, familleF, listePersonnes, new List<Streaming>()),

                FILM when listePersonnes == null => new Film(titre, dateDeSortie, lienImage, synopsis, theme, familleF,
                    new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), listeStream),

                FILM => new Film(titre, dateDeSortie, lienImage, synopsis, theme, familleF, listePersonnes, listeStream),

                EPISODE when listePersonnes == null && listeStream == null => new Episode(titre, dateDeSortie,
                    lienImage, synopsis, theme, familleF),

                EPISODE when listePersonnes != null && listeStream == null => new Episode(titre, dateDeSortie,
                    lienImage, synopsis, theme, familleF, listePersonnes, new List<Streaming>()),

                EPISODE when listePersonnes == null => new Episode(titre, dateDeSortie, lienImage, synopsis, theme, familleF,
                    new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), listeStream),

                EPISODE => new Episode(titre, dateDeSortie, lienImage, synopsis, theme, familleF, listePersonnes, listeStream),

                _ => null
            };

            if (leaf is not Leaf le) return null;

            foreach (var pers in le.Personnes.Select(p => p.Value).SelectMany(p => p.Keys))
                pers.AjouterOeuvre(le);

            return le;
        }

        /// <summary>
        /// Permet de créer un Composite
        /// </summary>
        /// <param name="type">Le type de Composite</param>
        /// <param name="titre">Le titre du Composite</param>
        /// <param name="dateDeSortie">La date de sortie du Composite</param>
        /// <param name="lienImage">L'image du Composite</param>
        /// <param name="synopsis">Le synopsis du Composite</param>
        /// <param name="theme">Le Themes du Composite</param>
        /// <param name="familleF">Le mode famille du Composite</param>
        /// <param name="listeOeuvres">La liste d'Oeuvre du Composite</param>
        /// <param name="listePersonnes">La liste de Personne du Composite</param>
        /// <param name="listeAvis">La liste d'Avis du Composite</param>
        /// <returns>Un Composite</returns>
        /// <seealso cref="Composite"/>
        /// <seealso cref="Themes"/>
        /// <seealso cref="Oeuvre"/>
        /// <seealso cref="Personne"/>
        /// <seealso cref="Avis"/>
        public Composite CreerComposite(string type, string titre, DateTime dateDeSortie, string lienImage,
            string synopsis, Themes theme, bool familleF, IEnumerable<Oeuvre> listeOeuvres,
            IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<Personne, string>>>> listePersonnes, IEnumerable<KeyValuePair<User, Avis>> listeAvis = null)
        {
            object comp = type switch
            {
                TRILOGIE when listePersonnes == null => new Trilogie(titre, dateDeSortie, lienImage, synopsis, theme, familleF,
                    new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), listeOeuvres),

                TRILOGIE => new Trilogie(titre, dateDeSortie, lienImage, synopsis, theme, familleF, listePersonnes, listeOeuvres),

                SERIE when listePersonnes == null => new Serie(titre, dateDeSortie, lienImage, synopsis, theme, familleF,
                    new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>()),

                SERIE => new Serie(titre, dateDeSortie, lienImage, synopsis, theme, familleF, listePersonnes),

                UNIVERS when listePersonnes == null => new Univers(titre, dateDeSortie, lienImage, synopsis, theme, familleF,
                    new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), listeOeuvres),

                UNIVERS => new Univers(titre, dateDeSortie, lienImage, synopsis, theme, familleF, listePersonnes, listeOeuvres),

                _ => null
            };

            if (comp is not Composite cmp) return null;
            if (cmp is not Serie && cmp.ReOeuvres.Count == 0) return null;

            foreach (var pers in cmp.Personnes.Select(p => p.Value).SelectMany(p => p.Keys))
                pers.AjouterOeuvre(cmp);

            return cmp;
        }

        /// <summary>
        /// Permet de créer une Personne
        /// </summary>
        /// <param name="prenom">Son prénom</param>
        /// <param name="nom">Son nom</param>
        /// <param name="bio">Sa biographie</param>
        /// <param name="nat">Sa nationalité</param>
        /// <param name="lien">Son image</param>
        /// <param name="date">Sa date de naissance</param>
        /// <returns>Une Personne</returns>
        /// <seealso cref="Personne"/>
        public Personne CreerPersonne(string prenom, string nom, string bio, string nat, string lien, DateTime date) =>
            new(nom, prenom, bio, nat, lien, date);

        /// <summary>
        /// Permet de créer des Streaming
        /// </summary>
        /// <param name="titre">Le  titre de la Leaf</param>
        /// <param name="d">Le dictionnaire de Plateformes et de lien</param>
        /// <returns>Une liste de Streaming</returns>
        /// <seealso cref="Leaf"/>
        /// <seealso cref="Plateformes"/>
        /// <seealso cref="Streaming"/>
        public IEnumerable<Streaming> AjouterStreamLeaf(string titre, IEnumerable<KeyValuePair<Plateformes, string>> d)=>
            d.Select(kvp => new Streaming(titre, kvp.Value, kvp.Key));

        /// <summary>
        /// Permet de créer un Avis
        /// </summary>
        /// <param name="note">La note de l'Avis</param>
        /// <param name="commentaire">Le commentaire de l'Avis</param>
        /// <returns>Un Avis</returns>
        /// <seealso cref="Avis"/>
        public Avis CreerAvis(float note, string commentaire) => new(note, commentaire);

        /// <summary>
        /// Permet de créer un User
        /// </summary>
        /// <param name="pseudo">Son Pseudo</param>
        /// <param name="mdp">Son mot de passe</param>
        /// <param name="photo">Sa photo</param>
        /// <param name="famille">Son mode famille</param>
        /// <param name="admin">Sa valeur admin</param>
        /// <param name="plateformes">Sa liste de Plateformes</param>
        /// <returns>Un user</returns>
        /// <seealso cref="User"/>
        /// <seealso cref="Plateformes"/>
        public User CreerUser(string pseudo, string mdp, string photo = null, bool famille = false, bool admin = false,
            IEnumerable<Plateformes> plateformes = null) => new(pseudo, mdp);

        public void ModifierLiseUser(User user, IEnumerable<Oeuvre> envie = null, IEnumerable<object> consulte = null)
        {
            throw new NotImplementedException();
        }
    }
}