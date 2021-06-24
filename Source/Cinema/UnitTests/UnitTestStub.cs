using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modele;
using Xunit;
using StubLib;
using static Modele.Plateformes;
using static Modele.Constante;

namespace UnitTests
{
    public class UnitTestStub
    {
        private Stub stub = new();

        [Theory]
        [InlineData("Antoine", "Admin", null, false, true, new[] {"Netflix", "PrimeVideo"})]
        [InlineData("Samuel", "Admin", null, false, true, new[] {"Netflix", "PrimeVideo", "OCS"})]
        [InlineData("Kikou63", "123", null, true, false, new[] {"Netflix"})]
        public void TestStubUser(string pseudo, string passwd, string photo, bool famille, bool isAdmin, string[] plats)
        {
            var (_, _, users) = stub.Charger();

            User us = new(pseudo, passwd);
            us.ChangerProfil(pseudo, photo, famille, plats.Select(Enum.Parse<Plateformes>));
            us.IsAdmin = isAdmin;

            Assert.NotNull(users.SingleOrDefault(user => user.Pseudo.Equals(us.Pseudo) &&
                                                         user.Mdp.Equals(us.Mdp) &&
                                                         user.ImageProfil is null && us.ImageProfil is null &&
                                                         user.IsModeFamille == us.IsModeFamille &&
                                                         user.IsAdmin == us.IsAdmin &&
                                                         user.ListePlateformes.ToString()!.Equals(us.ListePlateformes
                                                             .ToString())));
        }

        [Theory]
        [InlineData("David", "Fincher",
            "Il est principalement connu pour avoir réalisé les films Seven, Fight Club, L'Étrange Histoire de Benjamin Button, The Social Network et Gone Girl " +
            "qui lui ont valu diverses récompenses et nominations aux Oscars du cinéma ou aux Golden Globes. Réputé pour son perfectionnisme, il peut tourner un très grand nombre de prises de ses plans" +
            " et séquences afin d'obtenir le rendu visuel qu'il désire. Il a également développé et produit les séries télévisées House of Cards (pour laquelle il remporte l'Emmy Award de la meilleure réalisation" +
            " pour une série dramatique en 2013) et Mindhunter, diffusées sur Netflix. ",
            "Américaine", null, "28/08/1962")] 
        [InlineData("Edward", "Norton",
            "Edward Harrison Norton passe son enfance à Columbia dans le Maryland. Diplômé en histoire de l'Université Yale, il est féru d'art dramatique " +
            "depuis son plus jeune âge. Après ses études, il travaille à Osaka, au Japon, comme consultant pour l'entreprise de son grand-père. Il apprend la " +
            "langue et sait parler japonais. Puis, il part s'installer à New York où il décide de suivre une carrière d'acteur en débutant avec la troupe des Signature Players.",
            "Américaine", null, "18/08/1969")]
        [InlineData("Brad", "Pitt",
            "Repéré dans une publicité pour Levi's, Brad Pitt sort de l'anonymat grâce à un petit rôle dans le film Thelma et Louise" +
            " de Ridley Scott. En très peu de temps, il devient une véritable star et sa collaboration avec le réalisateur David Fincher donne naissance " +
            "aux films culte Seven, Fight Club et L'Étrange Histoire de Benjamin Button. Il tourne dans de nombreux autres succès comme Entretien avec un vampire de Neil Jordan, " +
            "Ocean's Eleven et ses suites de Steven Soderbergh, Troie de Wolfgang Petersen et Inglourious Basterds de Quentin Tarantino. Au cours de sa carrière, il reçoit six nominations " +
            "aux Oscars et cinq nominations aux Golden Globes, dont un remporté pour L'Armée des douze singes de Terry Gilliam en 1996. ",
            "Américaine", null, "18/12/1963")]
        [InlineData("Helena", "Bonham Carter",
            "Elle est reconnue pour ses interprétations de personnages excentriques et originaux. Elle est devenue célèbre grâce au rôle de Marla Singer dans Fight Club, puis avec ses nombreuses " +
            "collaborations avec Tim Burton comme dans Charlie et la Chocolaterie, Dark Shadows ou Alice au pays des merveilles. Elle a également joué le rôle de Bellatrix Lestrange " +
            "dans la saga Harry Potter. ", "Américaine", null,
            "28/08/1962")]
        public void TestStubPersonnes(string prenom, string nom, string bio, string nat, string lienImg, string date)
        {
            var (personnes, _, _) = stub.Charger();

            Personne pers = new(nom, prenom, bio, nat, lienImg, DateTime.Parse(date));

            Assert.NotNull(personnes.SingleOrDefault(personne => personne.Prenom.Equals(pers.Prenom) &&
                                                         personne.Nom.Equals(pers.Nom) &&
                                                         personne.LienImage is null && pers.LienImage is null &&
                                                         personne.Biographie.Equals(pers.Biographie) &&
                                                         personne.Nationalite.Equals(pers.Nationalite) &&
                                                         personne.DateDeNaissance.Equals(pers.DateDeNaissance)));
        }

        [Fact]
        public void TestStubOeuvres()
        {
            var (_, oeuvres, _) = stub.Charger();

            var us1 = new User("Antoine", "Admin");
            us1.ChangerProfil("Antoine", null, false, new List<Plateformes> {Netflix, PrimeVideo});
            us1.IsAdmin = true;

            var us2 = new User("Samuel", "Admin");
            us2.ChangerProfil("Samuel", null, false, new List<Plateformes> {Netflix, PrimeVideo, OCS});
            us2.IsAdmin = true;

            var us3 = new User("Kikou63", "123");
            us3.ChangerProfil("Kikou63", null, true, new List<Plateformes> {Netflix});

            var real = new Personne("David", "Fincher",
                "Il est principalement connu pour avoir réalisé les films Seven, Fight Club, L'Étrange Histoire de Benjamin Button, The Social Network et Gone Girl " +
                "qui lui ont valu diverses récompenses et nominations aux Oscars du cinéma ou aux Golden Globes. Réputé pour son perfectionnisme, il peut tourner un très grand nombre de prises de ses plans" +
                " et séquences afin d'obtenir le rendu visuel qu'il désire. Il a également développé et produit les séries télévisées House of Cards (pour laquelle il remporte l'Emmy Award de la meilleure réalisation" +
                " pour une série dramatique en 2013) et Mindhunter, diffusées sur Netflix. ",
                "Américaine", null, new DateTime(1962, 08, 28));

            var act1 = new Personne("Edward", "Norton",
                "Edward Harrison Norton passe son enfance à Columbia dans le Maryland. Diplômé en histoire de l'Université Yale, il est féru d'art dramatique " +
                "depuis son plus jeune âge. Après ses études, il travaille à Osaka, au Japon, comme consultant pour l'entreprise de son grand-père. Il apprend la " +
                "langue et sait parler japonais. Puis, il part s'installer à New York où il décide de suivre une carrière d'acteur en débutant avec la troupe des Signature Players.",
                "Américaine", null, new DateTime(1969, 08, 18));

            var act2 = new Personne("Brad", "Pitt",
                "Repéré dans une publicité pour Levi's, Brad Pitt sort de l'anonymat grâce à un petit rôle dans le film Thelma et Louise" +
                " de Ridley Scott. En très peu de temps, il devient une véritable star et sa collaboration avec le réalisateur David Fincher donne naissance " +
                "aux films culte Seven, Fight Club et L'Étrange Histoire de Benjamin Button. Il tourne dans de nombreux autres succès comme Entretien avec un vampire de Neil Jordan, " +
                "Ocean's Eleven et ses suites de Steven Soderbergh, Troie de Wolfgang Petersen et Inglourious Basterds de Quentin Tarantino. Au cours de sa carrière, il reçoit six nominations " +
                "aux Oscars et cinq nominations aux Golden Globes, dont un remporté pour L'Armée des douze singes de Terry Gilliam en 1996. ",
                "Américaine", null, new DateTime(1963, 12, 18));

            var act3 = new Personne("Helena", "Bonham Carter",
                "Elle est reconnue pour ses interprétations de personnages excentriques et originaux. Elle est devenue célèbre grâce au rôle de Marla Singer dans Fight Club, puis avec ses nombreuses " +
                "collaborations avec Tim Burton comme dans Charlie et la Chocolaterie, Dark Shadows ou Alice au pays des merveilles. Elle a également joué le rôle de Bellatrix Lestrange " +
                "dans la saga Harry Potter. ", "Américaine", null,
                new DateTime(1962, 08, 28));

            var personnes = new List<Personne>();
            personnes.Add(real);
            personnes.Add(act1);
            personnes.Add(act2);
            personnes.Add(act3);

            var personne = new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>();
            personne.Add(REALISATEUR, new Dictionary<Personne, string> {[real] = ""});
            personne.Add(ACTEUR,
                new Dictionary<Personne, string>
                    {[act1] = "The Narrator", [act2] = "Tyler Durden", [act3] = "Maria Singer"});

            var stream = new List<Streaming>() { new Streaming("FightClub",
                "https://www.primevideo.com/detail/Fight-Club/0GDP9E5PK17E66TK19EJL1X30J", PrimeVideo)} ;

            var stream2 = new List<Streaming>() { new Streaming("FightClub2",
                "https://www.primevideo.com/detail/Fight-Club/0GDP9E5PK17E66TK19EJL1X30J", PrimeVideo)};

            var avis1 = new Avis(4.6f, "genial");
            var avis2 = new Avis(2f, "C'est une vraie bouse");
            var avis3 = new Avis(3.5f, "Ouais bof... ai vue mieux");

            var oe1 = new Film("Fight Club", new DateTime(1999, 11, 11),
                null, "Le narrateur, sans identité précise, vit seul, travaille seul, dort seul, mange seul ses plateaux-repas pour une personne comme beaucoup d'autres personnes seules" +
                      " qui connaissent la misère humaine, morale et sexuelle. C'est pourquoi il va devenir membre du Fight club, un lieu clandestin ou il va pouvoir retrouver sa virilité, l'échange" +
                      " et la communication. Ce club est dirigé par Tyler Durden, une sorte d'anarchiste entre gourou et philosophe qui prêche l'amour de son prochain. ",
                Themes.Thriller, false, personne, stream);
            oe1.AjouterAvis(us2, avis1);
            oe1.AjouterAvis(us1, avis3);
            oe1.AjouterAvis(us3, avis2);

            var oe2 = new Episode("Fight Club2", new DateTime(1999, 11, 11),
                null, "Le narrateur, sans identité précise, vit seul, travaille seul, dort seul, mange seul ses plateaux-repas pour une personne comme beaucoup d'autres personnes seules" +
                      " qui connaissent la misère humaine, morale et sexuelle. C'est pourquoi il va devenir membre du Fight club, un lieu clandestin ou il va pouvoir retrouver sa virilité, l'échange" +
                      " et la communication. Ce club est dirigé par Tyler Durden, une sorte d'anarchiste entre gourou et philosophe qui prêche l'amour de son prochain. ",
                Themes.Thriller, false, personne, stream2);
            oe2.AjouterAvis(us2, avis1);
            oe2.AjouterAvis(us1, avis3);
            oe2.AjouterAvis(us3, avis2);

            var oe3 = new Serie("Fight Club2", new DateTime(1999, 11, 11),
                null, "Une Série de 1 épisode qui est la suite jamais sorti de fight Club qui pourtant est sorite le même jours comme son seul et unique épisode INCROYABLE !!!",
                Themes.Thriller, false, personne);
            oe3.AjouterAvis(us2, avis1);
            oe3.AjouterAvis(us1, avis3);
            oe3.AjouterAvis(us3, avis2);
            oe3.AjouterOeuvres(new List<Oeuvre> { oe2 });

            var oe4 = new Univers("Fight Club", new DateTime(1999, 11, 11),
                null, "L'univers incroyablement grand de Fight Club avec un film et une série d'un épisode magnifique",
                Themes.Thriller, false, personne, new List<Oeuvre> { oe1, oe3 });
            oe4.AjouterAvis(us2, avis1);
            oe4.AjouterAvis(us1, avis3);
            oe4.AjouterAvis(us3, avis2);

            Assert.Contains(oe1, oeuvres);
            Assert.Contains(oe2, oeuvres);
            Assert.Contains(oe3, oeuvres);
            Assert.Contains(oe4, oeuvres);
        }
    }
}
