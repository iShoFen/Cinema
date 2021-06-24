using System.IO;
using System.Xml;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml.Linq;

namespace DataEncryption
{
    /// <summary>
    /// Permet d'encrypter et de décrypter une partie fichier XML avec l’algo-rythme AES
    /// </summary>
    public class XmlAes : IEncryption
    {
        public string FolderPath { get; set; }

        private const string FILE_NAME = "XMLKey.bin";

        private string FilePath => Path.Combine(FolderPath, FILE_NAME);

        /// <summary>
        /// Permet de transformer un XDocument en XmlDocument
        /// </summary>
        /// <param name="xNode">Le XDocument</param>
        /// <returns>un XmlDocument</returns>
        private static XmlDocument ToXmlDocument(XNode xNode)
        {
            var xmlDocument = new XmlDocument{PreserveWhitespace = true};
            using var xmlReader = xNode.CreateReader();
            xmlDocument.Load(xmlReader);
            return xmlDocument;
        }

        /// <summary>
        /// Permet de transformer un XmlDocument en XDocument
        /// </summary>
        /// <param name="xmlNode">Le XmlDocument</param>
        /// <returns>un XDocument</returns>
        private static XDocument ToXDocument(XmlNode xmlNode)
        {
            using var nodeReader = new XmlNodeReader(xmlNode);
            nodeReader.MoveToContent();
            return XDocument.Load(nodeReader);
        }

        /// <summary>
        /// Permet d'encrypter une partie d'un fichier XML
        /// </summary>
        /// <param name="file">Le fichier XML</param>
        /// <param name="fileName">Le nom du fichier XML</param>
        /// <param name="elementName">L'élément à encrypter</param>
        public void Encrypter(object file, string fileName, string elementName)
        {
            if (file is null || elementName is null) return;

            XmlDocument xmlFile;
            try { xmlFile = ToXmlDocument(file as XDocument); }

            catch (XmlException) { return; }

            if (xmlFile.GetElementsByTagName(elementName)[0] is not XmlElement elementEncrypt) return;

            var key = Aes.Create();
            var encryptedElement = new EncryptedXml().EncryptData(elementEncrypt, key, false);


            var edElement = new EncryptedData
            {
                Type = EncryptedXml.XmlEncElementUrl,
                EncryptionMethod = new EncryptionMethod(EncryptedXml.XmlEncAES256Url),
                CipherData = { CipherValue = encryptedElement }
            };

            EncryptedXml.ReplaceElement(elementEncrypt, edElement, false);

            var settings = new XmlWriterSettings {Indent = true, Encoding = Encoding.UTF8};
            using (TextWriter tw = File.CreateText(Path.Combine(FolderPath, fileName)))
                using (var writer = XmlWriter.Create(tw, settings)) 
                    xmlFile.Save(writer);

            using (var writer = new BinaryWriter(File.Create(FilePath)))
            {
                writer.Write(int.MaxValue);
                writer.Write(key.Key.Length);

                writer.Write(float.MaxValue);
                writer.Write(key.Key);

                writer.Write(double.MaxValue);
                writer.Write(key.IV.Length);

                writer.Write(long.MaxValue);
                writer.Write(key.IV);
            }

            key.Clear();
        }

        /// <summary>
        /// Permet de décrypter une partie d'un fichier XML
        /// </summary>
        /// <param name="fileName">Le nom du fichier XML</param>
        public object Decrypter(string fileName)
        {
            if(fileName is null) return null;

            var xmlFile = new XmlDocument{PreserveWhitespace = true};
            try { xmlFile.Load(Path.Combine(FolderPath, fileName));}

            catch (XmlException) { return null; }

            if (!File.Exists(Path.Combine(FolderPath, FILE_NAME))) return ToXDocument(xmlFile);

            var key = Aes.Create();
            using (var reader = new BinaryReader(File.OpenRead(Path.Combine(FolderPath, FILE_NAME))))
            {
                _ = reader.ReadInt32();
                var keyLength= reader.ReadInt32();

                _ = reader.ReadSingle();
                key.Key = reader.ReadBytes(keyLength);

                _ = reader.ReadDouble();
                var ivLength = reader.ReadInt32();

                _ = reader.ReadInt64();
                key.IV = reader.ReadBytes(ivLength);
            }

            if (xmlFile.GetElementsByTagName("EncryptedData")[0] is not XmlElement encryptedElement) return ToXDocument(xmlFile);

            var edElement = new EncryptedData();
            edElement.LoadXml(encryptedElement);

            var eXml = new EncryptedXml();
            var decryptedElement = eXml.DecryptData(edElement, key);
            eXml.ReplaceData(encryptedElement, decryptedElement);

            key.Clear();
            return ToXDocument(xmlFile);
        }
    }
}