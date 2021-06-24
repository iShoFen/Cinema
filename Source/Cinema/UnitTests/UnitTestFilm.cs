using System;
using Modele;
using Xunit;

namespace UnitTests
{
    public class UnitTestFilm
    {
        [Fact]
        public void TestFilm_EqualsHashCode()
        {
            var mov = new Film("Fight Club", new DateTime(1999, 11, 10), "Images/Photos/FC.jpg",
                "Description film...", Themes.Action, false);

            var mov2 = new Film("Fight Club", new DateTime(1999, 11, 10), "image",
                "blabla", Themes.Aventure, false);

            var mov3 =  new Film("Fight", new DateTime(1985, 10, 01), "Images/Photos/FC.jpg",
                "Description film...", Themes.Action, false);

            Assert.False(mov.Equals((object)null));
            Assert.True(mov.Equals((object)mov));
            Assert.False(mov.Equals("test"));
            Assert.True(mov.Equals((object)mov2));
            Assert.False(mov.Equals(mov3));

            Assert.Equal(mov2.GetHashCode(), mov.GetHashCode());
            Assert.NotEqual(mov3.GetHashCode(), mov.GetHashCode());
        }

    }
}