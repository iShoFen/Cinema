using Modele;
using System;
using System.Collections.Generic;
using Xunit;

namespace UnitTests
{
    public class UnitTestUnivers
    {
        [Fact]
        public void TestUnivers_AjouterOeuvres()
        {
            var date = new DateTime(1985, 10, 5);

            var oFilm = new Film("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Streaming>());

            var oEp = new Episode("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Streaming>());

            var oTri = new Trilogie("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Oeuvre>());

            var oSerie = new Serie("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>());

            var oUni = new Univers("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Oeuvre>());

            oUni.AjouterOeuvres(new List<Oeuvre>{oFilm, oEp, oTri, oSerie});

            Assert.DoesNotContain(oEp, oUni.ReOeuvres);
            Assert.Contains(oFilm, oUni.ReOeuvres);
            Assert.Contains(oTri, oUni.ReOeuvres);
            Assert.Contains(oSerie, oUni.ReOeuvres);

            oUni.AjouterOeuvres(new List<Oeuvre>{oFilm, oEp, oTri, oSerie});

            Assert.Equal(3, oUni.ReOeuvres.Count);
            Assert.DoesNotContain(oEp, oUni.ReOeuvres);
            Assert.Contains(oFilm, oUni.ReOeuvres);
            Assert.Contains(oTri, oUni.ReOeuvres);
            Assert.Contains(oSerie, oUni.ReOeuvres);
        }

        [Fact]
        public void TestUnivers_EqualsHashCode()
        {
            var univers = new Univers("Fight Club", new DateTime(1999, 11, 10), "Images/Photos/FC.jpg",
                "Description film...", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Oeuvre>());

            var univers2 = new Univers("Fight Club", new DateTime(1999, 11, 10), "image",
                "blabla", Themes.Aventure, false, new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(),
                new List<Oeuvre>());

            var univers3 = new Univers("Fight", new DateTime(1985, 10, 01), "Images/Photos/FC.jpg",
                "Description film...", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Oeuvre>());

            Assert.False(univers.Equals((object) null));
            Assert.True(univers.Equals((object) univers));
            Assert.False(univers.Equals("test"));
            Assert.True(univers.Equals((object) univers2));
            Assert.False(univers.Equals(univers3));

            Assert.Equal(univers2.GetHashCode(), univers.GetHashCode());
            Assert.NotEqual(univers3.GetHashCode(), univers.GetHashCode());
        }
    }
}
