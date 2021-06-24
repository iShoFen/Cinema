using System;
using Modele;
using Xunit;

namespace UnitTests
{
    public class UnitTestAvis
    {
        [Theory]
        [InlineData(4f, "Pas terrible...")]
        [InlineData(-5f, "Pas terrible...")]
        [InlineData(8f, "Pas terrible...")]
        public void TestAvis(float note, string comm)
        {
            var avis = new Avis(note, comm);

            switch (note)
            {
                case < 0:
                    Assert.Equal(0f, avis.Note);
                    break;
                case > 5:
                    Assert.Equal(5f, avis.Note);
                    break;
                default:
                    Assert.Equal(note, avis.Note);
                    break;
            }

            Assert.Equal(comm, avis.Commentaire);
        }

        [Fact]
        public void TestAvis_EqualsHashCode()
        {
            var avis = new Avis(4f, "Pas terrible...");

            var avis2 = new Avis(4f, "Pas terrible...");

            var avis3 = new Avis(2f, "Trop bien");

            Assert.False(avis.Equals((object) null));
            Assert.True(avis.Equals((object) avis));
            Assert.False(avis.Equals("test"));
            Assert.True(avis.Equals((object) avis2));
            Assert.False(avis.Equals(avis3));

            Assert.Equal(avis2.GetHashCode(), avis.GetHashCode());
            Assert.NotEqual(avis3.GetHashCode(), avis.GetHashCode());
        }
    }
}
