using System;
using System.Collections.Generic;
using Xunit;
using Modele;
using static Modele.Plateformes;

namespace UnitTests
{
    public class UnitTestLeaf
    {
        [Fact]
        public void TestLeaf_AjouterStreamingFilm()
        {
            var date = new DateTime(1985, 10, 5);
            var str = new Streaming("bla", "lien", PrimeVideo);
            var str2 = new Streaming("bla", "lien", Netflix);

            var oFilm = new Film("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Streaming> {str, str});

            Assert.Single(oFilm.ListeStream);
            Assert.Contains(str, oFilm.ListeStream);

            oFilm.ModifierLeaf("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Streaming> {str, str2});

            Assert.Equal(2, oFilm.ListeStream.Count);
            Assert.Contains(str, oFilm.ListeStream);
            Assert.Contains(str2, oFilm.ListeStream);

            oFilm.ModifierLeaf("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Streaming> {str2});

            Assert.Single(oFilm.ListeStream);
            Assert.DoesNotContain(str, oFilm.ListeStream);
            Assert.Contains(str2, oFilm.ListeStream);
        }

        [Fact]
        public void TestLeaf_AjouterStreamingEpisode()
        {
            var date = new DateTime(1985, 10, 5);
            var str = new Streaming("bla", "lien", PrimeVideo);
            var str2 = new Streaming("bla", "lien", Netflix);

            var oEpisode = new Episode("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Streaming> { str, str });

            Assert.Single(oEpisode.ListeStream);
            Assert.Contains(str, oEpisode.ListeStream);

            oEpisode.ModifierLeaf("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Streaming> { str, str2 });

            Assert.Equal(2, oEpisode.ListeStream.Count);
            Assert.Contains(str, oEpisode.ListeStream);
            Assert.Contains(str2, oEpisode.ListeStream);

            oEpisode.ModifierLeaf("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Streaming> { str2 });

            Assert.Single(oEpisode.ListeStream);
            Assert.DoesNotContain(str, oEpisode.ListeStream);
            Assert.Contains(str2, oEpisode.ListeStream);
        }

        [Fact]
        public void TestLeaf_ModifierLeafFilm()
        {
            var date = new DateTime(1985, 10, 5);
            var date2 = new DateTime(1975, 8, 2);

            var oFilm = new Film("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Streaming>());

            oFilm.ModifierLeaf("test", date2, "lien", "le synopsis", Themes.Aventure, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Streaming>());

            Assert.Equal("test", oFilm.Titre);
            Assert.Equal(date2, oFilm.DateDeSortie);
            Assert.Equal("lien", oFilm.LienImage);
            Assert.Equal("le synopsis", oFilm.Synopsis);
            Assert.Equal(Themes.Aventure, oFilm.Theme);
        }

        [Fact]
        public void TestLeaf_ModifierLeafEpisode()
        {
            var date = new DateTime(1985, 10, 5);
            var date2 = new DateTime(1975, 8, 2);

            var oEpisode = new Episode("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Streaming>());

            oEpisode.ModifierLeaf("test", date2, "lien", "le synopsis", Themes.Aventure, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Streaming>());

            Assert.Equal("test", oEpisode.Titre);
            Assert.Equal(date2, oEpisode.DateDeSortie);
            Assert.Equal("lien", oEpisode.LienImage);
            Assert.Equal("le synopsis", oEpisode.Synopsis);
            Assert.Equal(Themes.Aventure, oEpisode.Theme);
        }
    }
}
