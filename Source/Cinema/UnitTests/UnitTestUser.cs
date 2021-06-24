using System;
using System.Collections.Generic;
using Modele;
using Xunit;

namespace UnitTests
{
    public class UnitTestUser
    {
        [Fact]
        public void TestUser()
        {
            var user = new User("MichMich", "*Azertyuiop123*");

            Assert.True(user.Pseudo == "MichMich");
            Assert.Null(user.ImageProfil);
            Assert.True(!user.IsModeFamille);
            Assert.True(!user.IsAdmin);
        }

        [Fact]
        public void TestUser_ChangerProfil()
        {
            var user = new User("MichMich", "*Azertyuiop123*");

            user.ChangerProfil("Michel", "nouv.png", true, new List<Plateformes> {Plateformes.PrimeVideo, Plateformes.Netflix});

            Assert.True(user.Pseudo == "Michel");
            Assert.True(user.ImageProfil == "nouv.png");
            Assert.True(user.IsModeFamille);
            Assert.Contains(Plateformes.PrimeVideo, user.ListePlateformes);
            Assert.Contains(Plateformes.Netflix, user.ListePlateformes);

            user.ChangerProfil("", "", true, new List<Plateformes>{Plateformes.PrimeVideo, Plateformes.Disney});

            Assert.Contains(Plateformes.Disney, user.ListePlateformes);
            Assert.Equal(2, user.ListePlateformes.Count);
        }

        [Fact]
        public void TestUser_AjouterLeaf()
        {
            var user = new User("MichMich", "*Azertyuiop123*");

            var film = new Film("Fight Club", new DateTime(1999, 11, 10), "Images/Photos/FC.jpg",
                "Description film...", Themes.Action, false);

            var episode = new Episode("Fight Club", new DateTime(1999, 11, 10), "Images/Photos/FC.jpg",
                "Description film...", Themes.Action, false);

            var trilogie = new Trilogie("Fight Club", new DateTime(1999, 11, 10), "Images/Photos/FC.jpg",
                "Description film...", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Oeuvre>());

            var serie = new Serie("Fight Club", new DateTime(1999, 11, 10), "Images/Photos/FC.jpg",
                "Description film...", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>());

            var univers = new Univers("Fight Club", new DateTime(1999, 11, 10), "Images/Photos/FC.jpg",
                "Description film...", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>(), new List<Oeuvre>());

            user.AjouterEnvie(film);
            user.AjouterEnvie(episode);
            user.AjouterEnvie(trilogie);
            user.AjouterEnvie(serie);
            user.AjouterEnvie(univers);
            
            Assert.Contains(film, user.ListeEnvie);
            Assert.DoesNotContain(episode, user.ListeEnvie);
            Assert.DoesNotContain(trilogie, user.ListeEnvie);
            Assert.Contains(serie, user.ListeEnvie);
            Assert.DoesNotContain(univers, user.ListeEnvie);

            user.AjouterEnvie(film);
            user.AjouterEnvie(serie);

            Assert.Equal(2, user.ListeEnvie.Count);
            Assert.Contains(film, user.ListeEnvie);
            Assert.Contains(serie, user.ListeEnvie);
        }

        [Fact]
        public void TestUser_RetirerLeaf()
        {
            var user = new User("MichMich", "*Azertyuiop123*");

            var mov = new Film("Fight Club", new DateTime(1999, 11, 10), "Images/Photos/FC.jpg",
                "Description film...", Themes.Action, false);

            var mov2 = new Film("Fight Club2", new DateTime(1999, 11, 10), "Images/Photos/FC.jpg",
                "Description film...", Themes.Action, false);

            user.AjouterEnvie(mov);
            user.RetirerEnvie(mov2);

            Assert.Contains(mov, user.ListeEnvie);

            user.RetirerEnvie(mov);

            Assert.Empty(user.ListeEnvie);
        }

        [Fact]
        public void TestUser_AjouterRecemmentConsulte()
        {
            var date = new DateTime(1985, 10, 5);
            var pers1 = new Personne("mich", "mouch", "la vie", "un pays", "image", date);
            var pers2 = new Personne("miche", "mouch", "la vie", "un pays", "image", date);
            var pers3 = new Personne("michr", "mouch", "la vie", "un pays", "image", date);
            var pers4 = new Personne("micht", "mouch", "la vie", "un pays", "image", date);
            var pers5 = new Personne("michy", "mouch", "la vie", "un pays", "image", date);
            var pers6 = new Personne("michu", "mouch", "la vie", "un pays", "image", date);
            
            var film = new Film("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>{["acteur"] = new Dictionary<Personne, string>{[pers1] = "bla"}},
                new List<Streaming>());
            
            var episode = new Episode("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>{["acteur"] = new Dictionary<Personne, string>{[pers1] = "bla"}},
                new List<Streaming>());
            
            var trilo = new Trilogie("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>{["acteur"] = new Dictionary<Personne, string>{[pers1] = "bla"}},
                new List<Oeuvre>());
            
            var serie = new Serie("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>{["acteur"] = new Dictionary<Personne, string>{[pers1] = "bla"}});

            
            var uni = new Univers("oeuvre", date, "image", "syn", Themes.Action, false,
                new Dictionary<string, IEnumerable<KeyValuePair<Personne, string>>>{["acteur"] = new Dictionary<Personne, string>{[pers1] = "bla"}},
                new List<Oeuvre>());

            var user = new User("couc", "1234");

            user.AjouterConsulte(pers1);
            user.AjouterConsulte(pers2);
            user.AjouterConsulte(pers3);
            user.AjouterConsulte(pers4);
            user.AjouterConsulte(pers5);
            user.AjouterConsulte(pers6);

            user.AjouterConsulte(film);
            user.AjouterConsulte(episode);
            user.AjouterConsulte(trilo);
            user.AjouterConsulte(serie);
            user.AjouterConsulte(uni);

            user.AjouterConsulte(date);


            Assert.Equal(10, user.RecemmentConsulte.Count);
            Assert.DoesNotContain(date, user.RecemmentConsulte);
            Assert.DoesNotContain(pers1, user.RecemmentConsulte);
            Assert.Contains(film, user.RecemmentConsulte);

            user.AjouterConsulte(pers2);

            Assert.Contains(pers2, user.RecemmentConsulte);
            Assert.Equal(pers2, user.RecemmentConsulte[0]);
        }

        [Fact]
        public void TestUser_EqualsHashCode()
        {
            var user = new User("MichMich", "*Azertyuiop123*");
            
            var user2 = new User("MichMich", "password");

            var user3 = new User("José", "");

            Assert.False(user.Equals((object) null));
            Assert.True(user.Equals((object) user));
            Assert.False(user.Equals("test"));
            Assert.True(user.Equals((object) user2));
            Assert.False(user.Equals(user3));

            Assert.Equal(user2.GetHashCode(), user.GetHashCode());
            Assert.NotEqual(user3.GetHashCode(), user.GetHashCode());
        }
    }
}