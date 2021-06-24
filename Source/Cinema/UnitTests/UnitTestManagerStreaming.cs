using System.Collections.Generic;
using System.Linq;
using Modele;
using Xunit;
using static Modele.Plateformes;

namespace UnitTests
{
    public partial class UnitTestManager
    {
        [Theory]
        [InlineData("Fight Club", "netflix.com", Netflix)]
        [InlineData("Fight Club", "prime.com", PrimeVideo)]
        public void TestManager_AddStreaming(string titre, string lien, Plateformes p)
        {
            var man = new Manager();

            var str = new Streaming(titre, lien, p);

            var list = man.AjouterStreamLeaf(titre,
                new Dictionary<Plateformes, string> {[p] = lien}).ToList();

            Assert.Contains(str, list);
        }
    }
}
