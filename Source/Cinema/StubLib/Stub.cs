using System;
using System.Collections.Generic;
using System.Linq;
using Modele;
using static Modele.Constante;
using static Modele.Plateformes;

namespace StubLib
{
    /// <summary>
    /// créer de fausse donnée
    /// </summary>
    public class Stub : IPersistance
    {
        private readonly IFactory _factory;

        /// <summary>
        /// Constructeur par défaut de StubLib
        /// </summary>
        public Stub() =>_factory = new LoadingFactory();

        /// <summary>
        /// Permet de choisir la factory de StubLib
        /// </summary>
        /// <param name="factory"></param>
        public Stub(IFactory factory) => _factory = factory;

        /// <summary>
        /// Permet de créer les fausses données
        /// </summary>
        /// <returns></returns>
        public (IEnumerable<Personne> personnes, IEnumerable<Oeuvre> oeuvres, IEnumerable<User> users) Charger()
        {
            var personnes = new List<Personne>();
            var oeuvres = new List<Oeuvre>();
            var users = new List<User>();

            var us1 = _factory.CreerUser("Antoine", "Admin", null, false, true, new List<Plateformes> {Netflix, PrimeVideo});

            var us2 = _factory.CreerUser("Samuel", "Admin", null, false, true, new List<Plateformes> {Netflix, PrimeVideo, OCS});

            var us3 = _factory.CreerUser("Kikou63", "123", null, true, false, new List<Plateformes> {Netflix});

            users.Add(us1);
            users.Add(us2);
            users.Add(us3);

            var real = _factory.CreerPersonne("David", "Fincher", 
                "Il est principalement connu pour avoir réalisé les films Seven, Fight Club, L'Étrange Histoire de Benjamin Button, The Social Network et Gone Girl " +
                "qui lui ont valu diverses récompenses et nominations aux Oscars du cinéma ou aux Golden Globes. Réputé pour son perfectionnisme, il peut tourner un très grand nombre de prises de ses plans" +
                " et séquences afin d'obtenir le rendu visuel qu'il désire. Il a également développé et produit les séries télévisées House of Cards (pour laquelle il remporte l'Emmy Award de la meilleure réalisation" +
                " pour une série dramatique en 2013) et Mindhunter, diffusées sur Netflix. ", 
                "Américaine", null, new DateTime(1962, 08, 28));

            var act1 = _factory.CreerPersonne("Edward", "Norton",
                "Edward Harrison Norton passe son enfance à Columbia dans le Maryland. Diplômé en histoire de l'Université Yale, il est féru d'art dramatique " +
                "depuis son plus jeune âge. Après ses études, il travaille à Osaka, au Japon, comme consultant pour l'entreprise de son grand-père. Il apprend la " +
                "langue et sait parler japonais. Puis, il part s'installer à New York où il décide de suivre une carrière d'acteur en débutant avec la troupe des Signature Players.",
                "Américaine", null, new DateTime(1969, 08, 18));

            var act2 = _factory.CreerPersonne("Brad", "Pitt",
                "Repéré dans une publicité pour Levi's, Brad Pitt sort de l'anonymat grâce à un petit rôle dans le film Thelma et Louise" +
                " de Ridley Scott. En très peu de temps, il devient une véritable star et sa collaboration avec le réalisateur David Fincher donne naissance " +
                "aux films culte Seven, Fight Club et L'Étrange Histoire de Benjamin Button. Il tourne dans de nombreux autres succès comme Entretien avec un vampire de Neil Jordan, " +
                "Ocean's Eleven et ses suites de Steven Soderbergh, Troie de Wolfgang Petersen et Inglourious Basterds de Quentin Tarantino. Au cours de sa carrière, il reçoit six nominations " +
                "aux Oscars et cinq nominations aux Golden Globes, dont un remporté pour L'Armée des douze singes de Terry Gilliam en 1996. ",
                "Américaine", null, new DateTime(1963, 12, 18));

            var act3 = _factory.CreerPersonne("Helena", "Bonham Carter",
                "Elle est reconnue pour ses interprétations de personnages excentriques et originaux. Elle est devenue célèbre grâce au rôle de Marla Singer dans Fight Club, puis avec ses nombreuses " +
                "collaborations avec Tim Burton comme dans Charlie et la Chocolaterie, Dark Shadows ou Alice au pays des merveilles. Elle a également joué le rôle de Bellatrix Lestrange " +
                "dans la saga Harry Potter. ", "Américaine", null,
                new DateTime(1962, 08, 28));

            personnes.Add(real);
            personnes.Add(act1);
            personnes.Add(act2);
            personnes.Add(act3);

            var personne = new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>();
            personne.Add(REALISATEUR, new Dictionary<Personne, string> {[real] = ""});
            personne.Add(ACTEUR,
                new Dictionary<Personne, string>
                    {[act1] = "The Narrator", [act2] = "Tyler Durden", [act3] = "Maria Singer"});

            var stream = _factory.AjouterStreamLeaf("FightClub",
                new Dictionary<Plateformes, string>
                {
                    [PrimeVideo] = "https://www.primevideo.com/detail/Fight-Club/0GDP9E5PK17E66TK19EJL1X30J"
                });
            var stream2 = _factory.AjouterStreamLeaf("FightClub2",
                new Dictionary<Plateformes, string>
                {
                    [PrimeVideo] = "https://www.primevideo.com/detail/Fight-Club/0GDP9E5PK17E66TK19EJL1X30J"
                });

            var avis1 = _factory.CreerAvis(4.5f, "J'aime bien ce film");
            var avis2 = _factory.CreerAvis(2f, "C'est une vraie bouse");
            var avis3 = _factory.CreerAvis(3.5f, "Ouais bof... ai vue mieux");

            var avis = new Dictionary<User, Avis> { [us2] = avis1, [us1] = avis3, [us3] = avis2 };


            var oe1 = _factory.CreerLeaf(FILM, "Fight Club", new DateTime(1999, 11, 11),
                null, "Le narrateur, sans identité précise, vit seul, travaille seul, dort seul, mange seul ses plateaux-repas pour une personne comme beaucoup d'autres personnes seules" +
                      " qui connaissent la misère humaine, morale et sexuelle. C'est pourquoi il va devenir membre du Fight club, un lieu clandestin ou il va pouvoir retrouver sa virilité, l'échange" +
                      " et la communication. Ce club est dirigé par Tyler Durden, une sorte d'anarchiste entre gourou et philosophe qui prêche l'amour de son prochain. ",
                Themes.Thriller, false, personne, stream, avis);

            var oe2 = _factory.CreerLeaf(EPISODE, "Fight Club2", new DateTime(1999, 11, 11),
                null, "Le narrateur, sans identité précise, vit seul, travaille seul, dort seul, mange seul ses plateaux-repas pour une personne comme beaucoup d'autres personnes seules" +
                      " qui connaissent la misère humaine, morale et sexuelle. C'est pourquoi il va devenir membre du Fight club, un lieu clandestin ou il va pouvoir retrouver sa virilité, l'échange" +
                      " et la communication. Ce club est dirigé par Tyler Durden, une sorte d'anarchiste entre gourou et philosophe qui prêche l'amour de son prochain. ",
                Themes.Thriller, false, personne, stream2, avis);

            var oe3 = _factory.CreerComposite(SERIE, "Fight Club2", new DateTime(1999, 11, 11),
                null, "Une Série de 1 épisode qui est la suite jamais sorti de fight Club qui pourtant est sorite le même jours comme son seul et unique épisode INCROYABLE !!!",
                Themes.Thriller, false, new List<Oeuvre> {oe2}, personne, avis);

            var oe4 = _factory.CreerComposite(UNIVERS, "Fight Club", new DateTime(1999, 11, 11),
                null, "L'univers incroyablement grand de Fight Club avec un film et une série d'un épisode magnifique",
                Themes.Thriller, false, new List<Oeuvre> {oe1, oe3}, personne, avis);

            oeuvres.Add(oe1);
            oeuvres.Add(oe2);
            oeuvres.Add(oe3);
            oeuvres.Add(oe4);

            _factory.ModifierLiseUser(us1, new List<Oeuvre>{oe1, oe3}, new List<object> {oe4, act1, act2, oe1, oe3});
            _factory.ModifierLiseUser(us2, new List<Oeuvre> {oe1}, new List<object> {oe4, act1, act2, act3, oe1});
            _factory.ModifierLiseUser(us3, new List<Oeuvre>(), new List<object> {act1, act2});

            return (personnes, oeuvres, users);
        }

        public void Sauvegarder(IEnumerable<Personne> personnes, IEnumerable<Oeuvre> oeuvres, IEnumerable<User> users)
        {
            throw new NotImplementedException();
        }
    }
}