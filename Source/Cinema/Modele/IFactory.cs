using System;
using System.Collections.Generic;

namespace Modele
{
    public interface IFactory
    {
        public Leaf CreerLeaf(string type, string titre, DateTime dateDeSortie, string lienImage,
            string synopsis, Themes theme, bool familleF,
            IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<Personne, string>>>> listePersonnes,
            IEnumerable<Streaming> listeStream, IEnumerable<KeyValuePair<User, Avis>> listeAvis = null);

        public Composite CreerComposite(string type, string titre, DateTime dateDeSortie, string lienImage,
            string synopsis, Themes theme, bool familleF, IEnumerable<Oeuvre> listeOeuvres,
            IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<Personne, string>>>> listePersonnes,
            IEnumerable<KeyValuePair<User, Avis>> listeAvis = null);

        public Personne CreerPersonne(string prenom, string nom, string bio, string nat, string lien,
            DateTime date);

        public IEnumerable<Streaming> AjouterStreamLeaf(string titre, IEnumerable<KeyValuePair<Plateformes, string>> d);

        public Avis CreerAvis(float note, string commentaire);

        public User CreerUser(string pseudo, string mdp, string photo = null, bool famille = false, bool admin = false,
            IEnumerable<Plateformes> plateformes = null);

        public void ModifierLiseUser(User user, IEnumerable<Oeuvre> envie = null, IEnumerable<object> consulte = null);
    }
}