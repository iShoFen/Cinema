using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using static Modele.Constante;

[assembly: 
    InternalsVisibleTo("UnitTests"), 
    InternalsVisibleTo("LINQ_to_Data")]
namespace Modele
{
    /// <summary>
    /// La partie global du Manager
    /// </summary>
    public partial class Manager : INotifyPropertyChanged
    {
        internal ReadOnlyCollection<User> Users { get; }
        private readonly List<User> _users = new();

        internal ReadOnlyCollection<Oeuvre> Oeuvres { get; }

        private readonly List<Oeuvre> _oeuvres = new();

        internal ReadOnlyCollection<Personne> Personnes { get; }
        private readonly List<Personne> _personnes = new();

        public User ConnectedUser
        {
            get => _connectedUser;
            set
            {
                _connectedUser = value;
                OnPropertyChanged(nameof(ConnectedUser));
            }
        }

        private User _connectedUser;

        public string ConnectedUserPseudo
        {
            get => _connectedUserPseudo;
            set
            {
                _connectedUserPseudo = value;
                OnPropertyChanged(nameof(ConnectedUserPseudo));
            }
        }

        private string _connectedUserPseudo;

        public string ConnectedUserPhoto
        {
            get => _connectedUserPhoto;
            set
            {
                _connectedUserPhoto = value;
                OnPropertyChanged(nameof(ConnectedUserPhoto));
            }
        }

        private string _connectedUserPhoto;
        
        public bool ConnectedUserFamille { get; set; }

        public List<Plateformes> ConnectedUserPlateformes { get; } = new();

        public User CurrentUser { get; set; }
        
        public Oeuvre CurrentOeuvre { get; set; }

        public string CurrentStream
        {
            get => _currentStream;
            set
            {
                _currentStream = value;
                OnPropertyChanged(nameof(CurrentStream));
            }
        }

        private string _currentStream;

        public Personne CurrentPersonne
        {
            get => _currentPersonne;
            set
            {
                _currentPersonne = value;
                OnPropertyChanged(nameof(CurrentPersonne));
            }
        }

        private Personne _currentPersonne;

        private readonly IFactory _factory;

        private IPersistance _persistance;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// Manager Par défaut sans persistance avec une Factory Par défaut
        /// </summary>
        /// <remarks>Cette version est utilisé pour les tests, ne charge ni sauvegarde aucune donnée</remarks>
        public Manager()
        {
            Users = new ReadOnlyCollection<User>(_users);
            Oeuvres = new ReadOnlyCollection<Oeuvre>(_oeuvres);
            Personnes = new ReadOnlyCollection<Personne>(_personnes);
            _factory = new ManagerFactory();
        }


        /// <summary>
        /// Permet d’initialiser les ReadOnlyCollection, la persistance et la factory en remplissant les listes
        /// </summary>
        /// <param name="persistance">La méthode de persistance choisi par l'utilisateur</param>
        /// <param name="factory">La méthode de Factory choisi par l'utilisateur</param>
        public Manager(IPersistance persistance, IFactory factory)
        {
            Users = new ReadOnlyCollection<User>(_users);
            Oeuvres = new ReadOnlyCollection<Oeuvre>(_oeuvres);
            Personnes = new ReadOnlyCollection<Personne>(_personnes);
            _factory = factory;

            if (persistance is null) return;

            _persistance = persistance;
            var (personnes, oeuvres, users) = _persistance.Charger();
            _personnes.AddRange(personnes);
            _oeuvres.AddRange(oeuvres);
            _users.AddRange(users);
        }

        /// <summary>
        /// Appelle le constructeur précédant en lui envoyant sa Factory par défaut (ManagerFactory)
        /// </summary>
        /// <param name="persistance">La méthode de persistance choisi par l'utilisateur</param>
        /// <seealso cref="ManagerFactory"/>
        /// <seealso cref="IPersistance"/>
        public Manager(IPersistance persistance) : this(persistance, new ManagerFactory()) { }

        public void Sauvegarder() => _persistance.Sauvegarder(_personnes, _oeuvres, _users);

        /// <summary>
        /// Permet de rendre tous les Themes sous format de string
        /// </summary>
        /// <returns>Rend tous les Themes dans une liste</returns>
        public static IEnumerable<string> RendreTousThemes() =>
            Enum.GetNames(typeof(Themes)).Where(th => !th.Equals("Inconnu"));

        public static IEnumerable<string> RendreToutesPlateformes() =>
            Enum.GetNames(typeof(Plateformes)).Where(pl => !pl.Equals("Inconnu"));

        /// <summary>
        /// Permet de rendre toutes les Oeuvres
        /// </summary>
        /// <param name="user">Le User qui a demandé la liste</param>
        /// <returns>Rend la liste d'Oeuvre</returns>
        /// <seealso cref="Oeuvre"/>
        /// <seealso cref="User"/>
        public IEnumerable<Oeuvre> RendreListeOeuvres(User user = null)
        {
            return user is not {IsModeFamille: true}
                ? _oeuvres.Where(o => o is not Episode).OrderBy(o => o.Titre).ThenBy(o => o.DateDeSortie)
                : _oeuvres.Where(o => o is not Episode && o.IsFamilleF).OrderBy(o => o.Titre)
                    .ThenBy(o => o.DateDeSortie);
        }

        /// <summary>
        /// Permet d'obtenir les 10 Oeuvre avec les meilleurs note
        /// </summary>
        /// <param name="user">Le User qui a demandé la liste</param>
        /// <returns>Rend la liste des meilleurs Oeuvre</returns>
        /// <seealso cref="Oeuvre"/>
        /// <seealso cref="User"/>
        public IEnumerable<Oeuvre> RendreListePopulaire(User user = null)
        {
            return user is not {IsModeFamille: true}
                ? _oeuvres.Where(o => o is not Episode).OrderByDescending(o => o.NoteMoyenne).Take(10)
                : _oeuvres.Where(o => o is not Episode && o.IsFamilleF).OrderByDescending(o => o.NoteMoyenne).Take(10);
        }

        /// <summary>
        /// Permet d'obtenir toutes les Oeuvre d'un Themes en particulier
        /// </summary>
        /// <param name="theme">Le Themes choisit</param>
        /// <param name="user">Le User qui a demandé la liste</param>
        /// <returns>Rend la liste des Oeuvre en fonction du Themes fournit</returns>
        /// <seealso cref="Oeuvre"/>
        /// <seealso cref="User"/>
        public IEnumerable<Oeuvre> RendreListeOeuvresTheme(Themes theme, User user = null)
        {
            return user is not {IsModeFamille: true}
                ? _oeuvres.Where(o => o is not Episode && o.Theme == theme).OrderBy(o => o.Titre)
                    .ThenBy(o => o.DateDeSortie)
                : _oeuvres.Where(o => o is not Episode && o.IsFamilleF && o.Theme == theme).OrderBy(o => o.Titre)
                    .ThenBy(o => o.DateDeSortie);
        }

        /// <summary>
        /// Permet d'obtenir toutes les Oeuvres correspondant à notre recherche
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pattern">La recherche effectué</param>
        /// <param name="user">Le User qui a demandé la liste</param>
        /// <returns>Rend la liste des Oeuvre correspondant à la rechercher</returns>
        /// <seealso cref="Oeuvre"/>
        /// <seealso cref="User"/>
        public IEnumerable<Oeuvre> RendreListeOeuvresSearch(string type, string pattern, User user = null)
        {
            pattern ??= "";
            return type switch
            {
                FILM when user is not {IsModeFamille: true} => _oeuvres
                    .Where(o => o is Film && o.Titre.StartsWith(pattern)).OrderBy(o => o.Titre)
                    .ThenBy(o => o.DateDeSortie),

                FILM => _oeuvres.Where(o => o is Film && o.IsFamilleF && o.Titre.StartsWith(pattern))
                    .OrderBy(o => o.Titre).ThenBy(o => o.DateDeSortie),

                EPISODE when user is not {IsModeFamille: true} => _oeuvres
                .Where(o => o is Episode && o.Titre.StartsWith(pattern)).OrderBy(o => o.Titre)
                .ThenBy(o => o.DateDeSortie),

                EPISODE => _oeuvres.Where(o => o is Episode && o.IsFamilleF && o.Titre.StartsWith(pattern))
                    .OrderBy(o => o.Titre).ThenBy(o => o.DateDeSortie),

                TRILOGIE when user is not {IsModeFamille: true} => _oeuvres.Where(o =>
                    o is Trilogie && o.Titre.StartsWith(pattern)).OrderBy(o => o.Titre).ThenBy(o => o.DateDeSortie),

                TRILOGIE => _oeuvres.Where(o => o is Trilogie && o.IsFamilleF && o.Titre.StartsWith(pattern))
                    .OrderBy(o => o.Titre).ThenBy(o => o.DateDeSortie),

                SERIE when user is not {IsModeFamille: true} => _oeuvres.Where(o =>
                    o is Serie && o.Titre.StartsWith(pattern)).OrderBy(o => o.Titre).ThenBy(o => o.DateDeSortie),

                SERIE => _oeuvres.Where(o => o is Serie && o.IsFamilleF && o.Titre.StartsWith(pattern))
                    .OrderBy(o => o.Titre).ThenBy(o => o.DateDeSortie),

                UNIVERS when user is not {IsModeFamille: true} => _oeuvres.Where(o =>
                    o is Univers && o.Titre.StartsWith(pattern)).OrderBy(o => o.Titre).ThenBy(o => o.DateDeSortie),

                UNIVERS => _oeuvres.Where(o => o is Univers && o.IsFamilleF && o.Titre.StartsWith(pattern))
                    .OrderBy(o => o.Titre).ThenBy(o => o.DateDeSortie),

                OEUVRE when user is not {IsModeFamille: true} => _oeuvres.Where(o =>
                    o is not Episode && o.Titre.StartsWith(pattern)).OrderBy(o => o.Titre).ThenBy(o => o.DateDeSortie),

                OEUVRE => _oeuvres.Where(o => o is not Episode && o.IsFamilleF && o.Titre.StartsWith(pattern))
                    .OrderBy(o => o.Titre).ThenBy(o => o.DateDeSortie),

                _ => _oeuvres.Where(o => o is not Episode or Univers && o.Titre.StartsWith(pattern))
                    .OrderBy(o => o.Titre).ThenBy(o => o.DateDeSortie)
            };
        }

        /// <summary>
        /// Permet d'obtenir la liste de tous les acteurs
        /// </summary>
        /// <returns>Rend la liste de tous les acteurs</returns>
        /// <seealso cref="Personne"/>
        public static IEnumerable<Oeuvre> RendreListeOeuvresActeur(User user, Personne pers)
        {
            if (pers is null) return null;

            return user is not {IsModeFamille: true}
                ? pers.Oeuvres.OrderBy(o => o.DateDeSortie)
                : pers.Oeuvres.Where(o => o.IsFamilleF).OrderBy(o => o.DateDeSortie);
        }


        /// <summary>
        /// Permet d'obtenir la liste de tous les acteurs
        /// </summary>
        /// <returns>Rend la liste de tous les acteurs</returns>
        /// <seealso cref="Personne"/>
        public IEnumerable<Personne> RendreListeActeurs() => _oeuvres.SelectMany(o => o.Personnes.Values.SelectMany(pers => pers.Keys))
            .OrderBy(p => p.Prenom).ThenBy(p => p.Nom).ThenBy(p => p.DateDeNaissance).Distinct();


        /// <summary>
        /// Permet d'obtenir tous les acteurs correspondant à notre recherche
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pattern1">Le premier mot de recherche</param>
        /// <param name="pattern2">Le second mot de recherche</param>
        /// <returns>Rend la liste des acteurs correspondant à notre recherche</returns>
        /// <seealso cref="Personne"/>
        public IEnumerable<Personne> RendreListeActeursSearch(string type, string pattern1, string pattern2 = null)
        {
            pattern1 ??= "";
            if (pattern2 == null)
                return _oeuvres.SelectMany(o =>
                        o.Personnes[type].Keys
                            .Where(p => p.Prenom.StartsWith(pattern1) || p.Nom.StartsWith(pattern1)))
                    .OrderBy(p => p.Prenom).ThenBy(p => p.Nom).ThenBy(p => p.DateDeNaissance).Distinct();

            return _oeuvres.SelectMany(o => o.Personnes[type].Keys.Where(p =>
                    (p.Prenom.StartsWith(pattern1) || p.Nom.StartsWith(pattern1)) &&
                    (p.Prenom.StartsWith(pattern2) || p.Nom.StartsWith(pattern2))))
                .OrderBy(p => p.Prenom).ThenBy(p => p.Nom)
                .ThenBy(p => p.DateDeNaissance).Distinct();
        }

        /// <summary>
        /// Permet d'obtenir la liste de tous les avis d'un User
        /// </summary>
        /// <param name="user">Le User qui veut sa liste d'Avis</param>
        /// <param name="nbA">Le nombre d'avis que le User à laissé</param>
        /// <returns>Rend un dictionnaire ayant pour clef l'Oeuvre auquel l'Avis est rattaché et en valeur l'Avis</returns>
        /// <seealso cref="User"/>
        /// <seealso cref="Avis"/>
        /// <seealso cref="Oeuvre"/>
        public IEnumerable<KeyValuePair<Oeuvre, Avis>> RendreListeAvis(User user, out int nbA)
        {
            if (user is null)
            {
                nbA = 0;
                return null;
            }

            var listeA = _oeuvres
                .Where(oe => oe.ListeAvis.ToDictionary(pair => pair.Key, pair => pair.Value).ContainsKey(user))
                .ToDictionary(oe => oe, oe => oe.ListeAvis.ToDictionary(pair => pair.Key, pair => pair.Value)[user]);

            nbA = listeA.Count;
            return listeA;
        }
    }
}