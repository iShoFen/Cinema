using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQ_to_Data;
using Modele;
using Xunit;

namespace UnitTests
{
    public class UnitTestPersistance
    {
        [Fact]
        public void TestPersistanceXml()
        {
            var xml = new LinqToXml();

            var xmlMan1 = new Manager(xml);

            xmlMan1.Sauvegarder();

            var xmlMan2 = new Manager(xml);

            Assert.Equal(xmlMan1.Oeuvres, xmlMan2.Oeuvres);
            Assert.Equal(xmlMan1.Personnes, xmlMan2.Personnes);
            Assert.Equal(xmlMan1.Users, xmlMan2.Users);
        }

        [Fact]
        public void TestPersistanceJson()
        {
            var json = new LinqToJson();

            var jsonMan1 = new Manager(json);

            jsonMan1.Sauvegarder();

            var jsonMan2 = new Manager(json);

            Assert.Equal(jsonMan1.Oeuvres, jsonMan2.Oeuvres);
            Assert.Equal(jsonMan1.Personnes, jsonMan2.Personnes);
            Assert.Equal(jsonMan1.Users, jsonMan2.Users);
        }
    }
}