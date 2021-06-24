using Modele;
using Xunit;

namespace UnitTests
{
    public class UnitTestStreaming
    {
        [Fact]
        public void TestStreaming()
        {
            var streaming = new Streaming("Fight Club", "primevideo.fightclub.com", Plateformes.PrimeVideo);

            Assert.True(streaming.Titre == "Fight Club");
            Assert.True(streaming.Lien == "primevideo.fightclub.com");
            Assert.True(streaming.Plateforme == Plateformes.PrimeVideo);
        }

        [Fact]
        public void TestStreaming_EqualsHashCode()
        {
            var stream = new Streaming("", "", Plateformes.PrimeVideo);

            var stream2 = new Streaming("", "", Plateformes.PrimeVideo);

            var stream3 = new Streaming("", "", Plateformes.OCS);

            Assert.False(stream.Equals((object) null));
            Assert.True(stream.Equals((object) stream));
            Assert.False(stream.Equals("test"));
            Assert.True(stream.Equals((object) stream2));
            Assert.False(stream.Equals(stream3));

            Assert.Equal(stream2.GetHashCode(), stream.GetHashCode());
            Assert.NotEqual(stream3.GetHashCode(), stream.GetHashCode());
        }
    }
}
