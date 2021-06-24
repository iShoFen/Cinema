using System;
using Modele;
using Xunit;

namespace UnitTests
{
    public class UnitTestEpisode
    {
        [Fact]
        public void TestEpisode_EqualsHashCode()
        {
            var ep = new Episode("Fight Club", new DateTime(1999, 11, 10), "Images/Photos/FC.jpg",
                "Description film...", Themes.Action, false);

            var ep2 = new Episode("Fight Club", new DateTime(1999, 11, 10), "image",
                "blabla", Themes.Aventure, false);

            var ep3 = new Episode("Fight", new DateTime(1985, 10, 01), "Images/Photos/FC.jpg",
                "Description film...", Themes.Action, false);

            Assert.False(ep.Equals((object) null));
            Assert.True(ep.Equals((object) ep));
            Assert.False(ep.Equals("test"));
            Assert.True(ep.Equals((object) ep2));
            Assert.False(ep.Equals(ep3));

            Assert.Equal(ep2.GetHashCode(), ep.GetHashCode());
            Assert.NotEqual(ep3.GetHashCode(), ep.GetHashCode());
        }
    }
}
