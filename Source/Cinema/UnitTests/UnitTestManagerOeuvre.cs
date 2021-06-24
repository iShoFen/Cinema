using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Modele;
using static Modele.Plateformes;
using static Modele.Themes;

namespace UnitTests
{
    public partial  class UnitTestManager
    {
        [Fact]
        public void TestManager_CreerLeafFilm()
        {
            var man = new Manager();

            var date = new DateTime(1985, 04, 15);

            man.CreerUser("Admin", "Admin", out var user);
            user.IsAdmin = true;

            man.CreerUser("Ambi", "Satan", out _);

            var pers = man.CreerPersonne(user, "Brad", "Pitt", "oui", "Américain", "image", date);

            var listeP = new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>
                {["acteur"] = new Dictionary<Personne, string> {[pers] = "Michou"}};

            var stream = new Manager().AjouterStreamLeaf("Fight Club",
                    new Dictionary<Plateformes, string> {[Netflix] = "netflix.com", [PrimeVideo] = "primevideo.com"})
                .ToList();

            var film = man.CreerLeaf(user, "Film", "Fight Club", date, "image", "synopsis", Themes.Action, false);

            Assert.NotNull(film);
            Assert.Contains(film, man.Oeuvres);

            man.SupprimerOeuvre(user, film);

            film = man.CreerLeaf(user, "Film", "Fight Club", date, "image", "synopsis", Themes.Action, false, listeP);

            Assert.NotNull(film);
            Assert.Contains(film, man.Oeuvres);
            Assert.NotEmpty(film.Personnes);

            man.SupprimerOeuvre(user, film);

            film = man.CreerLeaf(user, "Film", "Fight Club", date, "image", "synopsis", Themes.Action, false,
                listeStream: stream);

            Assert.NotNull(film);
            Assert.Contains(film, man.Oeuvres);
            Assert.NotEmpty(film.ListeStream);

            man.SupprimerOeuvre(user, film);

            film = man.CreerLeaf(user, "Film", "Fight Club", date, "image", "synopsis", Themes.Action, false, listeP,
                stream);

            Assert.NotNull(film);
            Assert.Contains(film, man.Oeuvres);
            Assert.NotEmpty(film.Personnes);
            Assert.NotEmpty(film.ListeStream);
        }

        [Fact]
        public void TestManager_CreerLeafEpisode()
        {
            var man = new Manager();

            var date = new DateTime(1985, 04, 15);

            man.CreerUser("Admin", "Admin", out var user);
            user.IsAdmin = true;

            man.CreerUser("Ambi", "Satan", out _);

            var pers = man.CreerPersonne(user, "Brad", "Pitt", "oui", "Américain", "image", date);

            var listeP = new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>
            { ["acteur"] = new Dictionary<Personne, string> { [pers] = "Michou" } };

            var stream = new Manager().AjouterStreamLeaf("Fight Club",
                    new Dictionary<Plateformes, string> { [Netflix] = "netflix.com", [PrimeVideo] = "primevideo.com" })
                .ToList();
            
            var episode = man.CreerLeaf(user, "Épisode", "Fight Club", date, "image", "synopsis", Themes.Action, false);

            Assert.NotNull(episode);
            Assert.Contains(episode, man.Oeuvres);

            man.SupprimerOeuvre(user, episode);

            episode = man.CreerLeaf(user, "Épisode", "Fight Club", date, "image", "synopsis", Themes.Action, false, listeP);

            Assert.NotNull(episode);
            Assert.Contains(episode, man.Oeuvres);
            Assert.NotEmpty(episode.Personnes);

            man.SupprimerOeuvre(user, episode);

            episode = man.CreerLeaf(user, "Épisode", "Fight Club", date, "image", "synopsis", Themes.Action, false,
                listeStream: stream);

            Assert.NotNull(episode);
            Assert.Contains(episode, man.Oeuvres);
            Assert.NotEmpty(episode.ListeStream);

            man.SupprimerOeuvre(user, episode);

            episode = man.CreerLeaf(user, "Épisode", "Fight Club", date, "image", "synopsis", Themes.Action, false, listeP,
                stream);

            Assert.NotNull(episode);
            Assert.Contains(episode, man.Oeuvres);
            Assert.NotEmpty(episode.Personnes);
            Assert.NotEmpty(episode.ListeStream);
        }

        [Fact]
        public void TestManager_CreerLeafNone()
        {
            var man = new Manager();

            var date = new DateTime(1985, 04, 15);

            man.CreerUser("Admin", "Admin", out var user);
            user.IsAdmin = true;

            man.CreerUser("Ambi", "Satan", out var user2);

            var film = man.CreerLeaf(user, "Film", "Fight Club", date, "image", "synopsis", Themes.Action, false);

            film = man.CreerLeaf(user, "Film", "Fight Club", date, "image", "synopsis", Themes.Action, false);

            Assert.Null(film);
            Assert.DoesNotContain(film, man.Oeuvres);

            man.SupprimerOeuvre(user, film);

            film = man.CreerLeaf(user, "Boudin", "Fight Club", date, "image", "synopsis", Themes.Action, false);

            Assert.Null(film);
            Assert.DoesNotContain(film, man.Oeuvres);

            man.SupprimerOeuvre(user, film);

            film = man.CreerLeaf(user2, "Film", "Fight Club", date, "image", "synopsis", Themes.Action, false);

            Assert.Null(film);
            Assert.DoesNotContain(film, man.Oeuvres);
        }

        [Fact]
        public void TestManager_ModifLeaf()
        {
            var man = new Manager();

            var date = new DateTime(1985, 04, 15);

            var date2 = new DateTime(1975, 12, 01); 

            man.CreerUser("Admin", "Admin", out var user);
            user.IsAdmin = true;

            man.CreerUser("Ambi", "Satan", out var user2);

            var pers = man.CreerPersonne(user, "Brad", "Pitt", "oui", "Américain", "image", date);

            var listeP = new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>
                {["acteur"] = new Dictionary<Personne, string> {[pers] = "Michou"}};

            var stream = new Manager().AjouterStreamLeaf("Fight Club",
                    new Dictionary<Plateformes, string> {[Netflix] = "netflix.com", [PrimeVideo] = "primevideo.com"})
                .ToList();

            var film = man.CreerLeaf(user, "Film", "Fight Club", date, "image", "synopsis", Themes.Action, true);

            user.AjouterEnvie(film);
            user.AjouterConsulte(film);
            man.AjouterAvis(user, film, 4.5f, "okkk");

            user2.ChangerProfil("Ambi", "image", true, new List<Plateformes>());
            user2.AjouterEnvie(film);
            user2.AjouterConsulte(film);
            man.AjouterAvis(user2, film, 4f, "noo");

            man.ModifierLeaf(user, film, "Fight", date2, "lien", "syno", Aventure, false, listeP, stream);
            
            Assert.Equal("Fight", film.Titre);
            Assert.Equal(date2, film.DateDeSortie);
            Assert.Equal("lien", film.LienImage);
            Assert.Equal("syno", film.Synopsis);
            Assert.Equal(Aventure, film.Theme);
            Assert.False(film.IsFamilleF);
            Assert.Contains("acteur", film.Personnes.Keys);
            Assert.Contains(pers, film.Personnes["acteur"].Keys);
            Assert.Contains("Michou", film.Personnes["acteur"][pers]);
            Assert.Contains(stream[0], film.ListeStream);
            Assert.Contains(stream[1], film.ListeStream);

            var dic = film.ListeAvis.ToDictionary(pair => pair.Key, pair => pair.Value );
            Assert.Contains(user, dic.Keys);
            Assert.DoesNotContain(user2, dic.Keys);

            Assert.Contains(film, user.ListeEnvie);
            Assert.Contains(film, user.RecemmentConsulte);
            Assert.DoesNotContain(film, user2.ListeEnvie);
            Assert.DoesNotContain(film, user2.RecemmentConsulte);


            man.ModifierLeaf(user2, film, "Fight Club", date, "image", "synopsis", Themes.Action, true,
                new List<KeyValuePair<string, IEnumerable<KeyValuePair<Personne, string>>>>(), new List<Streaming>());

            Assert.NotEqual("Fight Club", film.Titre);
            Assert.NotEqual(date, film.DateDeSortie);
            Assert.NotEqual("image", film.LienImage);
            Assert.NotEqual("synopsis", film.Synopsis);
            Assert.False(film.IsFamilleF);
            Assert.NotEqual(Themes.Action, film.Theme);
            Assert.NotEmpty(film.Personnes);
            Assert.NotEmpty(film.ListeStream);
        }
        
        [Fact]
        public void TestManager_CreerCompositeTrilogie()
        {
            var man = new Manager();

            var date = new DateTime(1985, 04, 15);

            man.CreerUser("Admin", "Admin", out var user);
            user.IsAdmin = true;

            man.CreerUser("Ambi", "Satan", out _);

            var pers = man.CreerPersonne(user, "Brad", "Pitt", "oui", "Américain", "image", date);

            var listeP = new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>
            { ["acteur"] = new Dictionary<Personne, string> { [pers] = "Michou" } };

            var film = man.CreerLeaf(user, "Film", "Fight Club", date, "image", "synopsis", Themes.Action, false);
            var film2 = man.CreerLeaf(user, "Film", "Fight Club2", date, "image", "synopsis", Themes.Action, false);
            var film3 = man.CreerLeaf(user, "Film", "Fight Club3", date, "image", "synopsis", Themes.Action, false);

            var trilogie = man.CreerComposite(user, "Trilogie", "Fight Club", date, "image", "synopsis", Themes.Action, false,
                new List<Oeuvre> {film, film2, film3});

            Assert.NotNull(trilogie);
            Assert.Contains(trilogie, man.Oeuvres);
            Assert.Contains(film, trilogie.ReOeuvres);
            Assert.Contains(film2, trilogie.ReOeuvres);
            Assert.Contains(film3, trilogie.ReOeuvres);

            man.SupprimerOeuvre(user, trilogie);

            trilogie = man.CreerComposite(user, "Trilogie", "Fight Club", date, "image", "synopsis", Themes.Action, false,
                new List<Oeuvre> {film, film2, film3}, listeP);

            Assert.NotNull(trilogie);
            Assert.Contains(trilogie, man.Oeuvres);
            Assert.Contains("acteur", trilogie.Personnes.Keys);
            Assert.Contains(pers, trilogie.Personnes["acteur"].Keys);
            Assert.Contains("Michou", trilogie.Personnes["acteur"][pers]);
            Assert.Contains(film, trilogie.ReOeuvres);
            Assert.Contains(film2, trilogie.ReOeuvres);
            Assert.Contains(film3, trilogie.ReOeuvres);
        }

        [Fact]
        public void TestManager_CreerCompositeSerie()
        {
            var man = new Manager();

            var date = new DateTime(1985, 04, 15);

            man.CreerUser("Admin", "Admin", out var user);
            user.IsAdmin = true;

            man.CreerUser("Ambi", "Satan", out _);

            var pers = man.CreerPersonne(user, "Brad", "Pitt", "oui", "Américain", "image", date);

            var listeP = new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>
            { ["acteur"] = new Dictionary<Personne, string> { [pers] = "Michou" } };

            var episode = man.CreerLeaf(user, "Épisode", "Fight Club3", date, "image", "synopsis", Themes.Action,
                false);

            var serie = man.CreerComposite(user, "Série", "Fight Club", date, "image", "synopsis", Themes.Action, false,
            new List<Oeuvre> { episode });

            Assert.NotNull(serie);
            Assert.Contains(serie, man.Oeuvres);
            Assert.DoesNotContain(episode, serie.ReOeuvres);

            man.SupprimerOeuvre(user, serie);

            serie = man.CreerComposite(user, "Série", "Fight Club", date, "image", "synopsis", Themes.Action, false,
                new List<Oeuvre> { episode }, listeP);

            Assert.NotNull(serie);
            Assert.Contains(serie, man.Oeuvres);
            Assert.Contains("acteur", serie.Personnes.Keys);
            Assert.Contains(pers, serie.Personnes["acteur"].Keys);
            Assert.Contains("Michou", serie.Personnes["acteur"][pers]);
            Assert.DoesNotContain(episode, serie.ReOeuvres);
        }

        [Fact]
        public void TestManager_CreerCompositeUnivers()
        {
            var man = new Manager();

            var date = new DateTime(1985, 04, 15);

            man.CreerUser("Admin", "Admin", out var user);
            user.IsAdmin = true;

            man.CreerUser("Ambi", "Satan", out _);

            var pers = man.CreerPersonne(user, "Brad", "Pitt", "oui", "Américain", "image", date);

            var listeP = new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>
            { ["acteur"] = new Dictionary<Personne, string> { [pers] = "Michou" } };

            var film = man.CreerLeaf(user, "Film", "Fight Club", date, "image", "synopsis", Themes.Action, false);
            var film2 = man.CreerLeaf(user, "Film", "Fight Club2", date, "image", "synopsis", Themes.Action, false);
            var film3 = man.CreerLeaf(user, "Film", "Fight Club3", date, "image", "synopsis", Themes.Action, false);
            var episode = man.CreerLeaf(user, "Épisode", "Fight Club3", date, "image", "synopsis", Themes.Action,
                false);

            var trilogie = man.CreerComposite(user, "Trilogie", "Fight Club", date, "image", "synopsis", Themes.Action, false,
                new List<Oeuvre> { film, film2, film3 });

            var serie = man.CreerComposite(user, "Série", "Fight Club", date, "image", "synopsis", Themes.Action, false,
            new List<Oeuvre> { episode });

            var univers = man.CreerComposite(user, "Univers", "Fight Club", date, "image", "synopsis", Themes.Action, false,
            new List<Oeuvre> { trilogie, serie });

            Assert.NotNull(univers);
            Assert.Contains(univers, man.Oeuvres);
            Assert.Contains(trilogie, univers.ReOeuvres);
            Assert.Contains(serie, univers.ReOeuvres);

            man.SupprimerOeuvre(user, univers);

            univers = man.CreerComposite(user, "Univers", "Fight Club", date, "image", "synopsis", Themes.Action, false,
                new List<Oeuvre> { trilogie, serie }, listeP);

            Assert.NotNull(univers);
            Assert.Contains(univers, man.Oeuvres);
            Assert.Contains("acteur", univers.Personnes.Keys);
            Assert.Contains(pers, univers.Personnes["acteur"].Keys);
            Assert.Contains("Michou", univers.Personnes["acteur"][pers]);
            Assert.Contains(trilogie, univers.ReOeuvres);
            Assert.Contains(serie, univers.ReOeuvres);
        }

        [Fact]
        public void TestManager_CreerCompositeNone()
        {
            var man = new Manager();

            var date = new DateTime(1985, 04, 15);

            man.CreerUser("Admin", "Admin", out var user);
            user.IsAdmin = true;

            man.CreerUser("Ambi", "Satan", out var user2);

            var episode = man.CreerLeaf(user, "Épisode", "Fight Club3", date, "image", "synopsis", Themes.Action,
                false);
           

            var serie = man.CreerComposite(user, "Serie", "Fight Club", date, "image", "synopsis", Themes.Action, false,
                new List<Oeuvre> { episode });

            Assert.Null(serie);
            Assert.DoesNotContain(serie, man.Oeuvres);

            man.SupprimerOeuvre(user, serie);

            serie = man.CreerComposite(user, "Boudin", "Fight Club", date, "image", "synopsis", Themes.Action, false,
                new List<Oeuvre> { episode });

            Assert.Null(serie);
            Assert.DoesNotContain(serie, man.Oeuvres);

            man.SupprimerOeuvre(user, serie);

            serie = man.CreerComposite(user2, "Serie", "Fight Club", date, "image", "synopsis", Themes.Action, false,
                new List<Oeuvre> { episode });

            Assert.Null(serie);
            Assert.DoesNotContain(serie, man.Oeuvres);
        }

        [Fact]
        public void TestManager_ModifComposite()
        {
            var man = new Manager();

            var date = new DateTime(1985, 04, 15);

            var date2 = new DateTime(1975, 12, 01);

            man.CreerUser("Admin", "Admin", out var user);
            user.IsAdmin = true;

            man.CreerUser("Ambi", "Satan", out var user2);

            var pers = man.CreerPersonne(user, "Brad", "Pitt", "oui", "Américain", "image", date);

            var listeP = new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>
            { ["acteur"] = new Dictionary<Personne, string> { [pers] = "Michou" } };

            var episode = man.CreerLeaf(user, "Épisode", "Fight Club", date, "image", "synopsis", Themes.Action, true);

            var serie = man.CreerComposite(user, "Série", "Fight Club", date, "image", "synopsis", Themes.Action, true,
                new List<Oeuvre> {episode});

            user.AjouterEnvie(serie);
            user.AjouterConsulte(serie);

            user2.ChangerProfil("Ambi", "image", true, new List<Plateformes>());
            user2.AjouterEnvie(serie);
            user2.AjouterConsulte(serie);

            man.ModifierComposite(user, serie, "Fight", date2, "lien", "syno", Aventure, false, listeP);

            Assert.Equal("Fight", serie.Titre);
            Assert.Equal(date2, serie.DateDeSortie);
            Assert.Equal("lien", serie.LienImage);
            Assert.Equal("syno", serie.Synopsis);
            Assert.Equal(Aventure, serie.Theme);
            Assert.False(serie.IsFamilleF);
            Assert.Contains("acteur", serie.Personnes.Keys);
            Assert.Contains(pers, serie.Personnes["acteur"].Keys);
            Assert.Contains("Michou", serie.Personnes["acteur"][pers]);

            Assert.Contains(serie, user.ListeEnvie);
            Assert.Contains(serie, user.RecemmentConsulte);
            Assert.DoesNotContain(serie, user2.ListeEnvie);
            Assert.DoesNotContain(serie, user2.RecemmentConsulte);

            man.ModifierComposite(user2, serie, "Fight Club", date, "image", "synopsis", Themes.Action, false,
                new List<KeyValuePair<string, IEnumerable<KeyValuePair<Personne, string>>>>());

            Assert.NotEqual("Fight Club", serie.Titre);
            Assert.NotEqual(date, serie.DateDeSortie);
            Assert.NotEqual("image", serie.LienImage);
            Assert.NotEqual("synopsis", serie.Synopsis);
            Assert.NotEqual(Themes.Action, serie.Theme);
            Assert.False(serie.IsFamilleF);
            Assert.NotEmpty(serie.Personnes);
        }

        [Fact]
        public void TestManager_AjouterCompOeuvre()
        {
            var man = new Manager();

            var date = new DateTime(1985, 04, 15);

            man.CreerUser("Admin", "Admin", out var user);
            user.IsAdmin = true;

            man.CreerUser("Ambi", "Satan", out var user2);

            var episode = man.CreerLeaf(user, "Épisode", "Fight Club", date, "image", "synopsis", Themes.Action, false);
            var episode2 = man.CreerLeaf(user, "Épisode", "Fight Club2", date, "image", "synopsis", Themes.Action, false);
            var episode3 = man.CreerLeaf(user, "Épisode", "Fight Club3", date, "image", "synopsis", Themes.Action, false);

            var serie = man.CreerComposite(user, "Série", "Fight Club", date, "image", "synopsis", Themes.Action, false,
                new List<Oeuvre> {episode});

            Manager.AjouterOeuvreComp(user, serie, episode2);

            Assert.Contains(episode2, serie.ReOeuvres);

            Manager.AjouterOeuvreComp(user2, serie, episode3);

            Assert.DoesNotContain(episode3, serie.ReOeuvres);
        }

        [Fact]
        public void TestManager_HasOeuvre()
        {
            var man = new Manager();

            var date = new DateTime(1985, 04, 15);

            man.CreerUser("Admin", "Admin", out var user);
            user.IsAdmin = true;

            var film = man.CreerLeaf(user, "Film", "Fight Club", date, "image", "synopsis", Themes.Action, false);

            Assert.False(man.HasOeuvre("Film", "Fight Club", date));

            Assert.True(man.HasOeuvre("Film", "Fight Club", new DateTime()));
            Assert.True(man.HasOeuvre("Film", "Fight", date));

            Assert.True(man.HasOeuvre("Épisode", "Fight Club", date));
            Assert.True(man.HasOeuvre("Trilogie", "Fight Club", date));
            Assert.True(man.HasOeuvre("Série", "Fight Club", date));
            Assert.True(man.HasOeuvre("Univers", "Fight Club", date));
        }

        [Fact]
        public void TestManager_RetirerCompOeuvre()
        {
            var man = new Manager();

            var date = new DateTime(1985, 04, 15);

            man.CreerUser("Admin", "Admin", out var user);
            user.IsAdmin = true;

            man.CreerUser("Ambi", "Satan", out var user2);


            var episode = man.CreerLeaf(user, "Épisode", "Fight Club", date, "image", "synopsis", Themes.Action, false);
            var episode2 = man.CreerLeaf(user, "Épisode", "Fight Club2", date, "image", "synopsis", Themes.Action, false);

            var serie = man.CreerComposite(user, "Série", "Fight Club", date, "image", "synopsis", Themes.Action, false,
                new List<Oeuvre> {episode});

            man.RetirerOeuvreComp(user, serie, episode);

            Assert.DoesNotContain(episode, serie.ReOeuvres);

            Manager.AjouterOeuvreComp(user, serie, episode2);
            man.RetirerOeuvreComp(user2, serie, episode2);

            Assert.Contains(episode2, serie.ReOeuvres);
        }

        [Fact]
        public void TestManager_SupprimerOeuvreTrilogie()
        {
            var man = new Manager();

            var date = new DateTime(1985, 04, 15);

            man.CreerUser("Admin", "Admin", out var user);
            user.IsAdmin = true;

            man.CreerUser("Ambi", "Satan", out _);

            var pers = man.CreerPersonne(user, "Brad", "Pitt", "oui", "Américain", "image", date);

            var listeP = new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>
                { ["acteur"] = new Dictionary<Personne, string> { [pers] = "Michou" } };

            var film = man.CreerLeaf(user, "Film", "Fight Club", date, "image", "synopsis", Themes.Action, false);
            var film2 = man.CreerLeaf(user, "Film", "Fight Club2", date, "image", "synopsis", Themes.Action, false, listeP);
            var film3 = man.CreerLeaf(user, "Film", "Fight Club3", date, "image", "synopsis", Themes.Action, false, listeP);

            var trilogie = man.CreerComposite(user, "Trilogie", "Fight Club", date, "image", "synopsis", Themes.Action, false,
                new List<Oeuvre> {film, film2, film3});

            Manager.AjouterALaListeEnvies(user, film3);

            man.SupprimerOeuvre(user, trilogie);

            Assert.DoesNotContain(trilogie, man.Oeuvres);
            Assert.Contains(film, man.Oeuvres);
            Assert.Contains(film2, man.Oeuvres);
            Assert.Contains(film3, man.Oeuvres);

            trilogie = man.CreerComposite(user, "Trilogie", "Fight Club", date, "image", "synopsis", Themes.Action, false, 
                new List<Oeuvre> {film, film2, film3});

            man.SupprimerOeuvre(user, film);

            Assert.DoesNotContain(film, man.Oeuvres);
            Assert.DoesNotContain(trilogie, man.Oeuvres);
        }

        [Fact]
        public void TestManager_SupprimerOeuvreSerie()
        {
            var man = new Manager();

            var date = new DateTime(1985, 04, 15);

            man.CreerUser("Admin", "Admin", out var user);
            user.IsAdmin = true;

            man.CreerUser("Ambi", "Satan", out _);

            var pers = man.CreerPersonne(user, "Brad", "Pitt", "oui", "Américain", "image", date);

            var episode = man.CreerLeaf(user, "Épisode", "Fight Club", date, "image", "synopsis", Themes.Action, false);
            var episode2 = man.CreerLeaf(user, "Épisode", "Fight Club2", date, "image", "synopsis", Themes.Action, false);

            var serie = man.CreerComposite(user, "Série", "Fight Club", date, "image", "synopsis", Themes.Action, false,
                new List<Oeuvre> { episode });
            Manager.AjouterOeuvreComp(user, serie, episode);

            man.SupprimerOeuvre(user, serie);

            Assert.DoesNotContain(serie, man.Oeuvres);
            Assert.DoesNotContain(episode, man.Oeuvres);

            episode = man.CreerLeaf(user, "Épisode", "Fight Club", date, "image", "synopsis", Themes.Action, false);

            serie = man.CreerComposite(user, "Série", "Fight Club", date, "image", "synopsis", Themes.Action, false,
                new List<Oeuvre> { episode, episode2 });

            Manager.AjouterOeuvreComp(user, serie, episode);
            Manager.AjouterOeuvreComp(user, serie, episode2);

            man.SupprimerOeuvre(user, episode);

            Assert.DoesNotContain(episode, man.Oeuvres);
            Assert.Contains(serie, man.Oeuvres);
            Assert.DoesNotContain(episode, serie.ReOeuvres);

            man.SupprimerOeuvre(user, episode2);

            Assert.DoesNotContain(episode2, man.Oeuvres);
            Assert.DoesNotContain(serie, man.Oeuvres);
            Assert.Contains(pers, man.Personnes);
        }

        [Fact]
        public void TestManager_SupprimerOeuvreUnivers()
        {
            var man = new Manager();

            var date = new DateTime(1985, 04, 15);

            man.CreerUser("Admin", "Admin", out var user);
            user.IsAdmin = true;

            man.CreerUser("Ambi", "Satan", out _);

            var pers = man.CreerPersonne(user, "Brad", "Pitt", "oui", "Américain", "image", date);

            var listeP = new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>
            { ["acteur"] = new Dictionary<Personne, string> { [pers] = "Michou" } };

            var film = man.CreerLeaf(user, "Film", "Fight Club", date, "image", "synopsis", Themes.Action, false);
            var film2 = man.CreerLeaf(user, "Film", "Fight Club2", date, "image", "synopsis", Themes.Action, false, listeP);
            var film3 = man.CreerLeaf(user, "Film", "Fight Club3", date, "image", "synopsis", Themes.Action, false, listeP);
            var episode = man.CreerLeaf(user, "Épisode", "Fight Club", date, "image", "synopsis", Themes.Action, false);

            var trilogie = man.CreerComposite(user, "Trilogie", "Fight Club", date, "image", "synopsis", Themes.Action, false,
                new List<Oeuvre> { film, film2, film3 });

            var serie = man.CreerComposite(user, "Série", "Fight Club", date, "image", "synopsis", Themes.Action, false,
                new List<Oeuvre> { episode });
            Manager.AjouterOeuvreComp(user, serie, episode);

            var univers = man.CreerComposite(user, "Univers", "Fight Club", date, "image", "synopsis", Themes.Action, false,
                new List<Oeuvre> { trilogie, serie });

            man.SupprimerOeuvre(user, serie);

            Assert.Contains(univers, man.Oeuvres);
            Assert.DoesNotContain(serie, univers.ReOeuvres);

            man.SupprimerOeuvre(user, trilogie);

            Assert.DoesNotContain(univers, man.Oeuvres);
        }
    }
}