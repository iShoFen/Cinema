using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using DataEncryption;
using Modele;
using static Modele.Constante;
using static LINQ_to_Data.ConstanteData;

namespace LINQ_to_Data
{
    /// <summary>
    /// Permet de sauvegarder les données dans un fichier XML
    /// </summary>
    public class LinqToXml : IPersistance
    {
        public string FolderPath { get; } = Path.Combine(Directory.GetCurrentDirectory(), DEFAULT_XML_FOLDER);

        public string FileName { get; } = DEFAULT_XML_FILENAME;

        private string FilePath => Path.Combine(FolderPath, FileName);

        private readonly IFactory _factory = new LoadingFactory();

        private readonly IEncryption _encryption;

        /// <summary>
        /// Constructeur par défaut de LinqToXml
        /// </summary>
        public LinqToXml() { }

        /// <summary>
        /// Permet de garder les paramètres de bases tout en ajoutant une encryption
        /// </summary>
        /// <param name="encryption">La méthode d'encryption</param>
        public LinqToXml(IEncryption encryption)
        {
            _encryption = encryption;
            _encryption.FolderPath = FolderPath;
        }

        
        /// <summary>
        /// Permet de fixer des emplacement différent pour le fichier
        /// </summary>
        /// <param name="fileName">Le nom du fichier</param>
        /// <param name="folderPath">Le nom  du dossier</param>
        public LinqToXml(string fileName, string folderPath)
        {
            FileName = fileName;
            FolderPath = folderPath;
        }

        /// <summary>
        /// Permet de configurer entièrement LinqToXml en faisant appelle a son constructeur précédant et en ajoutant une encryption
        /// </summary>
        /// <param name="encryption">La méthode d'encryption</param>
        /// <param name="fileName">Le nom du fichier</param>
        /// <param name="folderPath">Le nom du dossier</param>
        public LinqToXml(IEncryption encryption, string fileName, string folderPath) : this(fileName, folderPath)
        {
            _encryption = encryption;
            _encryption.FolderPath = FolderPath;
        }

        /// <summary>
        /// Permet de renvoyer un XElement ou nul en fonction de la valeur
        /// </summary>
        /// <param name="name">Le nom de l'élément</param>
        /// <param name="value">La valeur de l'élément</param>
        /// <returns>Renvoie null si la value est null si non crée un nouvel XElement</returns>
        private static XElement XElementIfNull(string name, string value) =>
            string.IsNullOrEmpty(value) ? null : new XElement(name, value);


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
        private static bool BollIfNull(string value) => bool.TryParse(value, out var result) && result;


        /// <summary>
        /// Permet de rendre un Theme en fonction de la valeur
        /// </summary>
        /// <param name="value">la valeur du theme</param>
        /// <returns>Rend le theme correspondant à la valeur s'il a réussi ç le créer si non rend le theme Inconnu</returns>
        /// <seealso cref="Themes"/>
        private static Themes ThemesIfNull(string value) => Enum.TryParse<Themes>(value, out var theme) ? theme : Themes.Inconnu;

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
        /// <param name="elements">La liste d'éléments contenants les valeurs</param>
        /// <param name="personnes">La liste de vraies Personne</param>
        /// <returns>Retourne la liste des Personne correspondant aux éléments</returns>
        /// <seealso cref="Personne"/>
        private static IEnumerable<Personne> RetrouverPersonne(IEnumerable<XElement> elements,
            IEnumerable<Personne> personnes)
        {
            var elementsList = elements.ToList();

            var perso = elementsList.Select(pers =>
                new
                {
                    Prenom = pers.Attribute("Prénom")?.Value,
                    Nom = pers.Attribute("Nom")?.Value,
                    Date = DateIfNull(pers.Attribute("Date")?.Value)
                }).ToList();

          return perso.Select(real => personnes.SingleOrDefault(persReal => 
                persReal.Prenom.Equals(real.Prenom) &&
                persReal.Nom.Equals(real.Nom) &&
                persReal.DateDeNaissance.Equals(real.Date)));
        }

        /// <summary>
        /// Permet de rendre le dictionnaire correspondant au vraies Personne
        /// </summary>
        /// <param name="elements">La liste d'éléments contenantss les valeurs</param>
        /// <param name="personnes">La liste des vraies Personne</param>
        /// <returns>Retourne le dictionnaire de Personne (acteurs / réalisateurs) d'une Oeuvre</returns>
        /// <seealso cref="Personne"/>
        /// <seealso cref="Oeuvre"/>
        private static IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<Personne, string>>>> CreateListePersonnes(
            IEnumerable<XElement> elements, IEnumerable<Personne> personnes)
        {
            var elementsList = elements.ToList();
            var personnesList = personnes.ToList();

            var acteurs = elementsList.Descendants(ACTEUR).ToDictionary(key =>
                    new
                    {
                        Prenom = key.Attribute("Prénom")?.Value,
                        Nom = key.Attribute("Nom")?.Value,
                        Date = DateIfNull(key.Attribute("Date")?.Value)
                    }
                , value => value.Attribute("Rôle")?.Value);

            var pers = new Dictionary<string, Dictionary<Personne, string>> { [REALISATEUR] = new(), [ACTEUR] = new() };

            foreach (var persReal in RetrouverPersonne(elementsList.Descendants(REALISATEUR), personnesList)) 
                if(persReal is not null) pers[REALISATEUR][persReal] = "";

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
        /// <param name="elements">La liste d'éléments contenants les valeurs</param>
        /// <param name="users">La liste des User</param>
        /// <returns>Retourne une liste de KeyValuePaie User, Avis</returns>
        /// <seealso cref="User"/>
        /// <seealso cref="Avis"/>
        private IEnumerable<KeyValuePair<User, Avis>> CreateListeAvis(IEnumerable<XElement> elements,
            IEnumerable<User> users)
        {
            return elements.ToDictionary(av =>
                users.SingleOrDefault(us => us.Pseudo.Equals(av.Attribute("User")?.Value)), av =>
                _factory.CreerAvis(
                    av.Attribute("Note")?.Value is null ? 0f : XmlConvert.ToSingle(av.Attribute("Note")?.Value!), 
                        av.Element("Commentaire")?.Value));
        }

        /// <summary>
        /// Permet de créer des Leafs en fonctions des valeurs
        /// </summary>
        /// <param name="type">Le type de Leaf</param>
        /// <param name="elements">La liste des éléments contenants les valeurs</param>
        /// <param name="personnes">La liste des vraies Personne</param>
        /// <param name="users">La liste de vraies User</param>
        /// <returns>Une liste de Leaf</returns>
        /// <seealso cref="Leaf"/>
        /// <seealso cref="Personne"/>
        /// <seealso cref="User"/>
        private IEnumerable<Leaf> CreerLeaf(string type, IEnumerable<XElement> elements, IEnumerable<Personne> personnes, IEnumerable<User> users)
        {
            return elements.Select(eltLeaf => 
                _factory.CreerLeaf(type,
                eltLeaf.Attribute("Titre")?.Value,
                DateIfNull(eltLeaf.Attribute("Date")?.Value),
                eltLeaf.Element("Image")?.Value,
                eltLeaf.Element("Synopsis")?.Value,
                ThemesIfNull(eltLeaf.Attribute("Thème")?.Value),
                BollIfNull(eltLeaf.Attribute("IsFamille")?.Value), 
                CreateListePersonnes(new List<XElement>{eltLeaf}, personnes), 
                _factory.AjouterStreamLeaf(
                    eltLeaf.Attribute("Titre")?.Value,
                    eltLeaf.Descendants("Streamings").Descendants("Stream").ToDictionary( 
                        pair =>PlateformesIfNull(pair.Attribute("Plateforme")?.Value),
                        pair => pair.Attribute("Lien")?.Value)),
                CreateListeAvis(eltLeaf.Descendants("Avis").Descendants("Avis"), users)
                ));
        }

        /// <summary>
        /// Permet de remplir la liste d'Oeuvre à partir de valeurs
        /// </summary>
        /// <param name="elements">La liste d'éléments contenants les valeurs</param>
        /// <param name="oeuvres">La liste de vraies Oeuvre</param>
        /// <seealso cref="Oeuvre"/>
        /// <returns>Retourne la liste d'Oeuvre</returns>
        private static IEnumerable<Oeuvre> SearchAndFillListOeuvres(IEnumerable<XElement> elements,
            IEnumerable<Oeuvre> oeuvres)
        {
            var oeElts = elements.Select(eltOeuvre => new
                {
                    Titre = eltOeuvre.Attribute("Titre")?.Value,
                    Date = DateIfNull(eltOeuvre.Attribute("Date")?.Value)
                });

            return oeElts.Select(oe =>
                oeuvres.SingleOrDefault(oeuvre =>
                    oeuvre.Titre.Equals(oe.Titre) && oeuvre.DateDeSortie.Equals(oe.Date)));
        }

        /// <summary>
        /// Permet de Créer la liste complète d'Oeuvre correspondant à des valeurs
        /// </summary>
        /// <param name="elements">La liste d'éléments contenants les valeurs</param>
        /// <param name="oeuvres">La liste d'Oeuvre</param>
        /// <returns>La liste complète des Oeuvre</returns>
        /// <seealso cref="Oeuvre"/>
        private static IEnumerable<Oeuvre> CreateListOeuvres(IEnumerable<XElement> elements, IEnumerable<Oeuvre> oeuvres)
        {
           var elementsList = elements.ToList();
           var oeuvresList = oeuvres.ToList();
           var oes = new List<Oeuvre>();

           oes.AddRange(SearchAndFillListOeuvres(elementsList.Descendants(FILM), oeuvresList.Where(oe => oe is Film)));
           oes.AddRange(SearchAndFillListOeuvres(elementsList.Descendants(EPISODE), oeuvresList.Where(oe => oe is Episode)));
           oes.AddRange(SearchAndFillListOeuvres(elementsList.Descendants(TRILOGIE), oeuvresList.Where(oe => oe is Trilogie)));
           oes.AddRange(SearchAndFillListOeuvres(elementsList.Descendants(SERIE), oeuvresList.Where(oe => oe is Serie)));
           oes.AddRange(SearchAndFillListOeuvres(elementsList.Descendants(UNIVERS), oeuvresList.Where(oe => oe is Univers)));

           return oes;
        }

        /// <summary>
        /// Permet de créer des Composite à partir de valeurs
        /// </summary>
        /// <param name="type">Le type de Composite</param>
        /// <param name="elements">La liste des éléments contenants les valeurs</param>
        /// <param name="personnes">La liste des vraies Personne</param>
        /// <param name="oeuvres">La liste des vraies Oeuvre</param>
        /// <param name="users">La liste des vraies User </param>
        /// <returns>Une liste de Composite</returns>
        /// <seealso cref="Composite"/>
        /// <seealso cref="Oeuvre"/>
        /// <seealso cref="Personne"/>
        /// <seealso cref="User"/>
        private IEnumerable<Composite> CreeComposite(string type, IEnumerable<XElement> elements,
            IEnumerable<Personne> personnes, IEnumerable<Oeuvre> oeuvres, IEnumerable<User> users)
        {
            return elements.Select(eltComp =>
                _factory.CreerComposite(type,
                    eltComp.Attribute("Titre")?.Value,
                    DateIfNull(eltComp.Attribute("Date")?.Value),
                    eltComp.Element("Image")?.Value,
                    eltComp.Element("Synopsis")?.Value,
                    ThemesIfNull(eltComp.Attribute("Thème")?.Value),
                    BollIfNull(eltComp.Attribute("IsFamille")?.Value),
                    CreateListOeuvres(new List<XElement> {eltComp}, oeuvres),
                    CreateListePersonnes(new List<XElement>{eltComp}, personnes),
                    CreateListeAvis(eltComp.Descendants("Avis").Descendants("Avis"), users)
                    ));
        }

        /// <summary>
        /// Permet de charger les données depuis un fichier XML
        /// </summary>
        /// <returns>Une liste de Personne, d'Oeuvre et de User</returns>
        /// <seealso cref="User"/>
        /// <seealso cref="Personne"/>
        /// <seealso cref="Oeuvre"/>
        public (IEnumerable<Personne> personnes, IEnumerable<Oeuvre> oeuvres, IEnumerable<User> users) Charger()
        {
            if (!File.Exists(FilePath)) return (new List<Personne>(), new List<Oeuvre>(), new List<User>());

            XDocument file;

            if (_encryption is not null)
            {
                file = _encryption.Decrypter(FileName) as XDocument;
                if (file is null) return (new List<Personne>(), new List<Oeuvre>(), new List<User>());
            }
            else
            {
                try {file = XDocument.Load(FilePath); }
            
                catch (Exception) { return (new List<Personne>(), new List<Oeuvre>(), new List<User>()); }
            }

            var users = file.Descendants("User").Select(us =>
                _factory.CreerUser(
                    us.Attribute("Pseudo")?.Value,
                    us.Element("Password")?.Value,
                    us.Element("Image")?.Value,
                    BollIfNull(us.Attribute("IsModeFamille")?.Value),
                    BollIfNull(us.Attribute("IsAdmin")?.Value),
                    us.Descendants("Plateforme").Select(plat => 
                        PlateformesIfNull(plat.Value))
                )).ToList();

            var personnes = file.Descendants("Personnes").Descendants("Personne")
                .Select(eltPers => _factory.CreerPersonne(
                    eltPers.Attribute("Prénom")?.Value,
                    eltPers.Attribute("Nom")?.Value,
                    eltPers.Element("Biographie")?.Value,
                    eltPers.Attribute("Nationalité")?.Value,
                    eltPers.Element("Image")?.Value ,
                    DateIfNull(eltPers.Attribute("Date")?.Value)
                    )).ToList();

            var films = CreerLeaf(FILM, file.Descendants("Leafs").Descendants("Films").Descendants(FILM), personnes, users).ToList();
            var episodes = CreerLeaf(EPISODE, file.Descendants("Leafs").Descendants("Épisodes").Descendants(EPISODE), personnes, users).ToList();

            var trilogies = CreeComposite(TRILOGIE, file.Descendants("Composites").Descendants("Trilogies").Descendants(TRILOGIE), personnes,
                films, users).ToList();

            var series = CreeComposite(SERIE, file.Descendants("Composites").Descendants("Séries").Descendants(SERIE), personnes,
                episodes, users).ToList();

            var univers = CreeComposite(UNIVERS, file.Descendants("Composites").Descendants("Univers").Descendants(UNIVERS), personnes,
                new List<Oeuvre>().Union(films).Union(trilogies).Union(series), users).ToList();

            var oeuvres = new List<Oeuvre>().Union(films).Union(episodes).Union(trilogies).Union(series).Union(univers).ToList();

            foreach (var usElt in file.Descendants("User"))
            {
                _factory.ModifierLiseUser(
                    users.SingleOrDefault(us => us.Pseudo.Equals(usElt.Attribute("Pseudo")?.Value)),
                    CreateListOeuvres(usElt.Descendants("Envies"), new List<Oeuvre>().Union(films).Union(series)),
                    new List<object>().Union(RetrouverPersonne(usElt.Descendants("Personne"), personnes))
                        .Union(CreateListOeuvres(usElt.Descendants("RécemmentConsulté"), oeuvres)));
            }

            return (
                personnes, 
                oeuvres,
                users);
        }

        /// <summary>
        /// Permet de sauvegarder des données dans un fichier XML
        /// </summary>
        /// <param name="personnes">La liste de Personne</param>
        /// <param name="oeuvres">La liste d'Oeuvre</param>
        /// <param name="users">La liste de User</param>
        /// <seealso cref="User"/>
        /// <seealso cref="Personne"/>
        /// <seealso cref="Oeuvre"/>
        public void Sauvegarder(IEnumerable<Personne> personnes, IEnumerable<Oeuvre> oeuvres, IEnumerable<User> users)
        {
            var file = new XDocument();

            var usersElts = users.Select(us =>
                new XElement("User",
                    new XAttribute("Pseudo", us.Pseudo),
                    new XAttribute("IsModeFamille", us.IsModeFamille),
                    new XAttribute("IsAdmin", us.IsAdmin),
                    new XElement("Password", us.Mdp),
                    XElementIfNull("Image", us.ImageProfil),

                    (us.ListeEnvie.Count != LISTE_MIN
                        ? new XElement("Envies",
                            us.ListeEnvie.Select(oe =>
                                new XElement(oe switch
                                {
                                    Film => FILM,
                                    Episode => EPISODE,
                                    Trilogie => TRILOGIE,
                                    Serie => SERIE,
                                    _ => UNIVERS
                                },
                                    new XAttribute("Titre", oe.Titre),
                                    new XAttribute("Date", oe.DateDeSortie))))
                        : null)!,

                    (us.RecemmentConsulte.Count != LISTE_MIN
                        ? new XElement("RécemmentConsulté",
                            us.RecemmentConsulte.Select(cons =>
                                cons switch
                                {
                                    Oeuvre oe => new XElement(oe switch
                                    {
                                        Film => FILM,
                                        Episode => EPISODE,
                                        Trilogie => TRILOGIE,
                                        Serie => SERIE,
                                        _ => UNIVERS
                                    },
                                        new XAttribute("Titre", oe.Titre),
                                        new XAttribute("Date", oe.DateDeSortie)),

                                    Personne pers => new XElement("Personne",
                                        new XAttribute("Prénom", pers.Prenom),
                                        new XAttribute("Nom", pers.Nom),
                                        new XAttribute("Date", pers.DateDeNaissance)),

                                    _ => null
                                }))
                        : null)!,

                    (us.ListePlateformes.Count != LISTE_MIN 
                        ? new XElement("Plateformes",
                            us.ListePlateformes.Select(plat =>
                                new XElement("Plateforme", plat)))
                        : null)!
                ));

            var personnesElts = personnes.Select(pers =>
                new XElement("Personne",
                    new XAttribute("Prénom", pers.Prenom),
                    new XAttribute("Nom", pers.Nom),
                    new XAttribute("Nationalité", pers.Nationalite),
                    new XAttribute("Date", pers.DateDeNaissance),
                    XElementIfNull("Image", pers.LienImage),
                    new XElement("Biographie", pers.Biographie)
                ));

            var oeuvresElts = oeuvres.Select(oe =>
                new XElement(oe switch
                    {
                        Film => FILM,
                        Episode => EPISODE,
                        Trilogie => TRILOGIE,
                        Serie => SERIE,
                        _ => UNIVERS
                    },
                    new XAttribute("Titre", oe.Titre),
                    new XAttribute("Date", oe.DateDeSortie),
                    new XAttribute("Thème", oe.Theme),
                    new XAttribute("IsFamille", oe.IsFamilleF),
                    XElementIfNull("Image", oe.LienImage),
                    new XElement("Synopsis", oe.Synopsis),

                    new XElement("Réalisateurs",
                        oe.Personnes[REALISATEUR].Keys.Select(real =>
                            new XElement(REALISATEUR,
                                new XAttribute("Prénom", real.Prenom),
                                new XAttribute("Nom", real.Nom),
                                new XAttribute("Date", real.DateDeNaissance)))),

                    new XElement("Acteurs",
                        oe.Personnes[ACTEUR].Select(act =>
                            new XElement(ACTEUR,
                                new XAttribute("Prénom", act.Key.Prenom),
                                new XAttribute("Nom", act.Key.Nom),
                                new XAttribute("Date", act.Key.DateDeNaissance),
                                new XAttribute("Rôle", act.Value)))),

                    (oe switch
                    {
                        Composite comp => comp.ReOeuvres.Count != LISTE_MIN
                            ? new XElement(comp switch
                                {
                                    Trilogie => "Films",
                                    Serie => "Épisodes",
                                    _ => "Oeuvres"
                                },
                                comp.ReOeuvres.Select(compo =>
                                    new XElement(compo switch
                                        {
                                            Film => FILM,
                                            Episode => EPISODE,
                                            Trilogie => TRILOGIE,
                                            _ => SERIE
                                        },
                                        new XAttribute("Titre", compo.Titre),
                                        new XAttribute("Date", compo.DateDeSortie))))
                            : null,

                           
                        Leaf leaf =>  leaf.ListeStream.Count != LISTE_MIN 
                            ? new XElement("Streamings", 
                                leaf.ListeStream.Select(st => 
                                    new XElement("Stream",
                                        new XAttribute("Plateforme", st.Plateforme.ToString()),
                                        new XAttribute("Lien", st.Lien))))
                            : null,

                        _ => null
                    })!,

                    new XElement("Avis",
                        oe.ListeAvis.ToDictionary(pair => pair.Key, pair => pair.Value).Select(avis =>
                            new XElement("Avis",
                                new XAttribute("User", avis.Key.Pseudo),
                                new XAttribute("Note", avis.Value.Note),
                                new XElement("Commentaire", avis.Value.Commentaire))))
                )).ToList();

            file.Add(new XElement("Data",
                new XElement("Users", usersElts),
                new XElement("Personnes", personnesElts),

                new XElement("Oeuvres",
                    new XElement("Leafs",
                        new XElement("Films", oeuvresElts.Where(le => le.Name.LocalName.Equals(FILM))), 
                        new XElement("Épisodes", oeuvresElts.Where(le => le.Name.LocalName.Equals(EPISODE)))),

                    new XElement("Composites",
                        new XElement("Trilogies", oeuvresElts.Where(le => le.Name.LocalName.Equals(TRILOGIE))), 
                        new XElement("Séries", oeuvresElts.Where(le => le.Name.LocalName.Equals(SERIE))), 
                        new XElement("Univers", oeuvresElts.Where(comp => comp.Name.LocalName.Equals(UNIVERS)))))
            ));


            if (!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);

            if(_encryption is not null)
                _encryption.Encrypter(file, FileName, "Users");

            else
            {
                var settings = new XmlWriterSettings {Indent = true, Encoding = Encoding.UTF8};
                using TextWriter tw = File.CreateText(FilePath);
                using var writer = XmlWriter.Create(tw, settings);
                file.Save(writer);
            }
        }
    }
}