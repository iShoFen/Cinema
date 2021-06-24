using System;
using System.Collections.Generic;
using Modele;
using Xunit;

namespace UnitTests
{
    public class UnitTestTrilogie
    {
        [Fact]
        public void TestTrilogie_AjouterOeuvres()
        {
            var date = new DateTime(1985, 10, 5);

            var oFilm = new Film("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Streaming>());

            var oFilm2 = new Film("film", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Streaming>());

            var oFilm3 = new Film("film2", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Streaming>());

            var oep = new Episode("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Streaming>());

            var oTri = new Trilogie("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Oeuvre>());

            oTri.AjouterOeuvres(new List<Oeuvre>{oFilm, oep, oFilm2});

            Assert.Empty(oTri.ReOeuvres);

            oTri.AjouterOeuvres(new List<Oeuvre>{oFilm, oFilm2, oFilm3});

            Assert.Contains(oFilm, oTri.ReOeuvres);
            Assert.Contains(oFilm2, oTri.ReOeuvres);
            Assert.Contains(oFilm3, oTri.ReOeuvres);

            oTri.AjouterOeuvres(new List<Oeuvre>{oFilm, oFilm2, oFilm3});

            Assert.Equal(3, oTri.ReOeuvres.Count);
            Assert.Contains(oFilm, oTri.ReOeuvres);
            Assert.Contains(oFilm2, oTri.ReOeuvres);
            Assert.Contains(oFilm3, oTri.ReOeuvres);
        }

        [Fact]
        public void TestTrilogie_EqualsHashCode()
        {
            var trilo = new Trilogie("Fight Club", new DateTime(1999, 11, 10), "Images/Photos/FC.jpg",
                "Description film...", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Oeuvre>());

            var trilo2 = new Trilogie("Fight Club", new DateTime(1999, 11, 10), "image",
                "blabla", Themes.Aventure, false, new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(),
                new List<Oeuvre>());

            var trilo3 = new Trilogie("Fight", new DateTime(1985, 10, 01), "Images/Photos/FC.jpg",
                "Description film...", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Oeuvre>());

            Assert.False(trilo.Equals((object) null));
            Assert.True(trilo.Equals((object) trilo));
            Assert.False(trilo.Equals("test"));
            Assert.True(trilo.Equals((object) trilo2));
            Assert.False(trilo.Equals(trilo3));

            Assert.Equal(trilo2.GetHashCode(), trilo.GetHashCode());
            Assert.NotEqual(trilo3.GetHashCode(), trilo.GetHashCode());
        }
    }
}
