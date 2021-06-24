using System;
using System.Collections.Generic;
using Modele;
using Xunit;

namespace UnitTests
{
    public class UnitTestPersonne
    {
        [Fact]
        public void TestPersonne()
        {
            var personne = new Personne("Pitt", "Brad", "blablablabla", "Américaine", "Images/Photos/brad.jpg", new DateTime(1963,12,18));
            
            Assert.Equal("Pitt", personne.Nom);
            Assert.Equal("Brad", personne.Prenom);
            Assert.Equal("blablablabla", personne.Biographie);
            Assert.Equal("Américaine", personne.Nationalite);
            Assert.Equal("Images/Photos/brad.jpg", personne.LienImage);
            Assert.Equal(new DateTime(1963,12,18), personne.DateDeNaissance);
        } 

        [Fact]
        public void TestPersonne_Modifier()
        { 
            var personne = new Personne("Pitt", "Brad", "blablablabla", "Américaine", "Images/Photos/brad.jpg", new DateTime(1963,12,18));

            personne.ModifierPersonne("Michel","Michel","il était une fois Michel","France","Michel.png",new DateTime(1999,01,01));

            Assert.Equal("Michel", personne.Nom);
            Assert.Equal("Michel", personne.Prenom);
            Assert.Equal("il était une fois Michel", personne.Biographie);
            Assert.Equal("France", personne.Nationalite);
            Assert.Equal("Michel.png", personne.LienImage);
            Assert.Equal(new DateTime(1999,01,01), personne.DateDeNaissance);
        }

        [Fact]
        public void TestPersonne_AjouterOeuvre()
        {
            var date = new DateTime(1985, 10, 5);

            var oFilm = new Film("oeuvre", date, "image", "syn", Themes.Action, false);
            var oEpisode = new Episode("oeuvre", date, "image", "syn", Themes.Action, false);
            var oTrilogie = new Trilogie("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Oeuvre>());

            var oSerie = new Serie("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>());

            var oUnivers = new Univers("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Oeuvre>());

            var pers = new Personne("brad", "broud", "il était une fois la vie", "la terre", "une image", date);

            pers.AjouterOeuvre(oFilm);
            pers.AjouterOeuvre(oEpisode);
            pers.AjouterOeuvre(oTrilogie);
            pers.AjouterOeuvre(oSerie);
            pers.AjouterOeuvre(oUnivers);

            Assert.Contains(oFilm, pers.Oeuvres);
            Assert.Contains(oEpisode, pers.Oeuvres);
            Assert.Contains(oTrilogie, pers.Oeuvres);
            Assert.Contains(oSerie, pers.Oeuvres);
            Assert.Contains(oUnivers, pers.Oeuvres);

            pers.AjouterOeuvre(oFilm);
            pers.AjouterOeuvre(oEpisode);
            pers.AjouterOeuvre(oTrilogie);
            pers.AjouterOeuvre(oSerie);
            pers.AjouterOeuvre(oUnivers);

            Assert.Equal(5, pers.Oeuvres.Count);
            Assert.Contains(oFilm, pers.Oeuvres);
            Assert.Contains(oEpisode, pers.Oeuvres);
            Assert.Contains(oTrilogie, pers.Oeuvres);
            Assert.Contains(oSerie, pers.Oeuvres);
            Assert.Contains(oUnivers, pers.Oeuvres);
        }

        [Fact]
        public void TesTPersonne_RetirerOeuvre()
        {
            var date = new DateTime(1985, 10, 5);

            var oFilm = new Film("oeuvre", date, "image", "syn", Themes.Action, false);
            var oFilm2 = new Film("oeuvre2", date, "image", "syn", Themes.Action, false);

            var pers = new Personne("brad", "broud", "il était une foi la vie", "la terre", "une image", date);

            pers.AjouterOeuvre(oFilm);
            pers.RetirerOeuvre(oFilm2);

            Assert.Contains(oFilm, pers.Oeuvres);

            pers.RetirerOeuvre(oFilm);

            Assert.Empty(pers.Oeuvres);
        }

        [Fact]
        public void TestPersonne_EqualsHashCode()
        {
            var pers = new Personne("Pitt", "Brad", "blablablabla", "Américaine", "Images/Photos/brad.jpg",
                new DateTime(1963, 12, 18));

            var pers2 = new Personne("Pitt", "Brad", "bonjour", "Israéliens", "image2", new DateTime(1963, 12, 18));

            var pers3 = new Personne("Michel", "Michel", "blablablabla", "Américaine", "Images/Photos/brad.jpg",
                new DateTime(1985, 10, 2));

            Assert.False(pers.Equals((object) null));
            Assert.True(pers.Equals((object) pers));
            Assert.False(pers.Equals("test"));
            Assert.True(pers.Equals((object) pers2));
            Assert.False(pers.Equals(pers3));

            Assert.Equal(pers2.GetHashCode(), pers.GetHashCode());
            Assert.NotEqual(pers3.GetHashCode(), pers.GetHashCode());
        }
    }
}
