using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using DataEncryption;
using Newtonsoft.Json.Linq;
using Modele;
using Newtonsoft.Json;
using static Modele.Constante;
using static LINQ_to_Data.ConstanteData;

namespace LINQ_to_Data
{
    /// <summary>
    /// Stock les données dans un fichier JSON
    /// </summary>
    public class LinqToJson : IPersistance
    {
        public string FolderPath { get; } = Path.Combine(Directory.GetCurrentDirectory(), DEFAULT_JSON_FOLDER);

        public string FileName { get; } = DEFAULT_JSON_FILENAME;

        private string FilePath => Path.Combine(FolderPath, FileName);

        private readonly IFactory _factory = new LoadingFactory();

        private readonly IEncryption _encryption;

        /// <summary>
        /// Constructeur par défaut de LinqToJson
        /// </summary>
        public LinqToJson() { }

        /// <summary>
        /// Permet d'ajouter une méthode d'encryption
        /// </summary>
        /// <param name="encryption">la méthode d'encryption</param>
        public LinqToJson(IEncryption encryption)
        {
            _encryption = encryption;
            _encryption.FolderPath = FolderPath;
        }

        /// <summary>
        /// Permet de fixer des emplacement différent pour le fichier
        /// </summary>
        /// <param name="fileName">Le nom du fichier</param>
        /// <param name="folderPath">Le nom  du dossier</param>
        public LinqToJson(string fileName, string folderPath)
        {
            FileName = fileName;
            FolderPath = folderPath;
        }

        /// <summary>
        /// Permet de configurer entièrement LinqToJson en faisant appelle a son constructeur précédant et en ajoutant une encryption
        /// </summary>
        /// <param name="encryption">La méthode d'encryption</param>
        /// <param name="fileName">Le nom du fichier</param>
        /// <param name="folderPath">Le nom du dossier</param>
        public LinqToJson(IEncryption encryption, string fileName, string folderPath) : this(fileName, folderPath)
        {
            _encryption = encryption;
            _encryption.FolderPath = FolderPath;
        }

        /// <summary>
        /// Permet de renvoyer une date en fonction de la valeur
        /// </summary>
        /// <param name="value">la valeur de la date</param>
        /// <returns>Retourne la date correspondant à la valeur s'il a réussi a la crée sinon rend la valeur minimum d'une date</returns>
        private static DateTime DateIfNull(string value) =>
            DateTime.TryParse(value, CultureInfo.CurrentCulture, DateTimeStyles.AdjustToUniversal, out var date)
                ? date
                : DateTime.MinValue;

        /// <summary>
        /// Permet de rendre un booléen en fonction de la valeur
        /// </summary>
        /// <param name="value">La  valeur du booléen</param>
        /// <returns>Retourne le booléen correspondant a la valeur s'il réussi à le créer si non rend false</returns>
        private static bool BoolIfNull(string value) => bool.TryParse(value, out var result) && result;

        /// <summary>
        /// Permet de rendre un Theme en fonction de la valeur
        /// </summary>
        /// <param name="value">la valeur du theme</param>
        /// <returns>Rend le theme correspondant à la valeur s'il a réussi ç le créer si non rend le theme Inconnu</returns>
        /// <seealso cref="Themes"/>
        private static Themes ThemesIfNull(string value) =>
            Enum.TryParse<Themes>(value, out var theme) ? theme : Themes.Inconnu;

        /// <summary>
        /// Permet de rendre la plateforme en fonction de la valeur
        /// </summary>
        /// <param name="value">la valeur de la plateforme</param>
        /// <returns>Retourne la plateforme correspondant à la valeur s'il a réussi à la créer si non rend la plateforme Inconnu </returns
        /// <seealso cref="Plateformes"/>
        private static Plateformes PlateformesIfNull(string value) =>
            Enum.TryParse<Plateformes>(value, out var plat) ? plat : Plateformes.Inconnu;

        /// <summary>
        /// Permet de retrouver des vraies Personne à partir de valeurs
        /// </summary>
        /// <param name="type">Le type de¨Personne</param>
        /// <param name="tokens">Le token contenant les valeurs</param>
        /// <param name="personnes">La liste de vraies Personne</param>
        /// <returns>Retourne la liste des Personne correspondant aux éléments</returns>
        /// <seealso cref="Personne"/>
        private static IEnumerable<Personne> RetrouverPersonne(string type ,IEnumerable<JToken> tokens, IEnumerable<Personne> personnes)
        {
            var perso = tokens.Select(pers =>
                new
                {
                    Prenom = (string) pers[type]["Prénom"],
                    Nom = (string) pers[type]["Nom"],
                    Date = DateIfNull((string) pers[type]["Date"]),
                });

            return perso.Select(real => personnes.SingleOrDefault(persReal =>
                persReal.Prenom.Equals(real.Prenom) &&
                persReal.Nom.Equals(real.Nom) &&
                persReal.DateDeNaissance.Equals(real.Date)));
        }

        /// <summary>
        /// Permet de rendre le dictionnaire correspondant au vraies Personne
        /// </summary>
        /// <param name="token">Le token contenant les valeurs</param>
        /// <param name="personnes">La liste des vraies Personne</param>
        /// <returns>Retourne le dictionnaire de Personne (acteurs / réalisateurs) d'une Oeuvre</returns>
        /// <seealso cref="Personne"/>
        /// <seealso cref="Oeuvre"/>
        private static IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<Personne, string>>>>
            CreateListePersonnes(JToken token, IEnumerable<Personne> personnes)
        {
            var personnesList = personnes.ToList();

            var acteurs = token["Acteurs"].ToDictionary(key =>
                    new
                    {
                        Prenom = (string) key[ACTEUR]["Prénom"],
                        Nom = (string) key[ACTEUR]["Nom"],
                        Date = DateIfNull((string) key[ACTEUR]["Date"]),
                    }
                , value => (string) value[ACTEUR]["Rôle"]);

            var pers = new Dictionary<string, Dictionary<Personne, string>> { [REALISATEUR] = new(), [ACTEUR] = new() };

            foreach (var persReal in RetrouverPersonne(REALISATEUR, token["Réalisateurs"].Where(
                tkReal => tkReal[REALISATEUR] is not null), personnesList)) pers[REALISATEUR][persReal] = "";

            foreach (var (persAct, role) in acteurs.ToDictionary(act =>
                    personnesList.Single(perdAct => perdAct.Prenom.Equals(act.Key.Prenom) &&
                                                    perdAct.Nom.Equals(act.Key.Nom) &&
                                                    perdAct.DateDeNaissance.Equals(act.Key.Date)),
                act => act.Value)
            ) pers[ACTEUR][persAct] = role;

            return pers.ToDictionary(pair => pair.Key, pair => pair.Value.ToList() as IEnumerable<KeyValuePair<Personne, string>>);
        }

        /// <summary>
        /// Permet de créer une Liste d'avis à partir de valeurs
        /// </summary>
        /// <param name="token">Le token contenant les valeurs</param>
        /// <param name="users">La liste des User</param>
        /// <returns>Retourne une liste de KeyValuePaie User, Avis</returns>
        /// <seealso cref="User"/>
        /// <seealso cref="Avis"/>
        private IEnumerable<KeyValuePair<User, Avis>> CreateListeAvis(JToken token, IEnumerable<User> users)
        {
            return token.ToDictionary(av =>
                users.SingleOrDefault(us => us.Pseudo.Equals((string) av["Avis"]?["User"])), av =>
                _factory.CreerAvis(
                    (float) av["Avis"]["Note"],
                    (string) av["Avis"]["Commentaire"]));
        }

        /// <summary>
        /// Permet de créer des Leafs en fonctions des valeurs
        /// </summary>
        /// <param name="type">Le type de Leaf</param>
        /// <param name="token">Le token contenant les valeurs</param>
        /// <param name="personnes">La liste des vraies Personne</param>
        /// <param name="users">La liste de vraies User</param>
        /// <returns>Une liste de Leaf</returns>
        /// <seealso cref="Leaf"/>
        /// <seealso cref="Personne"/>
        /// <seealso cref="User"/>
        private IEnumerable<Leaf> CreerLeaf(string type, JToken token, IEnumerable<Personne> personnes,
            IEnumerable<User> users)
        {
            return token.Select(tkLeaf =>
                _factory.CreerLeaf(type,
                    (string) tkLeaf[type]["Titre"], 
                    DateIfNull((string) tkLeaf[type]["Date"]),
                    (string) tkLeaf[type]["Image"],
                    (string)tkLeaf[type]["Synopsis"],
                    ThemesIfNull((string) tkLeaf[type]["Thème"]),
                    BoolIfNull((string) tkLeaf[type]["IsFamille"]), 
                    CreateListePersonnes(tkLeaf[type], personnes),
                    _factory.AjouterStreamLeaf(
                        (string)tkLeaf[type]["Titre"],
                        tkLeaf[type]["Streamings"]?.ToDictionary(
                            pair => PlateformesIfNull((string)pair["Stream"]["Plateforme"]),
                            pair => (string)pair["Stream"]["Lien"])),
                    CreateListeAvis(tkLeaf[type]["Avis"], users)
                ));
        }

        /// <summary>
        /// Permet de remplir la liste d'Oeuvre à partir de valeurs
        /// </summary>
        /// <param name="tokens">La liste de tokens contenants les valeurs</param>
        /// <param name="oeuvres">La liste de vraies Oeuvre</param>
        /// <seealso cref="Oeuvre"/>
        /// <returns>Retourne la liste d'Oeuvre</returns>
        private static IEnumerable<Oeuvre> SearchAndFillListOeuvres(string type, IEnumerable<JToken> tokens, IEnumerable<Oeuvre> oeuvres)
        {
            var oeTokens = tokens.Select(tkOeuvre => new
            {
                Titre = (string) tkOeuvre[type]?["Titre"],
                Date = DateIfNull((string) tkOeuvre[type]["Date"]),
            });

            return oeTokens.Select(oe =>
                oeuvres.SingleOrDefault(oeuvre =>
                    oeuvre.Titre.Equals(oe.Titre) && oeuvre.DateDeSortie.Equals(oe.Date)));
        }

        /// <summary>
        /// Permet de Créer la liste complète d'Oeuvre correspondant à des valeurs
        /// </summary>
        /// <param name="token">Le token contenant les valeurs</param>
        /// <param name="oeuvres">La liste d'Oeuvre</param>
        /// <returns>La liste complète des Oeuvre</returns>
        /// <seealso cref="Oeuvre"/>
        private static IEnumerable<Oeuvre> CreateListOeuvres(JToken token, IEnumerable<Oeuvre> oeuvres)
        {
           var oeuvresList = oeuvres.ToList();
           var oes = new List<Oeuvre>();

           oes.AddRange(SearchAndFillListOeuvres(FILM, token.Where(tkFilm => tkFilm[FILM] is not null),
               oeuvresList.Where(oe => oe is Film)));

           oes.AddRange(SearchAndFillListOeuvres(EPISODE, token.Where(tkFilm => tkFilm[EPISODE] is not null),
               oeuvresList.Where(oe => oe is Episode)));

           oes.AddRange(SearchAndFillListOeuvres(TRILOGIE, token.Where(tkFilm => tkFilm[TRILOGIE] is not null),
               oeuvresList.Where(oe => oe is Trilogie)));

           oes.AddRange(SearchAndFillListOeuvres(SERIE, token.Where(tkFilm => tkFilm[SERIE] is not null),
               oeuvresList.Where(oe => oe is Serie)));

           oes.AddRange(SearchAndFillListOeuvres(UNIVERS, token.Where(tkFilm => tkFilm[UNIVERS] is not null),
               oeuvresList.Where(oe => oe is Univers)));
           
           return oes;
        }

        /// <summary>
        /// Permet de créer des Composite à partir de valeurs
        /// </summary>
        /// <param name="type">Le type de Composite</param>
        /// <param name="token">Le token contenant les valeurs</param>
        /// <param name="personnes">La liste des vraies Personne</param>
        /// <param name="oeuvres">La liste des vraies Oeuvre</param>
        /// <param name="users">La liste des vraies User </param>
        /// <returns>Une liste de Composite</returns>
        /// <seealso cref="Composite"/>
        /// <seealso cref="Oeuvre"/>
        /// <seealso cref="Personne"/>
        /// <seealso cref="User"/>
        private IEnumerable<Composite> CreeComposite(string type, JToken token,
            IEnumerable<Personne> personnes, IEnumerable<Oeuvre> oeuvres, IEnumerable<User> users)
        {
            return token.Select(tkComp =>
                _factory.CreerComposite(type,
                    (string) tkComp[type]["Titre"],
                    DateIfNull((string) tkComp[type]["Date"]),
                    (string) tkComp[type]["Image"],
                    (string)tkComp[type]["Synopsis"],
                    ThemesIfNull((string) tkComp[type]["Thème"]),
                    BoolIfNull((string) tkComp[type]["IsFamille"]), 
                    CreateListOeuvres(tkComp[type][type switch
                    {
                        TRILOGIE => "Films",
                        SERIE => "Épisodes",
                        _ => "Oeuvres"
                    }], 
                        oeuvres),
                    CreateListePersonnes(tkComp[type], personnes),
                    CreateListeAvis(tkComp[type]["Avis"], users)
                ));
        }

        /// <summary>
        /// Permet de charger les données depuis un fichier JSON
        /// </summary>
        /// <returns>Une liste de Personne, d'Oeuvre et de User</returns>
        /// <seealso cref="User"/>
        /// <seealso cref="Personne"/>
        /// <seealso cref="Oeuvre"/>
        public (IEnumerable<Personne> personnes, IEnumerable<Oeuvre> oeuvres, IEnumerable<User> users) Charger()
        {
            if (!File.Exists(FilePath)) return (new List<Personne>(), new List<Oeuvre>(), new List<User>());

            JObject file;
            string text;
            if (_encryption is not null)
            {
                text = _encryption.Decrypter(FileName) as string;
                if (text is null) return (new List<Personne>(), new List<Oeuvre>(), new List<User>());
            }

            else
                text = File.ReadAllText(FilePath);

            try { file = JObject.Parse(text); }

            catch (JsonReaderException) { return (new List<Personne>(), new List<Oeuvre>(), new List<User>()); }

                var users = file["Data"]?[0]?["Users"]?.Select(tkUs =>
                _factory.CreerUser(
                    (string) tkUs["User"]?["Pseudo"],
                    (string) tkUs["User"]?["Password"],
                    (string) tkUs["User"]?["Image"],
                    BoolIfNull((string) tkUs["User"]?["IsModeFamille"]),
                    BoolIfNull((string) tkUs["User"]?["IsAdmin"]),
                     tkUs["User"]?["Plateformes"]?.Select(plat =>
                        PlateformesIfNull((string)plat["Plateforme"])) ?? new List<Plateformes>()
                )).ToList();

            var personnes = file["Data"]?[1]?["Personnes"]?.Select(tkPers => 
                _factory.CreerPersonne(
                    (string) tkPers["Personne"]["Prénom"],
                    (string) tkPers["Personne"]["Nom"],
                    (string) tkPers["Personne"]["Biographie"],
                    (string) tkPers["Personne"]["Nationalité"],
                    (string) tkPers["Personne"]["Image"],
                   DateIfNull((string) tkPers["Personne"]["Date"])
                )).ToList();

            var films = CreerLeaf(FILM, file["Data"]?[2]?["Oeuvres"]?[0]?["Leafs"]?[0]?["Films"], personnes, users).ToList();
            var episodes = CreerLeaf(EPISODE, file["Data"]?[2]?["Oeuvres"]?[0]?["Leafs"]?[1]?["Épisodes"], personnes, users).ToList();

            var trilogies = CreeComposite(TRILOGIE, file["Data"]?[2]?["Oeuvres"]?[1]?["Composites"]?[0]?["Trilogies"], personnes,
                films, users).ToList();

            var series = CreeComposite(SERIE, file["Data"]?[2]?["Oeuvres"]?[1]?["Composites"]?[1]?["Séries"], personnes,
                episodes, users).ToList();

            var univers = CreeComposite(UNIVERS, file["Data"]?[2]?["Oeuvres"]?[1]?["Composites"]?[2]?["Univers"], personnes,
                new List<Oeuvre>().Union(films).Union(trilogies).Union(series), users).ToList();

            var oeuvres = new List<Oeuvre>().Union(films).Union(episodes).Union(trilogies).Union(series).Union(univers).ToList();

            foreach (var tkUs in file["Data"]?[0]?["Users"]!)
            {
                _factory.ModifierLiseUser(
                    users.SingleOrDefault(us => us.Pseudo.Equals((string) tkUs["User"]?["Pseudo"])),
                    CreateListOeuvres(tkUs["User"]?["Envies"], new List<Oeuvre>().Union(films).Union(series)),
                    new List<object>().Union(RetrouverPersonne("Personne",tkUs["User"]["RécemmentConsultées"]?.Where(
                            tkPers => tkPers["Personne"] is not null), personnes)).Union(
                        CreateListOeuvres(tkUs["User"]["RécemmentConsultées"], oeuvres)));
            }

            return (personnes, oeuvres, users);
        }
        
        /// <summary>
        /// Permet de sauvegarder des données dans un fichier JSON
        /// </summary>
        /// <param name="personnes">La liste de Personne</param>
        /// <param name="oeuvres">La liste d'Oeuvre</param>
        /// <param name="users">La liste de User</param>
        /// <seealso cref="User"/>
        /// <seealso cref="Personne"/>
        /// <seealso cref="Oeuvre"/>
        public void Sauvegarder(IEnumerable<Personne> personnes, IEnumerable<Oeuvre> oeuvres, IEnumerable<User> users)
        {
            var usersElts = users.Select(us => new JObject(
                new JProperty("User", new JObject(
                    new JProperty("Pseudo", us.Pseudo),
                    new JProperty("IsModeFamille", us.IsModeFamille),
                    new JProperty("IsAdmin", us.IsAdmin),
                    new JProperty("Password", us.Mdp),
                    new JProperty("Image", us.ImageProfil),
                    us.ListeEnvie.Count != LISTE_MIN 
                        ? new JProperty("Envies",
                                us.ListeEnvie.Select(oe =>
                                    new JObject(new JProperty(oe switch
                                        {
                                            Film => FILM,
                                            Episode => EPISODE,
                                            Trilogie => TRILOGIE,
                                            Serie => SERIE,
                                            _ => UNIVERS
                                        }, 
                                        new JObject(
                                            new JProperty("Titre", oe.Titre), 
                                            new JProperty("Date", oe.DateDeSortie.ToString(CultureInfo.CurrentCulture)))
                                        ))
                                ))
                        : new JProperty("Envies"),
                    us.RecemmentConsulte.Count != LISTE_MIN 
                        ? new JProperty("RécemmentConsultées",
                            us.RecemmentConsulte.Select(cons =>
                                cons switch
                                {
                                    Oeuvre oe => new JObject(new JProperty(oe switch
                                    {
                                        Film => FILM,
                                        Episode => EPISODE,
                                        Trilogie => TRILOGIE,
                                        Serie => SERIE,
                                        _ => UNIVERS
                                    },
                                        new JObject(
                                            new JProperty("Titre", oe.Titre), 
                                            new JProperty("Date", oe.DateDeSortie)))),

                                    Personne pers => new JObject(new JProperty("Personne", 
                                        new JObject(
                                            new JProperty("Prénom", pers.Prenom),
                                            new JProperty("Nom", pers.Nom),
                                            new JProperty("Date", pers.DateDeNaissance.ToString(CultureInfo.CurrentCulture)))
                                        )),

                                    _ => null
                                }))
                        : new JProperty("RécemmentConsultées"),

                    us.ListePlateformes.Count != LISTE_MIN 
                        ? new  JProperty("Plateformes",
                            us.ListePlateformes.Select(plat =>
                                new JObject(new JProperty("Plateforme", plat.ToString()))
                            ))
                        : new JProperty("")
                ))));
            var personnesElts = personnes.Select(pers =>  new JObject(
                new JProperty("Personne", new JObject(
                    new JProperty("Prénom", pers.Prenom),
                    new JProperty("Nom", pers.Nom),
                    new JProperty("Nationalité", pers.Nationalite),
                    new JProperty("Date", pers.DateDeNaissance.ToString(CultureInfo.CurrentCulture)), 
                    new JProperty("Image", pers.LienImage),
                    new JProperty("Biographie", pers.Biographie)))
                ));

            var oeuvresElts = oeuvres.Select(oe => new JObject(
                new JProperty(oe switch
                {
                    Film => FILM,
                    Episode => EPISODE,
                    Trilogie => TRILOGIE,
                    Serie => SERIE,
                    _ => UNIVERS
                },
                    new JObject(
                        new JProperty("Titre", oe.Titre), 
                        new JProperty("Date", oe.DateDeSortie.ToString(CultureInfo.CurrentCulture)), 
                        new JProperty("Thème", oe.Theme.ToString()), 
                        new JProperty("IsFamille", oe.IsFamilleF), 
                        new JProperty("Image", oe.LienImage), 
                        new JProperty("Synopsis", oe.Synopsis),
                        
                        new JProperty("Réalisateurs",
                        oe.Personnes[REALISATEUR].Keys.Select(real =>
                            new JObject(new JProperty(REALISATEUR,
                                new JObject(
                                    new JProperty("Prénom", real.Prenom), 
                                    new JProperty("Nom", real.Nom), 
                                    new JProperty("Date", real.DateDeNaissance.ToString(CultureInfo.CurrentCulture)))
                                ))
                        )),

                        new JProperty("Acteurs",
                        oe.Personnes[ACTEUR].Select(act =>
                            new JObject(new JProperty(ACTEUR,
                                new JObject(
                                    new JProperty("Prénom", act.Key.Prenom),
                                    new JProperty("Nom", act.Key.Nom), 
                                    new JProperty("Date", act.Key.DateDeNaissance.ToString(CultureInfo.CurrentCulture)), 
                                    new JProperty("Rôle", act.Value))))
                        )),
                        
                        (oe switch 
                        {
                            Composite comp => comp.ReOeuvres.Count != LISTE_MIN 
                                ? new JProperty(comp switch 
                                    {
                                        Trilogie => "Films",
                                        Serie => "Épisodes", 
                                        _ => "Oeuvres"

                                    }, 
                                    comp.ReOeuvres.Select(compo =>
                                        new JObject(new JProperty(compo switch 
                                            {
                                                Film => FILM, 
                                                Episode => EPISODE, 
                                                Trilogie => TRILOGIE,
                                                _ => SERIE

                                            },
                                            new JObject(
                                                new JProperty("Titre", compo.Titre),
                                                new JProperty("Date", compo.DateDeSortie.ToString(CultureInfo.CurrentCulture))))
                                    )))

                                :  new JProperty(comp switch 
                                    {
                                        Trilogie => "Films",
                                        Serie => "Épisodes", 
                                        _ => "Oeuvres"

                                    }),

                            Leaf leaf => leaf.ListeStream.Count != LISTE_MIN
                                ? new JProperty("Streamings",
                                    leaf.ListeStream.Select(st =>
                                     new JObject(new JProperty("Stream",
                                         new JObject(
                                             new JProperty("Plateforme", st.Plateforme.ToString()),
                                             new JProperty("Lien", st.Lien))))
                                    ))

                                : new JProperty("Streamings"),

                            _ => null
                        })!,

                        (oe.ListeAvis.Count > LISTE_MIN
                            ? new JProperty("Avis",
                                oe.ListeAvis.ToDictionary(pair => pair.Key, pair => pair.Value).Select(avis =>
                                    new JObject(new JProperty("Avis", 
                                        new JObject(
                                            new JProperty("User", avis.Key.Pseudo), 
                                            new JProperty("Note", avis.Value.Note), 
                                            new JProperty("Commentaire", avis.Value.Commentaire))))
                                ))
                            
                            : new JProperty("Avis"))!
                        )))).ToList();

            var file = new JObject(new JProperty("Data",
                new JObject(new JProperty("Users", usersElts)),
                new JObject(new JProperty("Personnes", personnesElts)),

                new JObject(new JProperty("Oeuvres",
                    new JObject(new JProperty("Leafs",
                        new JObject(new JProperty("Films", oeuvresElts.Where(le => le.First!.Path.Equals(FILM)))),
                        new JObject(new JProperty("Épisodes", oeuvresElts.Where(le => le.First!.Path.Equals(EPISODE)))))),
                    
                    new JObject(new JProperty("Composites",
                        new JObject(new JProperty("Trilogies", oeuvresElts.Where(le => le.First!.Path.Equals(TRILOGIE)))),
                        new JObject(new JProperty("Séries", oeuvresElts.Where(le => le.First!.Path.Equals(SERIE)))),
                        new JObject(new JProperty("Univers", oeuvresElts.Where(le => le.First!.Path.Equals(UNIVERS))))))
                    ))));

            if (!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);

            if (_encryption is not null)
                _encryption.Encrypter(file.ToString(), FileName);

            else
                File.WriteAllText(FilePath, file.ToString());
        }
    }
}