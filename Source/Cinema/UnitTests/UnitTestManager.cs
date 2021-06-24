using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Modele;
using static Modele.Themes;
using static Modele.Constante;

namespace UnitTests
{
    public partial class UnitTestManager
    {
        public (Leaf, Leaf, Leaf, Leaf, Leaf, Leaf, Composite, Composite, 
            Composite, Composite, Composite, Composite, Composite, Composite,
            Composite, User, User, Manager) CreatTestRendreListeOeuvres()
        {
            var man = new Manager();

            var date = new DateTime(1985, 04, 15);

            man.CreerUser("Admin", "Admin", out var user);
            user.IsAdmin = true;

            man.CreerUser("Ambi", "Satan", out var user2);
            man.ChangerInfos(user2, "Satan", "Ambi", "image", true, new List<Plateformes>());

            var film = man.CreerLeaf(user, "Film", "Fight Club", date, "image", "synopsis", Themes.Action, true);
            var film2 = man.CreerLeaf(user, "Film", "Club", date, "image", "synopsis", Themes.Action, true);
            var film3 = man.CreerLeaf(user, "Film", "Fight Club3", date, "image", "synopsis", Aventure, true);
            var film4 = man.CreerLeaf(user, "Film", "Fight Club4", date, "image", "synopsis", Aventure, false);

            var episode = man.CreerLeaf(user, "Épisode", "Fight Club", date, "image", "synopsis", Themes.Action, false);
            var episode2 = man.CreerLeaf(user, "Épisode", "Club", date, "image", "synopsis", Themes.Action, true);

            var trilogie = man.CreerComposite(user, "Trilogie", "Fight Club", date, "image", "synopsis", Themes.Action,
                false, new List<Oeuvre> { film2, film3, film4 });
            var trilogie2 = man.CreerComposite(user, "Trilogie", "Fight Club2", date, "image", "synopsis", Themes.Action,
                true, new List<Oeuvre> { film, film2, film3 });
            var trilogie3 = man.CreerComposite(user, "Trilogie", "Club2", date, "image", "synopsis", Themes.Action,
                true, new List<Oeuvre> { film, film, film });

            var serie = man.CreerComposite(user, "Série", "Fight Club", date, "image", "synopsis", Themes.Action,
                false, new List<Oeuvre> { episode });
            var serie2 = man.CreerComposite(user, "Série", "Fight Club2", date, "image", "synopsis", Themes.Action,
                true, new List<Oeuvre> { episode2 });
            var serie3 = man.CreerComposite(user, "Série", "Club2", date, "image", "synopsis", Themes.Action,
                true, new List<Oeuvre> { episode2 });

            var univers = man.CreerComposite(user, "Univers", "Fight Club", date, "image", "synopsis", Themes.Action,
                false, new List<Oeuvre> { trilogie, serie });
            var univers2 = man.CreerComposite(user, "Univers", "Fight Club2", date, "image", "synopsis", Themes.Action,
                true, new List<Oeuvre> { trilogie2, serie2 });
            var univers3 = man.CreerComposite(user, "Univers", "Club2", date, "image", "synopsis", Themes.Action,
                true, new List<Oeuvre> { trilogie2, serie2 });

            return (film, film2, film3, film4, episode, episode2, trilogie, trilogie2, trilogie3, serie, serie2, serie3,
                univers, univers2, univers3, user, user2, man);
        }

        [Fact]
        public void TestManager_RendreTheme()
        {
            var man = new Manager();

            var themes = Manager.RendreTousThemes().ToList();

            Assert.DoesNotContain("Inconnu", themes);
            Assert.Contains("Action", themes);
            Assert.Contains("Suspense", themes);
            Assert.Contains("Thriller", themes);
            Assert.Contains("Horreur", themes);
            Assert.Contains("Comédie", themes);
            Assert.Contains("Aventure", themes);
            Assert.Contains("Sf", themes);
            Assert.Contains("Drame", themes);
            Assert.Contains("Documentaire", themes);
        }

        [Fact]
        public void TestManager_RendreListeOeuvres()
        {
            var man = new Manager();

            var date = new DateTime(1985, 04, 15);

            man.CreerUser("Admin", "Admin", out var user);
            user.IsAdmin = true;

            man.CreerUser("Ambi", "Satan", out var user2);
            man.ChangerInfos(user2, "Satan", "Ambi", "image", true, new List<Plateformes>());

            var film = man.CreerLeaf(user, "Film", "Fight Club", date, "image", "synopsis", Themes.Action, true);
            var film2 = man.CreerLeaf(user, "Film", "Fight Club2", date, "image", "synopsis", Themes.Action, false);
            var episode = man.CreerLeaf(user, "Episode", "Fight Club", date, "image", "synopsis", Themes.Action, false);

            var oe = man.RendreListeOeuvres().ToList();

            Assert.Contains(film, oe);
            Assert.Contains(film2, oe);
            Assert.DoesNotContain(episode, oe);

            oe = man.RendreListeOeuvres(user).ToList();

            Assert.Contains(film, oe);
            Assert.Contains(film2, oe);
            Assert.DoesNotContain(episode, oe);

            oe = man.RendreListeOeuvres(user2).ToList();

            Assert.Contains(film, oe);
            Assert.DoesNotContain(film2, oe);
            Assert.DoesNotContain(episode, oe);
        }

        [Fact]
        public void TestManager_RendreListePopulaire()
        {
            var man = new Manager();

            var date = new DateTime(1985, 04, 15);

            var avis = new Avis(4.5f, "oui");
            var avis2 = new Avis(4f, "oui");


            man.CreerUser("Admin", "Admin", out var user);
            user.IsAdmin = true;

            man.CreerUser("Ambi", "Satan", out var user2);
            man.ChangerInfos(user2, "Satan", "Ambi", "image", true, new List<Plateformes>());

            var film = man.CreerLeaf(user, "Film", "Fight Club", date, "image", "synopsis", Themes.Action, true);
            var film2 = man.CreerLeaf(user, "Film", "Fight Club2", date, "image", "synopsis", Themes.Action, false);
            var film3 = man.CreerLeaf(user, "Film", "Fight Club3", date, "image", "synopsis", Themes.Action, false);
            var film4 = man.CreerLeaf(user, "Film", "Fight Club4", date, "image", "synopsis", Themes.Action, false);
            var film5 = man.CreerLeaf(user, "Film", "Fight Club5", date, "image", "synopsis", Themes.Action, true);
            var film6 = man.CreerLeaf(user, "Film", "Fight Club6", date, "image", "synopsis", Themes.Action, false);
            var film7 = man.CreerLeaf(user, "Film", "Fight Club7", date, "image", "synopsis", Themes.Action, false);
            var film8 = man.CreerLeaf(user, "Film", "Fight Club8", date, "image", "synopsis", Themes.Action, true);
            var film9 = man.CreerLeaf(user, "Film", "Fight Club9", date, "image", "synopsis", Themes.Action, false);
            var film10 = man.CreerLeaf(user, "Film", "Fight Club10", date, "image", "synopsis", Themes.Action, false);
            var film11 = man.CreerLeaf(user, "Film", "Fight Club11", date, "image", "synopsis", Themes.Action, true);
            var episode = man.CreerLeaf(user, "Episode", "Fight Club", date, "image", "synopsis", Themes.Action, false);

            film.AjouterAvis(user, avis);
            film2.AjouterAvis(user, avis);
            film3.AjouterAvis(user, avis2);
            film4.AjouterAvis(user, avis);
            film5.AjouterAvis(user, avis2);
            film6.AjouterAvis(user, avis2);
            film7.AjouterAvis(user, avis);
            film8.AjouterAvis(user, avis);
            film9.AjouterAvis(user, avis);
            film10.AjouterAvis(user, avis2);
            film11.AjouterAvis(user, avis);

            var oe = man.RendreListePopulaire().ToList(); // test de la classification par note

            Assert.Contains(film, oe);
            Assert.Contains(film2, oe);
            Assert.Contains(film3, oe);
            Assert.Contains(film4, oe);
            Assert.Contains(film5, oe);
            Assert.Contains(film6, oe);
            Assert.Contains(film7, oe);
            Assert.Contains(film8, oe);
            Assert.Contains(film9, oe);
            Assert.DoesNotContain(film10, oe);
            Assert.Contains(film11, oe);
            Assert.DoesNotContain(episode, oe);

            oe = man.RendreListePopulaire(user).ToList(); // classification pour un user sans mode famille

            Assert.Contains(film, oe);
            Assert.Contains(film2, oe);
            Assert.Contains(film3, oe);
            Assert.Contains(film4, oe);
            Assert.Contains(film5, oe);
            Assert.Contains(film6, oe);
            Assert.Contains(film7, oe);
            Assert.Contains(film8, oe);
            Assert.Contains(film9, oe);
            Assert.DoesNotContain(film10, oe);
            Assert.Contains(film11, oe);
            Assert.DoesNotContain(episode, oe);

            oe = man.RendreListePopulaire(user2).ToList(); // classification pour un user avec mode famille

            Assert.Contains(film, oe);
            Assert.DoesNotContain(film2, oe);
            Assert.DoesNotContain(film3, oe);
            Assert.DoesNotContain(film4, oe);
            Assert.Contains(film5, oe);
            Assert.DoesNotContain(film6, oe);
            Assert.DoesNotContain(film7, oe);
            Assert.Contains(film8, oe);
            Assert.DoesNotContain(film9, oe);
            Assert.DoesNotContain(film10, oe);
            Assert.Contains(film11, oe);
            Assert.DoesNotContain(episode, oe);
        }

        [Fact]
        public void TestManager_RendreListeOeuvresTheme()
        {
            var man = new Manager();

            var date = new DateTime(1985, 04, 15);

            man.CreerUser("Admin", "Admin", out var user);
            user.IsAdmin = true;

            man.CreerUser("Ambi", "Satan", out var user2);
            man.ChangerInfos(user2, "Satan", "Ambi", "image", true, new List<Plateformes>());

            var film = man.CreerLeaf(user, "Film", "Fight Club", date, "image", "synopsis", Themes.Action, false);
            var film2 = man.CreerLeaf(user, "Film", "Fight Club2", date, "image", "synopsis", Themes.Action, true);
            var film3 = man.CreerLeaf(user, "Film", "Fight Club3", date, "image", "synopsis", Aventure, true);
            var episode = man.CreerLeaf(user, "Episode", "Fight Club", date, "image", "synopsis", Themes.Action, false);

            var oe = man.RendreListeOeuvresTheme(Themes.Action).ToList();

            Assert.Contains(film, oe);
            Assert.Contains(film2, oe);
            Assert.DoesNotContain(film3, oe);
            Assert.DoesNotContain(episode, oe);
        }

        [Fact]
        public void TestManager_RendreListeOeuvresThemeUser()
        {
            var man = new Manager();

            var date = new DateTime(1985, 04, 15);

            man.CreerUser("Admin", "Admin", out var user);
            user.IsAdmin = true;

            man.CreerUser("Ambi", "Satan", out var user2);
            man.ChangerInfos(user2, "Satan", "Ambi", "image", true, new List<Plateformes>());

            var film = man.CreerLeaf(user, "Film", "Fight Club", date, "image", "synopsis", Themes.Action, false);
            var film2 = man.CreerLeaf(user, "Film", "Fight Club2", date, "image", "synopsis", Themes.Action, true);
            var film3 = man.CreerLeaf(user, "Film", "Fight Club3", date, "image", "synopsis", Aventure, true);
            var episode = man.CreerLeaf(user, "Episode", "Fight Club", date, "image", "synopsis", Themes.Action, false);

            var oe = man.RendreListeOeuvresTheme(Themes.Action, user).ToList();

            Assert.Contains(film, oe);
            Assert.Contains(film2, oe);
            Assert.DoesNotContain(film3, oe);
            Assert.DoesNotContain(episode, oe);

            oe = man.RendreListeOeuvresTheme(Themes.Action, user2).ToList();

            Assert.DoesNotContain(film, oe);
            Assert.Contains(film2, oe);
            Assert.DoesNotContain(film3, oe);
            Assert.DoesNotContain(episode, oe);
        }

        [Fact]
        public void TestManager_RendreListeOeuvresSearchOeuvre()
        {
            Manager man;
            User user, user2;

            Leaf film, film2, film3, film4, episode, episode2;

            Composite trilogie, trilogie2, trilogie3,
                serie, serie2, serie3,
                univers, univers2, univers3;

            (film, film2, film3, film4,
                episode, episode2,
                trilogie, trilogie2, trilogie3,
                serie, serie2, serie3,
                univers, univers2, univers3,
                user, user2,
                man) = CreatTestRendreListeOeuvres();

            var oe = man.RendreListeOeuvresSearch("Oeuvre","Fight").ToList();

            Assert.Contains(film, oe);
            Assert.DoesNotContain(film2, oe);
            Assert.Contains(film3, oe);
            Assert.Contains(film4, oe);
            Assert.DoesNotContain(episode, oe);
            Assert.DoesNotContain(episode2, oe);
            Assert.Contains(trilogie, oe);
            Assert.Contains(trilogie2, oe);
            Assert.DoesNotContain(trilogie3, oe);
            Assert.Contains(serie, oe);
            Assert.Contains(serie2, oe);
            Assert.DoesNotContain(serie3, oe);
            Assert.Contains(univers, oe);
            Assert.Contains(univers2, oe);
            Assert.DoesNotContain(univers3, oe);

            oe = man.RendreListeOeuvresSearch("Oeuvre", "Fight", user).ToList();

            Assert.Contains(film, oe);
            Assert.DoesNotContain(film2, oe);
            Assert.Contains(film3, oe);
            Assert.Contains(film4, oe);
            Assert.DoesNotContain(episode, oe);
            Assert.DoesNotContain(episode2, oe);
            Assert.Contains(trilogie, oe);
            Assert.Contains(trilogie2, oe);
            Assert.DoesNotContain(trilogie3, oe);
            Assert.Contains(serie, oe);
            Assert.Contains(serie2, oe);
            Assert.DoesNotContain(serie3, oe);
            Assert.Contains(univers, oe);
            Assert.Contains(univers2, oe);
            Assert.DoesNotContain(univers3, oe);

            oe = man.RendreListeOeuvresSearch("Oeuvre", "Fight", user2).ToList();

            Assert.Contains(film, oe);
            Assert.DoesNotContain(film2, oe);
            Assert.Contains(film3, oe);
            Assert.DoesNotContain(film4, oe);
            Assert.DoesNotContain(episode, oe);
            Assert.DoesNotContain(episode2, oe);
            Assert.DoesNotContain(trilogie, oe);
            Assert.Contains(trilogie2, oe);
            Assert.DoesNotContain(trilogie3, oe);
            Assert.DoesNotContain(serie, oe);
            Assert.Contains(serie2, oe);
            Assert.DoesNotContain(serie3, oe);
            Assert.DoesNotContain(univers, oe);
            Assert.Contains(univers2, oe);
            Assert.DoesNotContain(univers3, oe);
        }

        [Fact]
        public void TestManager_RendreListeOeuvresSearchFilm()
        {
            Manager man;
            User user, user2;

            Leaf film, film2, film3, film4, episode, episode2;

            Composite trilogie, trilogie2, trilogie3,
                serie, serie2, serie3,
                univers, univers2, univers3;

            (film, film2, film3, film4,
                episode, episode2,
                trilogie, trilogie2, trilogie3,
                serie, serie2, serie3,
                univers, univers2, univers3,
                user, user2,
                man) = CreatTestRendreListeOeuvres();

            var oe = man.RendreListeOeuvresSearch("Film", "Fight").ToList();

            Assert.Contains(film, oe);
            Assert.DoesNotContain(film2, oe);
            Assert.Contains(film3, oe);
            Assert.Contains(film4, oe);
            Assert.DoesNotContain(episode, oe);
            Assert.DoesNotContain(episode2, oe);
            Assert.DoesNotContain(trilogie, oe);
            Assert.DoesNotContain(trilogie2, oe);
            Assert.DoesNotContain(trilogie3, oe);
            Assert.DoesNotContain(serie, oe);
            Assert.DoesNotContain(serie2, oe);
            Assert.DoesNotContain(serie3, oe);
            Assert.DoesNotContain(univers, oe);
            Assert.DoesNotContain(univers2, oe);
            Assert.DoesNotContain(univers3, oe);

            oe = man.RendreListeOeuvresSearch("Film", "Fight", user).ToList();

            Assert.Contains(film, oe);
            Assert.DoesNotContain(film2, oe);
            Assert.Contains(film3, oe);
            Assert.Contains(film4, oe);
            Assert.DoesNotContain(episode, oe);
            Assert.DoesNotContain(episode2, oe);
            Assert.DoesNotContain(trilogie, oe);
            Assert.DoesNotContain(trilogie2, oe);
            Assert.DoesNotContain(trilogie3, oe);
            Assert.DoesNotContain(serie, oe);
            Assert.DoesNotContain(serie2, oe);
            Assert.DoesNotContain(serie3, oe);
            Assert.DoesNotContain(univers, oe);
            Assert.DoesNotContain(univers2, oe);
            Assert.DoesNotContain(univers3, oe);

            oe = man.RendreListeOeuvresSearch("Film", "Fight", user2).ToList();

            Assert.Contains(film, oe);
            Assert.DoesNotContain(film2, oe);
            Assert.Contains(film3, oe);
            Assert.DoesNotContain(film4, oe);
            Assert.DoesNotContain(episode, oe);
            Assert.DoesNotContain(episode2, oe);
            Assert.DoesNotContain(trilogie, oe);
            Assert.DoesNotContain(trilogie2, oe);
            Assert.DoesNotContain(trilogie3, oe);
            Assert.DoesNotContain(serie, oe);
            Assert.DoesNotContain(serie2, oe);
            Assert.DoesNotContain(serie3, oe);
            Assert.DoesNotContain(univers, oe);
            Assert.DoesNotContain(univers2, oe);
            Assert.DoesNotContain(univers3, oe);
        }

        [Fact]
        public void TestManager_RendreListeOeuvresSearchTrilogie()
        {
            Manager man;
            User user, user2;

            Leaf film, film2, film3, film4, episode, episode2;

            Composite trilogie, trilogie2, trilogie3,
                serie, serie2, serie3,
                univers, univers2, univers3;

            (film, film2, film3, film4,
                episode, episode2,
                trilogie, trilogie2, trilogie3,
                serie, serie2, serie3,
                univers, univers2, univers3,
                user, user2,
                man) = CreatTestRendreListeOeuvres();

            var oe = man.RendreListeOeuvresSearch("Trilogie", "Fight").ToList();

            Assert.DoesNotContain(film, oe);
            Assert.DoesNotContain(film2, oe);
            Assert.DoesNotContain(film3, oe);
            Assert.DoesNotContain(film4, oe);
            Assert.DoesNotContain(episode, oe);
            Assert.DoesNotContain(episode2, oe);
            Assert.Contains(trilogie, oe);
            Assert.Contains(trilogie2, oe);
            Assert.DoesNotContain(trilogie3, oe);
            Assert.DoesNotContain(serie, oe);
            Assert.DoesNotContain(serie2, oe);
            Assert.DoesNotContain(serie3, oe);
            Assert.DoesNotContain(univers, oe);
            Assert.DoesNotContain(univers2, oe);
            Assert.DoesNotContain(univers3, oe);

            oe = man.RendreListeOeuvresSearch("Trilogie", "Fight", user).ToList();

            Assert.DoesNotContain(film, oe);
            Assert.DoesNotContain(film2, oe);
            Assert.DoesNotContain(film3, oe);
            Assert.DoesNotContain(film4, oe);
            Assert.DoesNotContain(episode, oe);
            Assert.DoesNotContain(episode2, oe);
            Assert.Contains(trilogie, oe);
            Assert.Contains(trilogie2, oe);
            Assert.DoesNotContain(trilogie3, oe);
            Assert.DoesNotContain(serie, oe);
            Assert.DoesNotContain(serie2, oe);
            Assert.DoesNotContain(serie3, oe);
            Assert.DoesNotContain(univers, oe);
            Assert.DoesNotContain(univers2, oe);
            Assert.DoesNotContain(univers3, oe);

            oe = man.RendreListeOeuvresSearch("Trilogie", "Fight", user2).ToList();

            Assert.DoesNotContain(film, oe);
            Assert.DoesNotContain(film2, oe);
            Assert.DoesNotContain(film3, oe);
            Assert.DoesNotContain(film4, oe);
            Assert.DoesNotContain(episode, oe);
            Assert.DoesNotContain(episode2, oe);
            Assert.DoesNotContain(trilogie, oe);
            Assert.Contains(trilogie2, oe);
            Assert.DoesNotContain(trilogie3, oe);
            Assert.DoesNotContain(serie, oe);
            Assert.DoesNotContain(serie2, oe);
            Assert.DoesNotContain(serie3, oe);
            Assert.DoesNotContain(univers, oe);
            Assert.DoesNotContain(univers2, oe);
            Assert.DoesNotContain(univers3, oe);
        }

        [Fact]
        public void TestManager_RendreListeOeuvresSearchSerie()
        {
            Manager man;
            User user, user2;

            Leaf film, film2, film3, film4, episode, episode2;

            Composite trilogie, trilogie2, trilogie3,
                serie, serie2, serie3,
                univers, univers2, univers3;

            (film, film2, film3, film4,
                episode, episode2,
                trilogie, trilogie2, trilogie3,
                serie, serie2, serie3,
                univers, univers2, univers3,
                user, user2,
                man) = CreatTestRendreListeOeuvres();

            var oe = man.RendreListeOeuvresSearch("Série", "Fight").ToList();

            Assert.DoesNotContain(film, oe);
            Assert.DoesNotContain(film2, oe);
            Assert.DoesNotContain(film3, oe);
            Assert.DoesNotContain(film4, oe);
            Assert.DoesNotContain(episode, oe);
            Assert.DoesNotContain(episode2, oe);
            Assert.DoesNotContain(trilogie, oe);
            Assert.DoesNotContain(trilogie2, oe);
            Assert.DoesNotContain(trilogie3, oe);
            Assert.Contains(serie, oe);
            Assert.Contains(serie2, oe);
            Assert.DoesNotContain(serie3, oe);
            Assert.DoesNotContain(univers, oe);
            Assert.DoesNotContain(univers2, oe);
            Assert.DoesNotContain(univers3, oe);

            oe = man.RendreListeOeuvresSearch("Série", "Fight", user).ToList();

            Assert.DoesNotContain(film, oe);
            Assert.DoesNotContain(film2, oe);
            Assert.DoesNotContain(film3, oe);
            Assert.DoesNotContain(film4, oe);
            Assert.DoesNotContain(episode, oe);
            Assert.DoesNotContain(episode2, oe);
            Assert.DoesNotContain(trilogie, oe);
            Assert.DoesNotContain(trilogie2, oe);
            Assert.DoesNotContain(trilogie3, oe);
            Assert.Contains(serie, oe);
            Assert.Contains(serie2, oe);
            Assert.DoesNotContain(serie3, oe);
            Assert.DoesNotContain(univers, oe);
            Assert.DoesNotContain(univers2, oe);
            Assert.DoesNotContain(univers3, oe);

            oe = man.RendreListeOeuvresSearch("Série", "Fight", user2).ToList();

            Assert.DoesNotContain(film, oe);
            Assert.DoesNotContain(film2, oe);
            Assert.DoesNotContain(film3, oe);
            Assert.DoesNotContain(film4, oe);
            Assert.DoesNotContain(episode, oe);
            Assert.DoesNotContain(episode2, oe);
            Assert.DoesNotContain(trilogie, oe);
            Assert.DoesNotContain(trilogie2, oe);
            Assert.DoesNotContain(trilogie3, oe);
            Assert.DoesNotContain(serie, oe);
            Assert.Contains(serie2, oe);
            Assert.DoesNotContain(serie3, oe);
            Assert.DoesNotContain(univers, oe);
            Assert.DoesNotContain(univers2, oe);
            Assert.DoesNotContain(univers3, oe);
        }

        [Fact]
        public void TestManager_RendreListeOeuvresSearchUnivers()
        {
            Manager man;
            User user, user2;

            Leaf film, film2, film3, film4, episode, episode2;

            Composite trilogie, trilogie2, trilogie3,
                serie, serie2, serie3,
                univers, univers2, univers3;

            (film, film2, film3, film4,
                episode, episode2,
                trilogie, trilogie2, trilogie3,
                serie, serie2, serie3,
                univers, univers2, univers3,
                user, user2,
                man) = CreatTestRendreListeOeuvres();

            var oe = man.RendreListeOeuvresSearch("Univers", "Fight").ToList();

            Assert.DoesNotContain(film, oe);
            Assert.DoesNotContain(film2, oe);
            Assert.DoesNotContain(film3, oe);
            Assert.DoesNotContain(film4, oe);
            Assert.DoesNotContain(episode, oe);
            Assert.DoesNotContain(episode2, oe);
            Assert.DoesNotContain(trilogie, oe);
            Assert.DoesNotContain(trilogie2, oe);
            Assert.DoesNotContain(trilogie3, oe);
            Assert.DoesNotContain(serie, oe);
            Assert.DoesNotContain(serie2, oe);
            Assert.DoesNotContain(serie3, oe);
            Assert.Contains(univers, oe);
            Assert.Contains(univers2, oe);
            Assert.DoesNotContain(univers3, oe);

            oe = man.RendreListeOeuvresSearch("Univers", "Fight", user).ToList();

            Assert.DoesNotContain(film, oe);
            Assert.DoesNotContain(film2, oe);
            Assert.DoesNotContain(film3, oe);
            Assert.DoesNotContain(film4, oe);
            Assert.DoesNotContain(episode, oe);
            Assert.DoesNotContain(episode2, oe);
            Assert.DoesNotContain(trilogie, oe);
            Assert.DoesNotContain(trilogie2, oe);
            Assert.DoesNotContain(trilogie3, oe);
            Assert.DoesNotContain(serie, oe);
            Assert.DoesNotContain(serie2, oe);
            Assert.DoesNotContain(serie3, oe);
            Assert.Contains(univers, oe);
            Assert.Contains(univers2, oe);
            Assert.DoesNotContain(univers3, oe);

            oe = man.RendreListeOeuvresSearch("Univers", "Fight", user2).ToList();

            Assert.DoesNotContain(film, oe);
            Assert.DoesNotContain(film2, oe);
            Assert.DoesNotContain(film3, oe);
            Assert.DoesNotContain(film4, oe);
            Assert.DoesNotContain(episode, oe);
            Assert.DoesNotContain(episode2, oe);
            Assert.DoesNotContain(trilogie, oe);
            Assert.DoesNotContain(trilogie2, oe);
            Assert.DoesNotContain(trilogie3, oe);
            Assert.DoesNotContain(serie, oe);
            Assert.DoesNotContain(serie2, oe);
            Assert.DoesNotContain(serie3, oe);
            Assert.DoesNotContain(univers, oe);
            Assert.Contains(univers2, oe);
            Assert.DoesNotContain(univers3, oe);
        }

        [Fact]
        public void TestManager_RendreListeActeurs()
        {
            var man = new Manager();

            var date = new DateTime(1985, 04, 15);

            man.CreerUser("Admin", "Admin", out var user);
            user.IsAdmin = true;

           var pers =  man.CreerPersonne(user, "Brad", "Pitt", "oui", "Américain", "image", date);
           var pers2 =  man.CreerPersonne(user, "Leonardo", "DiCaprio", "oui", "Américain", "image", date);

           man.CreerLeaf(user, "Film", "Fight Club", date, "image", "synopsis", Themes.Action, false,
               new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>{["Acteur"] = new Dictionary<Personne, string> {[pers] = "Roger"}});


           var per = man.RendreListeActeurs().ToList();

           Assert.Contains(pers, per);
           Assert.DoesNotContain(pers2, per);
        }

        [Fact]
        public void TestManager_RendreListeActeursSearch()
        {
            var man = new Manager();

            var date = new DateTime(1985, 04, 15);

            man.CreerUser("Admin", "Admin", out var user);
            user.IsAdmin = true;

            var pers =  man.CreerPersonne(user, "Brad", "Pitt", "oui", "Américain", "image", date);
            var pers2 =  man.CreerPersonne(user, "Leonardo", "DiCaprio", "oui", "Américain", "image", date);
            var pers3 =  man.CreerPersonne(user, "Michel", "Du Pont", "oui", "Américain", "image", date);

            man.CreerLeaf(user, "Film", "Fight Club", date, "image", "synopsis", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>{["Acteur"] = new Dictionary<Personne, string> {[pers] = "Roger", [pers2] = "Michel"}});

            var per = man.RendreListeActeursSearch(ACTEUR, "Brad").ToList();

            Assert.Contains(pers, per);
            Assert.DoesNotContain(pers2, per);
            Assert.DoesNotContain(pers3, per);

            per = man.RendreListeActeursSearch(ACTEUR ,"Pitt").ToList();

            Assert.Contains(pers, per);
            Assert.DoesNotContain(pers2, per);
            Assert.DoesNotContain(pers3, per);

            per = man.RendreListeActeursSearch(ACTEUR, "Brad", "Pitt").ToList();

            Assert.Contains(pers, per);
            Assert.DoesNotContain(pers2, per);
            Assert.DoesNotContain(pers3, per);

            per = man.RendreListeActeursSearch(ACTEUR, "Pitt", "Brad").ToList();

            Assert.Contains(pers, per);
            Assert.DoesNotContain(pers2, per);
            Assert.DoesNotContain(pers3, per);
        }

        [Fact]
        public void TestManager_RendreListeAvis()
        {
            var man = new Manager();

            var date = new DateTime(1985, 04, 15);

            man.CreerUser("Admin", "Admin", out var user);
            user.IsAdmin = true;

            man.CreerUser("Ambi", "Satan", out var user2);

            var film = man.CreerLeaf(user, "Film", "Fight Club", date, "image", "synopsis", Themes.Action, false);
            var film2 = man.CreerLeaf(user, "Film", "Fight Club2", date, "image", "synopsis", Themes.Action, false);
            var film3 = man.CreerLeaf(user, "Film", "Fight Club3", date, "image", "synopsis", Themes.Action, false);

            var avis1 = new Avis(4.5f, "cool");
            var avis2 = new Avis(3.5f, "nul");
            var avis3 = new Avis(4f, "ouii");
            var avis4 = new Avis(4.5f, "non");
            var avis5 = new Avis(4.5f, "noice");

            film.AjouterAvis(user, avis1);
            film.AjouterAvis(user2, avis2);
            film2.AjouterAvis(user, avis3);
            film2.AjouterAvis(user2, avis4);
            film3.AjouterAvis(user2, avis5);

            var avis = man.RendreListeAvis(user, out var nbA).ToDictionary(oe => oe.Key, av => av.Value);

            Assert.Equal(2, avis.Count);

            Assert.Contains(film, avis.Keys);
            Assert.Contains(avis1, avis.Values);
            Assert.DoesNotContain(avis2, avis.Values);

            Assert.Contains(film2, avis.Keys);
            Assert.Contains(avis3, avis.Values);
            Assert.DoesNotContain(avis4, avis.Values);

            Assert.DoesNotContain(film3, avis.Keys);
            Assert.DoesNotContain(avis5, avis.Values);
        }
    }
}