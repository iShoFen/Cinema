using System;
using System.Collections.Generic;
using Modele;
using Xunit;

namespace UnitTests
{
    public class UnitTestSerie
    {
        [Fact]
        public void TestSerie_AjouterOeuvres()
        {
            var date = new DateTime(1985, 10, 5);

            var oFilm = new Film("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Streaming>());

            var oEp = new Episode("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Streaming>());

            var oSerie = new Serie("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>());

            oSerie.AjouterOeuvres(new List<Oeuvre>{oFilm, oEp});

            Assert.Single(oSerie.ReOeuvres);
            Assert.Contains(oEp, oSerie.ReOeuvres);

            oSerie.AjouterOeuvres(new List<Oeuvre>{oEp});

            Assert.Single(oSerie.ReOeuvres);
            Assert.Contains(oEp, oSerie.ReOeuvres);
        }

        [Fact]
        public void TestSerie_EqualsHashCode()
        {
            var serie = new Serie("Fight Club", new DateTime(1999, 11, 10), "Images/Photos/FC.jpg",
                "Description film...", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>());

            var serie2 = new Serie("Fight Club", new DateTime(1999, 11, 10), "image",
                "blabla", Themes.Aventure, false, new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>());

            var serie3 = new Serie("Fight", new DateTime(1985, 10, 01), "Images/Photos/FC.jpg",
                "Description film...", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>());

            Assert.False(serie.Equals((object) null));
            Assert.True(serie.Equals((object) serie));
            Assert.False(serie.Equals("test"));
            Assert.True(serie.Equals((object) serie2));
            Assert.False(serie.Equals(serie3));

            Assert.Equal(serie2.GetHashCode(), serie.GetHashCode());
            Assert.NotEqual(serie3.GetHashCode(), serie.GetHashCode());
        }
    }
}