using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataEncryption;
using LINQ_to_Data;
using Modele;
using Xunit;

namespace UnitTests
{
    public class UnitTestEncryption
    {
        [Fact]
        public void TestRsaXml()
        {
            var xml = new LinqToXml(new XmlRsa(), "rsaXMLTest.xml",
                Path.Combine(Directory.GetCurrentDirectory(), "Data\\XML\\RSA"));

            var xmlMan1 = new Manager(xml);

            xmlMan1.Sauvegarder();

            var filePath = Path.Combine(xml.FolderPath, xml.FileName);
            var stringFile = File.ReadAllText(filePath);

            Assert.True(stringFile.Contains("EncryptedData") && !stringFile.Contains("User"));

            var xmlMan2 = new Manager(xml);

            Assert.Equal(xmlMan1.Users, xmlMan2.Users);
        }

        [Fact]
        public void TestAesXml()
        {
            var xml = new LinqToXml(new XmlAes(), "aesXMLTest.xml",
                Path.Combine(Directory.GetCurrentDirectory(), "Data\\XML\\AES"));

            var xmlMan1 = new Manager(xml);

            xmlMan1.Sauvegarder();

            var filePath = Path.Combine(xml.FolderPath, xml.FileName);
            var stringFile = File.ReadAllText(filePath);

            Assert.True(stringFile.Contains("EncryptedData") && !stringFile.Contains("User"));

            var xmlMan2 = new Manager(xml);

            Assert.Equal(xmlMan1.Oeuvres, xmlMan2.Oeuvres);
            Assert.Equal(xmlMan1.Personnes, xmlMan2.Personnes);
            Assert.Equal(xmlMan1.Users, xmlMan2.Users);
        }

        [Fact]
        public void TestAesJson()
        {
            var json = new LinqToJson(new JsonAes(), "aesJSONTest.json",  Path.Combine(Directory.GetCurrentDirectory(), "Data\\JSON\\AES"));

            var JsonMan1 = new Manager(json);

            JsonMan1.Sauvegarder();

            var filePath = Path.Combine(json.FolderPath, json.FileName);
            var stringFile = File.ReadAllText(filePath);

            Assert.True(!stringFile.Contains("Password"));

            var JsonMan2 = new Manager(json);

            Assert.Equal(JsonMan1.Oeuvres, JsonMan2.Oeuvres);
            Assert.Equal(JsonMan1.Personnes, JsonMan2.Personnes);
            Assert.Equal(JsonMan1.Users, JsonMan2.Users);
        }
    }
}
