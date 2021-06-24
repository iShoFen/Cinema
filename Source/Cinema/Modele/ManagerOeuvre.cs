using System;
using System.Collections.Generic;
using System.Linq;
using static Modele.Constante;

namespace Modele
{
    /// <summary>
    /// La partie concernant les Oeuvre du Manager
    /// </summary>
    partial class Manager
    {
        // Partie Leaf

        /// <summary>
        /// Permet de créer une Leaf
        /// </summary>
        /// <param name="user">Le User qui veut créer une Leaf</param>
        /// <param name="type">Le Type de Leaf à créer</param>
        /// <param name="titre">Le titre de la Leaf</param>
        /// <param name="dateDeSortie">La date de sortie de la Leaf</param>
        /// <param name="lienImage">Le lien de l'image de la Leaf</param>
        /// <param name="synopsis">Le synopsis de la Leaf</param>
        /// <param name="theme">Le Theme de la Leaf</param>
        /// <param name="familleF">La valeur du FamilyFriendly de la Leaf</param>
        /// <param name="listePersonnes">La liste de Personne de la Leaf</param>
        /// <param name="listeStream">La liste de streams de la Leaf</param>
        /// <returns>La Leaf créer</returns>
        /// <seealso cref="Leaf"/>
        /// <seealso cref="User"/>
        /// <seealso cref="Personne"/>
        public Leaf CreerLeaf(User user, string type, string titre, DateTime dateDeSortie, string lienImage,
            string synopsis, Themes theme, bool familleF,
            IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<Personne, string>>>> listePersonnes = null,
            IEnumerable<Streaming> listeStream = null)
        {
            var oe = type switch
            {
                FILM => _oeuvres.Find(o => o.Titre.Equals(titre) && o is Film),
                EPISODE => _oeuvres.Find(o => o.Titre.Equals(titre) && o is Episode),
                _ => null
            };

            if (!user.IsAdmin || oe != null) return null;

            var leaf = _factory.CreerLeaf(type, titre, dateDeSortie, lienImage, synopsis, theme, familleF,
                listePersonnes, listeStream);

            if (leaf is null) return null;

            _oeuvres.Add(leaf);
            CurrentOeuvre = leaf;
            return leaf;
        }

        /// <summary>
        /// Permet de modifier une Leaf
        /// </summary>
        /// <param name="user">Le User qui veut modifier la Leaf</param>
        /// <param name="leaf">La Leaf à modifier</param>
        /// <param name="titre">Le nouveau titre de la Leaf</param>
        /// <param name="dateDeSortie">La nouvelle date de sortie de la Leaf</param>
        /// <param name="lienImage">Le nouveau lien d'image de la Leaf</param>
        /// <param name="synopsis">Le nouveau synopsis de la Leaf</param>
        /// <param name="theme">Le nouveau theme de la Leaf</param>
        /// <param name="familleF">La nouvelle valeur du FamilyFriendly de la Leaf</param>
        /// <param name="listePersonnes">La nouvelle liste de Personne de la Leaf</param>
        /// <param name="listeStream">La nouvelle liste de Stream de la Leaf</param>
        /// <seealso cref="Leaf"/>
        /// <seealso cref="User"/>
        /// <seealso cref="Personne"/>
        public void ModifierLeaf(User user, Leaf leaf, string titre, DateTime dateDeSortie, string lienImage,
            string synopsis, Themes theme, bool familleF,
            IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<Personne, string>>>> listePersonnes,
            IEnumerable<Streaming> listeStream)
        {
            if (user.IsAdmin)
            {
                var personnes = listePersonnes.ToList();
                var oldPers = new List<Personne>();
                oldPers.AddRange(leaf.Personnes.Select(p => p.Value).SelectMany(p => p.Keys));
                
                leaf.ModifierLeaf(titre, dateDeSortie, lienImage, synopsis, theme, familleF, personnes,
                    listeStream);

                var newPers = personnes.Select(p => p.Value.ToDictionary(pair => pair.Key, pair => pair.Value))
                    .SelectMany(p => p.Keys).ToList();

                ModifierOeuvrePersonnes(leaf, oldPers, newPers);
            }
            
            if (leaf.IsFamilleF) return;
            foreach (var us in _users.Where(us => us.IsModeFamille))
            {
                leaf.RetirerAvis(us);
                us.RetirerEnvie(leaf);
                us.RetirerConsulte(leaf);
            }
        }

        // Partie Composite

        /// <summary>
        /// Permet de créer un Composite
        /// </summary>
        /// <param name="user">Le User qui veut créer un Composite</param>
        /// <param name="type">Le Type de Composite à créer</param>
        /// <param name="titre">Le titre du Composite</param>
        /// <param name="dateDeSortie">La date de sortie du Composite</param>
        /// <param name="lienImage">Le lien de l'image du Composite</param>
        /// <param name="synopsis">Le synopsis du Composite</param>
        /// <param name="theme">Le Theme du Composite</param>
        /// <param name="familleF">La valeur du FamilyFriendly du Composite</param>
        /// <param name="listePersonnes">La liste de Personne du Composite</param>
        /// <param name="listeOeuvres">La liste d'Oeuvre du Composite</param>
        /// <returns>Le Composite créer</returns>
        /// <seealso cref="Composite"/>
        /// <seealso cref="User"/>
        /// <seealso cref="Personne"/>
        public Composite CreerComposite(User user, string type, string titre, DateTime dateDeSortie, string lienImage,
            string synopsis, Themes theme, bool familleF, IEnumerable<Oeuvre> listeOeuvres,
            IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<Personne, string>>>> listePersonnes = null)
        {
            var oe = type switch
            {
                TRILOGIE => _oeuvres.Find(o => o.Titre.Equals(titre) && o is Trilogie),
                SERIE => _oeuvres.Find(o => o.Titre.Equals(titre) && o is Serie),
                UNIVERS => _oeuvres.Find(o => o.Titre.Equals(titre) && o is Univers),
                _ => null
            };

            if (!user.IsAdmin || oe != null) return null;

            var comp = _factory.CreerComposite(type, titre, dateDeSortie, lienImage, synopsis, theme, familleF,
                listeOeuvres, listePersonnes);

            if (comp is null) return null;

            _oeuvres.Add(comp);
            CurrentOeuvre = comp;
            return comp;
        }

        /// <summary>
        /// Permet de modifier un Composite
        /// </summary>
        /// <param name="user">Le User qui veut modifier le Composite</param>
        /// <param name="comp">Le Composite à modifier</param>
        /// <param name="titre">Le nouveau titre du Composite</param>
        /// <param name="dateDeSortie">La nouvelle date de sortie du Composite</param>
        /// <param name="lienImage">Le nouveau lien d'image du Composite</param>
        /// <param name="synopsis">Le nouveau synopsis du Composite</param>
        /// <param name="theme">Le nouveau theme du Composite</param>
        /// <param name="familleF">La nouvelle valeur du FamilyFriendly du Composite</param>
        /// <param name="listePersonnes">La nouvelle liste de Personne du Composite</param>
        /// <seealso cref="Composite"/>
        /// <seealso cref="User"/>
        /// <seealso cref="Personne"/>
        public void ModifierComposite(User user, Composite comp, string titre, DateTime dateDeSortie, string lienImage,
            string synopsis, Themes theme, bool familleF,
            IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<Personne, string>>>> listePersonnes)
        {
            if (user.IsAdmin)
            {
                var personnes = listePersonnes.ToList();
                var oldPers = new List<Personne>();
                oldPers.AddRange(comp.Personnes.Select(p => p.Value).SelectMany(p => p.Keys));
                
                comp.ModifierComposite(titre, dateDeSortie, lienImage, synopsis, theme, familleF, personnes);

                var newPers = personnes.Select(p => p.Value.ToDictionary(pair => pair.Key, pair => pair.Value))
                    .SelectMany(p => p.Keys).ToList();

                ModifierOeuvrePersonnes(comp, oldPers, newPers);
            }

            if (comp.IsFamilleF) return;
            foreach (var us in _users.Where(us => us.IsModeFamille))
            {
                comp.RetirerAvis(us);
                us.RetirerEnvie(comp);
                us.RetirerConsulte(comp);
            }
        }

        /// <summary>
        /// Permet d'ajouter une Oeuvre à un Composite
        /// </summary>
        /// <param name="user">Le User qui veut ajouter une Oeuvre</param>
        /// <param name="comp">Le Composite sur lequel ajouter l'Oeuvre</param>
        /// <param name="oeuvre">L'Oeuvre à rajouter</param>
        /// <seealso cref="Composite"/>
        /// <seealso cref="Oeuvre"/>
        /// <seealso cref="User"/>
        public static void AjouterOeuvreComp(User user, Composite comp, Oeuvre oeuvre)
        {
            if (!user.IsAdmin) return;
            comp.AjouterOeuvres(new List<Oeuvre> {oeuvre});
        }

        /// <summary>
        /// Permet de retirer une Oeuvre à un Composite
        /// </summary>
        /// <param name="user">Le User qui veut retirer une Oeuvre</param>
        /// <param name="comp">Le Composite sur lequel retirer l'Oeuvre</param>
        /// <param name="oeuvre">L'Oeuvre à retirer</param>
        /// <seealso cref="Composite"/>
        /// <seealso cref="Oeuvre"/>
        /// <seealso cref="User"/>
        public void RetirerOeuvreComp(User user, Composite comp, Oeuvre oeuvre)
        {
            if (!user.IsAdmin) return;
            if (comp is Trilogie) SupprimerComposite(comp);
            else if(comp.ReOeuvres.Count -1 == LISTE_MIN) SupprimerOeuvre(user, comp);
            else comp.RetirerOeuvre(oeuvre);
        }

        //Partie Globale

        /// <summary>
        /// Permet de savoir si une Oeuvre existe
        /// </summary>
        /// <param name="type">Le type d'Oeuvre</param>
        /// <param name="titre">Le  titre de l'Oeuvre</param>
        /// <param name="date">La date de l'Oeuvre</param>
        /// <seealso cref="Oeuvre"/>
        /// <returns></returns>
        public bool HasOeuvre(string type, string titre, DateTime date)
        {
            titre ??= "";
            return !Oeuvres.Any(oe => type switch
            {
                FILM => oe is Film,
                EPISODE => oe is Episode,
                TRILOGIE => oe is Trilogie,
                SERIE => oe is Serie,
                UNIVERS => oe is Univers,
                _ => oe != null
            } && oe.Titre.Equals(titre) && oe.DateDeSortie.Equals(date));
        }

        private void ModifierOeuvrePersonnes(Oeuvre oeuvre, IEnumerable<Personne> oldPers, IEnumerable<Personne> newPers)
        {
            var newP = newPers.ToList();
            var old = oldPers.Except(newP).ToList();

            foreach (var pers in newP)
                pers.AjouterOeuvre(oeuvre);

            foreach (var pers in old)
            {
                foreach (var user in _users.Where(us => us.RecemmentConsulte.Contains(pers)).ToList())
                    user.RetirerConsulte(pers);

                _personnes.Remove(pers);
            }
        }

        /// <summary>
        /// Permet de retirer une Oeuvre d'une liste de Personne et une Personne de la liste de User si nécessaire. 
        /// </summary>
        /// <param name="oeuvre">L'Oeuvre à supprimer</param>
        /// <seealso cref="Oeuvre"/>
        /// <seealso cref="Personne"/>
        /// <seealso cref="User"/>
        private void RetirerOeuvrePersonnes(Oeuvre oeuvre)
        {
            foreach (var pers in _personnes.Where(pers => pers.Oeuvres.Contains(oeuvre)).ToList())
            {
                if (pers.Oeuvres.Count - 1 == LISTE_MIN)
                {
                    foreach (var user in _users.Where(user => user.RecemmentConsulte.Contains(pers)).ToList())
                        user.RetirerConsulte(pers);

                    _personnes.Remove(pers);
                }

                else pers.RetirerOeuvre(oeuvre);
            }
        }

        /// <summary>
        /// Permet de retirer une Oeuvre d'une liste de User
        /// </summary>
        /// <param name="oeuvre">L'Oeuvre à supprimer</param>
        /// <seealso cref="Oeuvre"/>
        /// <seealso cref="User"/>
        private void RetirerOeuvreUsers(Oeuvre oeuvre)
        {
            foreach (var user in _users
                .Where(user => user.ListeEnvie.Contains(oeuvre) || user.RecemmentConsulte.Contains(oeuvre)).ToList())
            {
                user.RetirerConsulte(oeuvre);
                user.RetirerEnvie(oeuvre);
            }
        }

        /// <summary>
        /// Permet de Supprimer un Composite
        /// </summary>
        /// <param name="comp">Le Composite à supprimer</param>
        /// <seealso cref="Composite"/>
        private void SupprimerComposite(Composite comp)
        {
            if (comp is not (Trilogie or Serie)) return;

            if (comp is Serie oeS)
                foreach (var oeu in oeS.ReOeuvres)
                {
                    RetirerOeuvrePersonnes(oeu);
                    _oeuvres.Remove(oeu);
                }

            foreach (var oeuvre1 in _oeuvres.Where(o => o is Univers).ToList())
            {
                if (oeuvre1 is not Univers oeU || !oeU.ReOeuvres.Contains(comp)) continue;

                if (oeU.ReOeuvres.Count - 1 == LISTE_MIN)
                {
                    RetirerOeuvrePersonnes(oeU);
                    RetirerOeuvreUsers(oeU);
                    _oeuvres.Remove(oeU);
                }

                else oeU.RetirerOeuvre(comp);
            }

            RetirerOeuvrePersonnes(comp);
            RetirerOeuvreUsers(comp);
            _oeuvres.Remove(comp);
            CurrentOeuvre = null;
        }

        /// <summary>
        /// Permet de supprimer une Leaf
        /// </summary>
        /// <param name="leaf">La Leaf à supprimer</param>
        /// <seealso cref="Leaf"/>
        private void SupprimerLeaf(Leaf leaf)
        {
            foreach (var oeuvre1 in _oeuvres.Where(o => o is Composite).ToList())
            {
                if (oeuvre1 is not Composite oeC || !oeC.ReOeuvres.Contains(leaf)) continue;

                if (oeC is Trilogie || oeC.ReOeuvres.Count - 1 == LISTE_MIN)
                    SupprimerComposite(oeC);

                else oeC.RetirerOeuvre(leaf);

            }

            RetirerOeuvrePersonnes(leaf);
            RetirerOeuvreUsers(leaf);
            _oeuvres.Remove(leaf);
        }

        /// <summary>
        /// Permet de supprimer une Oeuvre
        /// </summary>
        /// <param name="user">Le User qui veut supprimer une Oeuvre</param>
        /// <param name="oeuvre">L'Oeuvre à supprimer</param>
        /// <seealso cref="Oeuvre"/>
        /// <seealso cref="Personne"/>
        /// <seealso cref="User"/>
        public void SupprimerOeuvre(User user, Oeuvre oeuvre)
        {
            if (!user.IsAdmin) return;
            switch (oeuvre)
            {
                case Leaf leaf: SupprimerLeaf(leaf); break;

                case Trilogie or Serie: SupprimerComposite((Composite) oeuvre); break;

                default:
                    RetirerOeuvrePersonnes(oeuvre);
                    RetirerOeuvreUsers(oeuvre);
                    _oeuvres.Remove(oeuvre);
                    CurrentOeuvre = null;

                    break;
            }
        }
    }
}