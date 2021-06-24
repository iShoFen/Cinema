using System;
using System.Collections.Generic;
using System.Linq;
using Modele;
using Xunit;

namespace UnitTests
{
    public class UnitTestOeuvre
    {
        [Fact]
        public void TestOeuvreFilm()
        {
            var date = new DateTime(1985, 10, 5);

            var avis = new Avis(4.5f, "bla");
            var avis2 = new Avis(3.5f, "noo");

            var user = new User("Michel", "1234");
            var user2 = new User("Mich", "1234");


            var oFilm = new Film("oeuvre", date, "image", "syn", Themes.Action, false);

            oFilm.AjouterAvis(user, avis);
            oFilm.AjouterAvis(user2, avis2);

            Assert.Equal("oeuvre", oFilm.Titre);
            Assert.Equal(date, oFilm.DateDeSortie);
            Assert.Equal("image", oFilm.LienImage);
            Assert.Equal("syn", oFilm.Synopsis);
            Assert.Equal(Themes.Action, oFilm.Theme);
            Assert.Equal(4f, oFilm.NoteMoyenne);
            Assert.False(oFilm.IsFamilleF);
            Assert.NotNull(oFilm.Personnes);
            Assert.NotNull(oFilm.ListeAvis);
            Assert.NotNull(oFilm.ListeStream);
        }

        [Fact]
        public void TestOeuvreEpisode()
        {
            var date = new DateTime(1985, 10, 5);

            var avis = new Avis(4.5f, "bla");
            var avis2 = new Avis(3.5f, "noo");

            var user = new User("Michel", "1234");
            var user2 = new User("Mich", "1234");


            var oEpisode = new Episode("oeuvre", date, "image", "syn", Themes.Action, false);

            oEpisode.AjouterAvis(user, avis);
            oEpisode.AjouterAvis(user2, avis2);

            Assert.Equal("oeuvre", oEpisode.Titre);
            Assert.Equal(date, oEpisode.DateDeSortie);
            Assert.Equal("image", oEpisode.LienImage);
            Assert.Equal("syn", oEpisode.Synopsis);
            Assert.Equal(Themes.Action, oEpisode.Theme);
            Assert.Equal(4f, oEpisode.NoteMoyenne);
            Assert.False(oEpisode.IsFamilleF);
            Assert.NotNull(oEpisode.Personnes);
            Assert.NotNull(oEpisode.ListeAvis);
            Assert.NotNull(oEpisode.ListeStream);
        }

        [Fact]
        public void TestOeuvreTrilogie()
        {
            var date = new DateTime(1985, 10, 5);

            var avis = new Avis(4.5f, "bla");
            var avis2 = new Avis(3.5f, "noo");

            var user = new User("Michel", "1234");
            var user2 = new User("Mich", "1234");


            var oFilm = new Film("oeuvre", date, "image", "syn", Themes.Action, false);
            var oTrilogie = new Trilogie("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Oeuvre> { oFilm });

            oTrilogie.AjouterAvis(user, avis);
            oTrilogie.AjouterAvis(user2, avis2);

            Assert.Equal("oeuvre", oTrilogie.Titre);
            Assert.Equal(date, oTrilogie.DateDeSortie);
            Assert.Equal("image", oTrilogie.LienImage);
            Assert.Equal("syn", oTrilogie.Synopsis);
            Assert.Equal(Themes.Action, oTrilogie.Theme);
            Assert.Equal(4f, oTrilogie.NoteMoyenne);
            Assert.False(oTrilogie.IsFamilleF);
            Assert.NotNull(oTrilogie.Personnes);
            Assert.NotNull(oTrilogie.ListeAvis);
            Assert.NotNull(oTrilogie.ReOeuvres);
        }

        [Fact]
        public void TestOeuvreSerie()
        {
            var date = new DateTime(1985, 10, 5);

            var avis = new Avis(4.5f, "bla");
            var avis2 = new Avis(3.5f, "noo");

            var user = new User("Michel", "1234");
            var user2 = new User("Mich", "1234");

            var oSerie = new Serie("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>());

            oSerie.AjouterAvis(user, avis);
            oSerie.AjouterAvis(user2, avis2);

            Assert.Equal("oeuvre", oSerie.Titre);
            Assert.Equal(date, oSerie.DateDeSortie);
            Assert.Equal("image", oSerie.LienImage);
            Assert.Equal("syn", oSerie.Synopsis);
            Assert.Equal(Themes.Action, oSerie.Theme);
            Assert.Equal(4f, oSerie.NoteMoyenne);
            Assert.False(oSerie.IsFamilleF);
            Assert.NotNull(oSerie.Personnes);
            Assert.NotNull(oSerie.ListeAvis);
            Assert.NotNull(oSerie.ReOeuvres);
        }

        [Fact]
        public void TestOeuvreUnivers()
        {
            var date = new DateTime(1985, 10, 5);

            var avis = new Avis(4.5f, "bla");
            var avis2 = new Avis(3.5f, "noo");

            var user = new User("Michel", "1234");
            var user2 = new User("Mich", "1234");


            var oFilm = new Film("oeuvre", date, "image", "syn", Themes.Action, false);
            var oUnivers = new Univers("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Oeuvre> { oFilm });

            oUnivers.AjouterAvis(user, avis);
            oUnivers.AjouterAvis(user2, avis2);

            Assert.Equal("oeuvre", oUnivers.Titre);
            Assert.Equal(date, oUnivers.DateDeSortie);
            Assert.Equal("image", oUnivers.LienImage);
            Assert.Equal("syn", oUnivers.Synopsis);
            Assert.Equal(Themes.Action, oUnivers.Theme);
            Assert.Equal(4f, oUnivers.NoteMoyenne);
            Assert.False(oUnivers.IsFamilleF);
            Assert.NotNull(oUnivers.Personnes);
            Assert.NotNull(oUnivers.ListeAvis);
            Assert.NotNull(oUnivers.ReOeuvres);
        }

        [Fact]
        public void TestOeuvre_AjouterPersonne()
        {
            var date = new DateTime(1985, 10, 5);
            var pers1 = new Personne("mich", "mouch", "la vie", "un pays", "image", date);


            var oFilm = new Film("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>
                    {["acteur"] = new Dictionary<Personne, string> {[pers1] = "bla"}}, new List<Streaming>());

            var oEpisode = new Episode("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>
                    {["acteur"] = new Dictionary<Personne, string> {[pers1] = "bla"}}, new List<Streaming>());

            var oTrilogie = new Trilogie("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>
                    {["acteur"] = new Dictionary<Personne, string> {[pers1] = "bla"}}, new List<Oeuvre>());

            var oSerie = new Serie("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>
                    {["acteur"] = new Dictionary<Personne, string> {[pers1] = "bla"}});

            var oUnivers = new Univers("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>
                    {["acteur"] = new Dictionary<Personne, string> {[pers1] = "bla"}}, new List<Oeuvre>());

            Assert.True(oFilm.Personnes.ContainsKey("acteur"));
            Assert.Single(oFilm.Personnes["acteur"].Keys);

            Assert.True(oEpisode.Personnes.ContainsKey("acteur"));
            Assert.Single(oEpisode.Personnes["acteur"]);

            Assert.True(oTrilogie.Personnes.ContainsKey("acteur"));
            Assert.Single(oTrilogie.Personnes["acteur"]);

            Assert.True(oSerie.Personnes.ContainsKey("acteur"));
            Assert.Single(oSerie.Personnes["acteur"]);

            Assert.True(oUnivers.Personnes.ContainsKey("acteur"));
            Assert.Single(oUnivers.Personnes["acteur"]);
        }

        [Fact]
        public void TestOeuvre_ModifierPersonneFilm()
        {
            var date = new DateTime(1985, 10, 5);
            var pers1 = new Personne("mich", "mouch", "la vie", "un pays", "image", date);
            var pers2 = new Personne("mouch", "mich", "la vie", "un pays", "image", date);


            var oFilm = new Film("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>
                    {["acteur"] = new Dictionary<Personne, string> {[pers1] = "bla"}}, new List<Streaming>());

            oFilm.ModifierLeaf("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>
                    {["acteur"] = new Dictionary<Personne, string> {[pers2] = "bla"}}, new List<Streaming>());

            Assert.Single(oFilm.Personnes);
            Assert.Contains(pers2, oFilm.Personnes["acteur"].Keys);

            oFilm.ModifierLeaf("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>
                    {["realisateur"] = new Dictionary<Personne, string> {[pers2] = "bla"}}, new List<Streaming>());

            Assert.Single(oFilm.Personnes);
            Assert.Contains(pers2, oFilm.Personnes["realisateur"].Keys);
        }

        [Fact]
        public void TestOeuvre_ModifierPersonneEpisode()
        {
            var date = new DateTime(1985, 10, 5);
            var pers1 = new Personne("mich", "mouch", "la vie", "un pays", "image", date);
            var pers2 = new Personne("mouch", "mich", "la vie", "un pays", "image", date);

            var oEpisode = new Episode("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>
                { ["acteur"] = new Dictionary<Personne, string> { [pers1] = "bla" } }, new List<Streaming>());

            oEpisode.ModifierLeaf("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>
                { ["acteur"] = new Dictionary<Personne, string> { [pers2] = "bla" } }, new List<Streaming>());

            Assert.Single(oEpisode.Personnes);
            Assert.Contains(pers2, oEpisode.Personnes["acteur"].Keys);

            oEpisode.ModifierLeaf("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>
                { ["realisateur"] = new Dictionary<Personne, string> { [pers2] = "bla" } }, new List<Streaming>());

            Assert.Single(oEpisode.Personnes);
            Assert.Contains(pers2, oEpisode.Personnes["realisateur"].Keys);
        }

        [Fact]
        public void TestOeuvre_ModifierPersonneTrilogie()
        {
            var date = new DateTime(1985, 10, 5);
            var pers1 = new Personne("mich", "mouch", "la vie", "un pays", "image", date);
            var pers2 = new Personne("mouch", "mich", "la vie", "un pays", "image", date);

            var oTrilogie = new Trilogie("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>
                { ["acteur"] = new Dictionary<Personne, string> { [pers1] = "bla" } }, new List<Oeuvre>());

            oTrilogie.ModifierComposite("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>
                { ["acteur"] = new Dictionary<Personne, string> { [pers2] = "bla" } });

            Assert.Single(oTrilogie.Personnes);
            Assert.Contains(pers2, oTrilogie.Personnes["acteur"].Keys);

            oTrilogie.ModifierComposite("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>
                { ["realisateur"] = new Dictionary<Personne, string> { [pers2] = "bla" } });

            Assert.Single(oTrilogie.Personnes);
            Assert.Contains(pers2, oTrilogie.Personnes["realisateur"].Keys);
        }

        [Fact]
        public void TestOeuvre_ModifierPersonneSerie()
        {
            var date = new DateTime(1985, 10, 5);
            var pers1 = new Personne("mich", "mouch", "la vie", "un pays", "image", date);
            var pers2 = new Personne("mouch", "mich", "la vie", "un pays", "image", date);

            var oSerie = new Serie("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>
                { ["acteur"] = new Dictionary<Personne, string> { [pers1] = "bla" } });

            oSerie.ModifierComposite("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>
                { ["acteur"] = new Dictionary<Personne, string> { [pers2] = "bla" } });

            Assert.Single(oSerie.Personnes);
            Assert.Contains(pers2, oSerie.Personnes["acteur"].Keys);

            oSerie.ModifierComposite("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>
                { ["realisateur"] = new Dictionary<Personne, string> { [pers2] = "bla" } });

            Assert.Single(oSerie.Personnes);
            Assert.Contains(pers2, oSerie.Personnes["realisateur"].Keys);
        }

        [Fact]
        public void TestOeuvre_ModifierPersonneUnivers()
        {
            var date = new DateTime(1985, 10, 5);
            var pers1 = new Personne("mich", "mouch", "la vie", "un pays", "image", date);
            var pers2 = new Personne("mouch", "mich", "la vie", "un pays", "image", date);

            var oUnivers = new Univers("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>
                { ["acteur"] = new Dictionary<Personne, string> { [pers1] = "bla" } }, new List<Oeuvre>());

            oUnivers.ModifierComposite("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>()
                { ["acteur"] = new Dictionary<Personne, string> { [pers2] = "bla" } });

            Assert.Single(oUnivers.Personnes);
            Assert.Contains(pers2, oUnivers.Personnes["acteur"].Keys);

            oUnivers.ModifierComposite("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>
                { ["realisateur"] = new Dictionary<Personne, string> { [pers2] = "bla" } });

            Assert.Single(oUnivers.Personnes);
            Assert.Contains(pers2, oUnivers.Personnes["realisateur"].Keys);
        }

        [Fact]
        public void TestOeuvre_AjouterAvis()
        {
            var date = new DateTime(1985, 10, 5);
            var user = new User("anto", "1234");
            var avis = new Avis(4.5f, "blabla");
            var avis2 = new Avis(4f, "blublu");

            var oFilm = new Film("oeuvre", date, "image", "syn", Themes.Action, false);
            var oEpisode = new Episode("oeuvre", date, "image", "syn", Themes.Action, false);
            var oTrilogie = new Trilogie("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Oeuvre> {oFilm});

            var oSerie = new Serie("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>());

            var oUnivers = new Univers("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Oeuvre> {oFilm});

            oFilm.AjouterAvis(user, avis);
            oFilm.AjouterAvis(user, avis2);

            oEpisode.AjouterAvis(user, avis);
            oEpisode.AjouterAvis(user, avis2);

            oTrilogie.AjouterAvis(user, avis);
            oTrilogie.AjouterAvis(user, avis2);

            oSerie.AjouterAvis(user, avis);
            oSerie.AjouterAvis(user, avis2);

            oUnivers.AjouterAvis(user, avis);
            oUnivers.AjouterAvis(user, avis2);

            var dic = oFilm.ListeAvis.ToDictionary(pair => pair.Key, pair => pair.Value);
            var dic2 = oEpisode.ListeAvis.ToDictionary(pair => pair.Key, pair => pair.Value);
            var dic3 = oTrilogie.ListeAvis.ToDictionary(pair => pair.Key, pair => pair.Value);
            var dic4 = oSerie.ListeAvis.ToDictionary(pair => pair.Key, pair => pair.Value);
            var dic5 = oUnivers.ListeAvis.ToDictionary(pair => pair.Key, pair => pair.Value);

            Assert.Equal(avis, dic[user]);
            Assert.Equal(avis, dic2[user]);
            Assert.Equal(avis, dic3[user]);
            Assert.Equal(avis, dic4[user]);
            Assert.Equal(avis, dic5[user]);
        }

        [Fact]
        public void TestOeuvre_RetirerAvis()
        {
            var date = new DateTime(1985, 10, 5);
            var user = new User("anto", "1234");
            var avis = new Avis(4.5f, "blabla");

            var oFilm = new Film("oeuvre", date, "image", "syn", Themes.Action, false);
            var oEpisode = new Episode("oeuvre", date, "image", "syn", Themes.Action, false);
            var oTrilogie = new Trilogie("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Oeuvre> {oFilm});

            var oSerie = new Serie("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>());

            var oUnivers = new Univers("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Oeuvre> {oFilm});

            oFilm.AjouterAvis(user, avis);
            oFilm.RetirerAvis(user);

            oEpisode.AjouterAvis(user, avis);
            oEpisode.RetirerAvis(user);

            oTrilogie.AjouterAvis(user, avis);
            oTrilogie.RetirerAvis(user);

            oSerie.AjouterAvis(user, avis);
            oSerie.RetirerAvis(user);

            oUnivers.AjouterAvis(user, avis);
            oUnivers.RetirerAvis(user);

            Assert.Empty(oFilm.ListeAvis);
            Assert.Empty(oEpisode.ListeAvis);
            Assert.Empty(oTrilogie.ListeAvis);
            Assert.Empty(oSerie.ListeAvis);
            Assert.Empty(oUnivers.ListeAvis);
        }

        [Fact]
        public void TestOeuvre_AjouterOeuvres()
        {
            var date = new DateTime(1985, 10, 5);

            var oFilm = new Film("oeuvre", date, "image", "syn", Themes.Action, false);
            var oEpisode = new Episode("oeuvre", date, "image", "syn", Themes.Action, false);
            var oTrilogie = new Trilogie("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Oeuvre> {oFilm});

            var oSerie = new Serie("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>());

            var oUnivers = new Univers("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Oeuvre> {oFilm});

            Assert.Throws<NotImplementedException>(() => oFilm.AjouterOeuvres(new List<Oeuvre>()));
            Assert.Throws<NotImplementedException>(() => oEpisode.AjouterOeuvres(new List<Oeuvre>()));
            oTrilogie.AjouterOeuvres(new List<Oeuvre>());
            oSerie.AjouterOeuvres(new List<Oeuvre>());
            oUnivers.AjouterOeuvres(new List<Oeuvre>());
        }

        [Fact]
        public void TestOeuvre_RetirerOeuvre()
        {
            var date = new DateTime(1985, 10, 5);

            var oFilm = new Film("oeuvre", date, "image", "syn", Themes.Action, false);
            var oEpisode = new Episode("oeuvre", date, "image", "syn", Themes.Action, false);
            var oTrilogie = new Trilogie("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Oeuvre> {oFilm});

            var oSerie = new Serie("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>());

            var oUnivers = new Univers("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Oeuvre> {oFilm});

            Assert.Throws<NotImplementedException>(() => oFilm.RetirerOeuvre(oFilm));
            Assert.Throws<NotImplementedException>(() => oEpisode.RetirerOeuvre(oFilm));
            oTrilogie.RetirerOeuvre(oFilm);
            oSerie.RetirerOeuvre(oFilm);
            oUnivers.RetirerOeuvre(oFilm);
        }
    }
}