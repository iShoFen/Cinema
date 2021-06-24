using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static Modele.Constante;

namespace Modele
{
    /// <summary>
    /// Représente un utilisateur contenant : un Pseudo, un Password, une Image de profil, 
    /// une Liste des plateformes abonnées, une Liste d'envie et une liste d'objets récemment consultés
    /// </summary>
    public class User : IEquatable<User>
    {
        public string Pseudo { get; private set; }

        internal string Mdp { get; set; }

        /// <summary>
        /// Contient le lien de l'image de profil
        /// </summary>
        public string ImageProfil { get; private set; }

        /// <summary>
        /// Permet de savoir si l'utilisateur a activer ou non le mode famille
        /// </summary>
        public bool IsModeFamille { get; private set; }

        /// <summary>
        /// Permet de savoir si l'utilisateur est un administrateur ou non
        /// </summary>
        public bool IsAdmin { get; internal set; }

        /// <summary>
        /// La liste des plateformes auxquelles l’utilisateur est abonnées
        /// </summary>
        public ReadOnlyCollection<Plateformes> ListePlateformes { get; }
        private readonly List<Plateformes> _listePlateformes = new();

        public ReadOnlyCollection<Oeuvre> ListeEnvie { get; }
        private readonly List<Oeuvre> _listeEnvie = new();


        /// <summary>
        /// Une liste des différentes Personnes ou Oeuvres récemment consultés
        /// <seealso cref="Personne"/>
        /// <seealso cref="Oeuvre"/>
        /// </summary>
        public ReadOnlyCollection<object> RecemmentConsulte { get; }
        private readonly List<object> _recemmentConsulte = new();


        /// <summary>
        /// Initialise toutes les propriétés de User
        /// </summary>
        /// <param name="pseudo">Pseudo de l'utilisateur</param>
        /// <param name="mdp"> Mot de passe de l'utilisateur</param>
        internal User(string pseudo, string mdp)
        {
            Pseudo = pseudo;
            Mdp = mdp;
            ListePlateformes = new ReadOnlyCollection<Plateformes>(_listePlateformes);
            ListeEnvie = new ReadOnlyCollection<Oeuvre>(_listeEnvie);
            RecemmentConsulte = new ReadOnlyCollection<object>(_recemmentConsulte);
        }

        /// <summary>
        /// Permet de modifier les différentes informations d'une instance de User
        /// </summary>
        /// <param name="pseudo">Le Pseudo de l'utilisateur</param>
        /// <param name="pp">La photo de profil de l'utilisateur</param>
        /// <param name="famille">La valeur du mode famille (true/false)</param>
        /// <param name="plateformes">La liste des plateformes auxquelles est abonnées l'utilisateur</param>
        internal void ChangerProfil(string pseudo, string pp, bool famille, IEnumerable<Plateformes> plateformes)
        {
            Pseudo = pseudo;
            ImageProfil = pp;
            IsModeFamille = famille;

            _listePlateformes.Clear();
            _listePlateformes.AddRange(plateformes.Distinct());
        }

        /// <summary>
        /// Permet d'ajouter une Oeuvre à la liste d'envie de l'utilisateur
        /// </summary>
        /// <param name="oeuvre">L'Oeuvre à rajouter à la liste</param>
        /// <seealso cref="Oeuvre"/>
        internal void AjouterEnvie(Oeuvre oeuvre)
        {
            if (oeuvre is not (Film or Serie)) return;
            if (!_listeEnvie.Contains(oeuvre))
                _listeEnvie.Add(oeuvre);
        }

        /// <summary>
        /// Permet de retirer une Oeuvre de la liste d'envie de l'utilisateur
        /// </summary>
        /// <param name="oeuvre">L'Oeuvre à retirer</param>
        /// <seealso cref="Oeuvre"/>
        internal void RetirerEnvie(Oeuvre oeuvre) => _listeEnvie.Remove(oeuvre);


        /// <summary>
        /// Permet d'ajouter une Oeuvre ou une Personne à la liste des récemment consulté 
        /// </summary>
        /// <param name="obj">L'objet à rajouter</param>
        /// <seealso cref="Personne"/>
        /// <seealso cref="Oeuvre"/>
        internal void AjouterConsulte (object obj)
        {
            object ob = obj switch
            {
                Personne pers => pers,
                Oeuvre  oeu => oeu,
                _ => null
            };

            if (ob == null) return;
            if (!_recemmentConsulte.Contains(ob))
            {
                if (_recemmentConsulte.Count < CONSULTE_MAX)
                {
                    _recemmentConsulte.Insert(0, ob);
                }
                else
                {
                    _recemmentConsulte.RemoveAt(_recemmentConsulte.Count-1);
                    _recemmentConsulte.Insert(0, ob);
                }
            }
            else
            {
                _recemmentConsulte.Remove(ob);
                _recemmentConsulte.Insert(0, ob);
            }
        }

        /// <summary>
        /// Permet de retirer une Oeuvre de la liste des Consulte (n'est utilisé que en cas de modifications de profil ou d'une Oeuvre)
        /// </summary>
        /// <param name="obj">L'Oeuvre ou la Personne à supprimer de la liste</param>
        /// <seealso cref="Oeuvre"/>
        /// <seealso cref="Personne"/>
        internal void RetirerConsulte(object obj) => _recemmentConsulte.Remove(obj);


        /// <summary>
        /// Définis la méthode equals de IEquatable
        /// </summary>
        /// <param name="other"> prend un objet de type User</param>
        /// <returns> Rend true si égale et false s'il ne l'est pas</returns>
        /// <seealso cref="IEquatable{T}"/>
        public bool Equals(User other) => Pseudo == other?.Pseudo;

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
            return obj.GetType() == GetType() && Equals((User)obj);
        }

        /// <summary>
        /// Redéfinis la méthode GetHasCode de object
        /// </summary>
        /// <returns>Retourne un entier qui varie en fonction de la valeurs de différentes propriétés testées</returns>
        public override int GetHashCode() => Pseudo != null ? Pseudo.GetHashCode() : 0;
    }
}