using System;
using Xunit;
using Modele;

namespace UnitTests
{
    public partial class UnitTestManager
    {
        [Fact]
        public void TestManager_CreerPersonne()
        {
            var man = new Manager();

            var date = new DateTime(1985, 08, 15);

            man.CreerUser("Michou", "1234", out var user);
            if (user == null) return;
            
            man.CreerUser("Admin", "Admin", out var user2);
            if (user2 == null) return;
            user2.IsAdmin = true;

            var pers = man.CreerPersonne(user2, "Brad", "Pitt", "il est vivant", "Américain", "une image", date);

            Assert.Contains(pers, man.Personnes);

            pers = man.CreerPersonne(user, "Leonardo", "Dicaprio", "il vie toujours", "Américain", "une image",
                date);

            Assert.Null(pers);
            Assert.DoesNotContain(pers, man.Personnes);
        }

        [Fact]
        public void TestManager_HasPersonne()
        {
            var man = new Manager();

            var date = new DateTime(1985, 08, 15);

            man.CreerUser("Admin", "Admin", out var user);
            if (user == null) return;
            user.IsAdmin = true;

            var pers = man.CreerPersonne(user, "Brad", "Pitt", "il est vivant", "Américain", "une image", date);

            var fine = man.HasPersonne("Brad", "Pitt", out var pers2);

            Assert.True(fine);
            Assert.NotNull(pers2);
            Assert.Equal(pers, pers2);

            fine = man.HasPersonne("Leonardo", "Dicaprio", out pers2);

            Assert.False(fine);
            Assert.Null(pers2);
        }

        [Fact]
        public void TestManager_SupprimerPersonne()
        {
            var man = new Manager();

            var date = new DateTime(1985, 08, 15);

            man.CreerUser("Admin", "Admin", out var user);
            if (user == null) return;
            user.IsAdmin = true;

            var pers = man.CreerPersonne(user, "Brad", "Pitt", "il est vivant", "Américain", "une image", date);

            man.SupprimerPersonne(user, pers);
            Assert.DoesNotContain(pers, man.Personnes);

            man.SupprimerPersonne(user, pers);
            Assert.DoesNotContain(pers, man.Personnes);
        }
    }
}
