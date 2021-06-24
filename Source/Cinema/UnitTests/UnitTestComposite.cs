using System;
using System.Collections.Generic;
using Modele;
using Xunit;

namespace UnitTests
{
    public class UnitTestComposite
    {
        [Fact]
        public void TestComposite_ModifierTrilogie()
        {
            var date = new DateTime(1985, 10, 5);
            var date2 = new DateTime(1975, 8, 2);

            var oTri = new Trilogie("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Oeuvre>());

            oTri.ModifierComposite("test", date2, "lien", "le synopsis", Themes.Aventure, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>());

            Assert.Equal("test", oTri.Titre);
            Assert.Equal(date2, oTri.DateDeSortie);
            Assert.Equal("lien", oTri.LienImage);
            Assert.Equal("le synopsis", oTri.Synopsis);
            Assert.Equal(Themes.Aventure, oTri.Theme);
        }

        [Fact]
        public void TestComposite_ModifierSerie()
        {
            var date = new DateTime(1985, 10, 5);
            var date2 = new DateTime(1975, 8, 2);

            var oUni = new Univers("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Oeuvre>());

            oUni.ModifierComposite("test", date2, "lien", "le synopsis", Themes.Aventure, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>());

            Assert.Equal("test", oUni.Titre);
            Assert.Equal(date2, oUni.DateDeSortie);
            Assert.Equal("lien", oUni.LienImage);
            Assert.Equal("le synopsis", oUni.Synopsis);
            Assert.Equal(Themes.Aventure, oUni.Theme);
        }

        [Fact]
        public void TestComposite_ModifierUnivers()
        {
            var date = new DateTime(1985, 10, 5);
            var date2 = new DateTime(1975, 8, 2);

            var oUni = new Univers("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Oeuvre>());

            oUni.ModifierComposite("test", date2, "lien", "le synopsis", Themes.Aventure, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>());

            Assert.Equal("test", oUni.Titre);
            Assert.Equal(date2, oUni.DateDeSortie);
            Assert.Equal("lien", oUni.LienImage);
            Assert.Equal("le synopsis", oUni.Synopsis);
            Assert.Equal(Themes.Aventure, oUni.Theme);
        }

        [Fact]
        public void TestComposite_RetirerOeuvreTrilogie()
        {
            var date = new DateTime(1985, 10, 5);

            var film = new Film("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Streaming>());

            var film2 = new Film("oeuvre2", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Streaming>());

            var film3 = new Film("oeuvre3", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Streaming>());

            var eps = new Episode("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Streaming>());

            var oTri = new Trilogie("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Oeuvre>());

            oTri.AjouterOeuvres(new List<Oeuvre>{film, film2, film3});
            oTri.RetirerOeuvre(eps);

            Assert.Contains(film, oTri.ReOeuvres);
            Assert.Contains(film2, oTri.ReOeuvres);
            Assert.Contains(film3, oTri.ReOeuvres);

            oTri.RetirerOeuvre(film);

            Assert.DoesNotContain(film, oTri.ReOeuvres);
            Assert.Contains(film2, oTri.ReOeuvres);
            Assert.Contains(film3, oTri.ReOeuvres);
        }

        [Fact]
        public void TestComposite_RetirerOeuvreSerie()
        {
            var date = new DateTime(1985, 10, 5);

            var film = new Film("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Streaming>());

            var eps = new Episode("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Streaming>());

            var oSerie = new Serie("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>());

            oSerie.AjouterOeuvres(new List<Oeuvre> { eps });
            oSerie.RetirerOeuvre(film);

            Assert.Contains(eps, oSerie.ReOeuvres);

            oSerie.RetirerOeuvre(eps);

            Assert.Empty(oSerie.ReOeuvres);
        }

        [Fact]
        public void TestComposite_RetirerOeuvreUnivers()
        {
            var date = new DateTime(1985, 10, 5);

            var oSerie = new Serie("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>());

            var oUni = new Univers("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Oeuvre>());

            oUni.AjouterOeuvres(new List<Oeuvre> { oSerie });

            Assert.Contains(oSerie, oUni.ReOeuvres);

            Assert.Contains(oSerie, oUni.ReOeuvres);
        }
    }
}