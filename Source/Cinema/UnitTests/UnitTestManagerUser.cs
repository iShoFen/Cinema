using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Modele;
using static Modele.Plateformes;

namespace UnitTests
{
    public partial class UnitTestManager
    {
        [Fact]
        public void TestManager_CreerUser()
        {
            var man = new Manager();

            var user1 = new User("Mich", "1234");

            var fine = man.CreerUser("Mich", "1234", out var user);

            Assert.True(fine);
            Assert.Equal(user1, user);

            fine = man.CreerUser("Mich", "1234", out user);

            Assert.False(fine);
            Assert.Null(user);
        }

        [Fact]
        public void TestManager_Connexion()
        {
            var man = new Manager();

            var user1 = new User("Mich", "1234");

            man.CreerUser("Mich", "1234", out _);

            var fine = man.Connexion("Mich", "1234", out var user);

            Assert.True(fine);
            Assert.Equal(user1, user);

            fine = man.Connexion("Mich", "4321", out user);
            
            Assert.False(fine);
            Assert.Null(user);

            fine = man.Connexion("Ambi", "Satan", out user);

            Assert.False(fine);
            Assert.Null(user);
        }

        [Fact]
        public void TestManager_ChangerPassword()
        {
            var man = new Manager();

            man.CreerUser("Ambi", "Satan", out var user);

            var fine= Manager.ChangerPassword(user, "Satan", "1234");

            Assert.True(fine);
            Assert.Equal("1234", user.Mdp);

            fine = Manager.ChangerPassword(user, "Satan", "4567");

            Assert.False(fine);
            Assert.NotEqual("4567", user.Mdp);
        }

        [Fact]
        public void TestManager_IsAvailable()
        {
            var man = new Manager();

            man.CreerUser("Ambi", "Satan", out _);

            var fine = man.IsAvailable("Miel");

            Assert.True(fine);

            fine = man.IsAvailable("Ambi");

            Assert.False(fine);
        }

        [Fact]
        public void TestManager_AjouterAvis()
        {
            var man = new Manager();

            var date = new DateTime(1985, 12, 15);

            man.CreerUser("Admin", "Admin", out var user);
            user.IsAdmin = true;

            var avis = new Avis(4.5f, "blabla");
            var avis2 = new Avis(4f, "nooooooooo");

            var film = man.CreerLeaf(user, "Film", "Fight Club", date, "image", "synopsis", Themes.Action, false);

            man.AjouterAvis(user, film, 4.5f, "blabla");
            man.AjouterAvis(user, film, 4f, "nooooooooo");

            var dic = film.ListeAvis.ToDictionary(pair => pair.Key, pair => pair.Value);
            Assert.Contains(user, dic.Keys);
            Assert.Contains(avis, dic.Values);
            Assert.DoesNotContain(avis2, dic.Values);
        }

        [Fact]
        public void TestManager_SupprimerAvis()
        {
            var man = new Manager();

            var date = new DateTime(1985, 12, 15);

            man.CreerUser("Admin", "Admin", out var user);
            user.IsAdmin = true;

            var avis = new Avis(4.5f, "blabla");
            var avis2 = new Avis(4f, "nooooooooo");

            var film = man.CreerLeaf(user, "Film", "Fight Club", date, "image", "synopsis", Themes.Action, false);

            man.AjouterAvis(user, film, 4.5f, "blabla");
            man.AjouterAvis(user, film, 4f, "nooooooooo");

            Manager.SupprimerAvis(user, user, film);

            var dic = film.ListeAvis.ToDictionary(pair => pair.Key, pair => pair.Value);
            Assert.DoesNotContain(avis, dic.Values);

            
            Manager.SupprimerAvis(user, user, film);

            dic = film.ListeAvis.ToDictionary(pair => pair.Key, pair => pair.Value);
            Assert.DoesNotContain(avis, dic.Values);
        }

        [Fact]
        public void TestManager_ModifInfo()
        {
            var man = new Manager();

            var date = new DateTime(1985, 12, 15);

            man.CreerUser("Admin", "Admin", out var user);
            user.IsAdmin = true;

            var avis = new Avis(4.5f, "cool");

            var film = man.CreerLeaf(user, "Film", "Fight Club", date, "image", "synopsis", Themes.Action, false);
            var film2 = man.CreerLeaf(user, "Film", "Club", date, "image", "synopsis", Themes.Action, true);

            film.AjouterAvis(user, avis);
            film2.AjouterAvis(user, avis);

            user.AjouterEnvie(film);
            user.AjouterEnvie(film2);
            user.AjouterConsulte(film);
            user.AjouterConsulte(film2);

            var fine = man.ChangerInfos(user, "Admin", "Bonjour", "image", true, new List<Plateformes> {Netflix, Disney});

            Assert.True(fine);
            Assert.Equal("Bonjour", user.Pseudo);
            Assert.Equal("image", user.ImageProfil);
            Assert.True(user.IsModeFamille);
            Assert.Contains(Netflix, user.ListePlateformes);
            Assert.Contains(Disney, user.ListePlateformes);
            Assert.DoesNotContain(film, user.ListeEnvie);
            Assert.Contains(film2, user.ListeEnvie);
            Assert.DoesNotContain(film, user.RecemmentConsulte);
            Assert.Contains(film2, user.RecemmentConsulte);

            var dic = film.ListeAvis.ToDictionary(pair => pair.Key, pair => pair.Value);
            var dic2 = film2.ListeAvis.ToDictionary(pair => pair.Key, pair => pair.Value);
            Assert.DoesNotContain(user, dic.Keys);
            Assert.Contains(user, dic2.Keys);

            fine = man.ChangerInfos(user, "1234", "Au revoir", "non", false, new List<Plateformes> {OCS, PrimeVideo});

            Assert.False(fine);
            Assert.NotEqual("Au revoir", user.Pseudo);
            Assert.NotEqual("non", user.ImageProfil);
            Assert.True(user.IsModeFamille);
            Assert.DoesNotContain(OCS, user.ListePlateformes);
            Assert.DoesNotContain(PrimeVideo, user.ListePlateformes);
        }

        [Fact]
        public void TestManager_RecemmentConsulte()
        {
            var man = new Manager();

            var date = new DateTime(1995, 10, 5);

            var oe = new Film("Oui", date, "lien image", "il était une fois", Themes.Action, false);
            var pers = new Personne("Michel", "Polnaref", "oui", "toujours oui", null, date);

            man.CreerUser("Ambi", "Satan", out var user);

            Manager.AjouterAuRecemmentConsulte(user, oe);
            Manager.AjouterAuRecemmentConsulte(user, pers);
            Manager.AjouterAuRecemmentConsulte(user, "et oui");

            Assert.Contains(oe, user.RecemmentConsulte);
            Assert.Contains(pers, user.RecemmentConsulte);
            Assert.DoesNotContain("et oui", user.RecemmentConsulte);
        }

        [Fact]
        public void TestManager_AjoutEnvie()
        {
            var man = new Manager();

            var date = new DateTime(1995, 10, 5);

            var oe = new Film("Oui", date, "lien image", "il était une fois", Themes.Action, false);

            man.CreerUser("Ambi", "Satan", out var user);

            Manager.AjouterALaListeEnvies(user, oe);

            Assert.Contains(oe, user.ListeEnvie);

            Manager.AjouterALaListeEnvies(user, oe);

            Assert.Single(user.ListeEnvie);
            Assert.Contains(oe, user.ListeEnvie);
        }

        [Fact]
        public void TestManager_SupprimerEnvie()
        {
            var man = new Manager();

            var date = new DateTime(1995, 10, 5);

            var oe = new Film("Oui", date, "lien image", "il était une fois", Themes.Action, false);

            var oe2 = new Episode("Oui", date, "lien image", "il était une fois", Themes.Action, false);

            man.CreerUser("Ambi", "Satan", out var user);
            if (user == null) return;

            Manager.AjouterALaListeEnvies(user, oe);
            Manager.RetirerDeLaListeEnvies(user, oe);

            Assert.Empty(user.ListeEnvie);
            Assert.DoesNotContain(oe, user.ListeEnvie);

            Manager.RetirerDeLaListeEnvies(user, oe2);

            Assert.Empty(user.ListeEnvie);
            Assert.DoesNotContain(oe2, user.ListeEnvie);
        }

        [Fact]
        public void TestManager_GraderUser()
        {
            var man = new Manager();

            man.CreerUser("Admin", "Admin", out var userA);
            userA.IsAdmin = true;

            man.CreerUser("Ambi", "Satan", out var user);
            
            Manager.GraderUser(userA, user);
            Assert.True(user.IsAdmin);
        }

        [Fact]
        public void TestManager_SupprimerUser()
        {
            var man = new Manager();

            var date = new DateTime(1995, 10, 5);

            man.CreerUser("Michel", "1324", out var user);

            man.CreerUser("Ambi", "Satan", out var user2);

            man.CreerUser("Admin", "Admin", out var user3);
            user3.IsAdmin = true;

            var film = man.CreerLeaf(user3, "Film", "Fight Club", date, "image", "synopsis", Themes.Action, false);
            man.AjouterAvis(user, film, 4.5f, "oueee");
            man.AjouterAvis(user2, film, 4f, "yess");

            man.SupprimerUser(user3, user);

            Assert.DoesNotContain(user, man.Users);

            var dic = film.ListeAvis.ToDictionary(pair => pair.Key, pair => pair.Value);
            Assert.DoesNotContain(user, dic.Keys);

            man.SupprimerUser(user2, user3);

            Assert.Contains(user3, man.Users);
        }
    }
}
