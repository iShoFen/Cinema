using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace DataEncryption
{
    /// <summary>
    /// Permet d'encrypter et de décrypter une partie fichier XML avec l’algo-rythme AES puis chiffre cette clef avec RSA
    /// </summary>
    public class XmlRsa : IEncryption
    {
        public string FolderPath { get; set; }

        private const string FILE_NAME = "XMLKey.bin";

        private const string CONTAINER_NAME = "XML_ENC_RSA_KEY";

        private const string KEY_NAME = "RSA_KEY";

        private const string ENCRYPTION_ELEMENT_ID = "EncryptedData";

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
            
            var rsaKey = new RSACryptoServiceProvider(new CspParameters {KeyContainerName = CONTAINER_NAME});
            var aesKey = Aes.Create();

            var encryptedElement = new EncryptedXml().EncryptData(elementEncrypt, aesKey, false);
            var encryptedKey = EncryptedXml.EncryptKey(aesKey.Key, rsaKey, false);
            aesKey.Clear();

            var ek = new EncryptedKey
            {
                CipherData = new CipherData(encryptedKey),
                EncryptionMethod = new EncryptionMethod(EncryptedXml.XmlEncRSA15Url)
            };
            ek.AddReference(new DataReference {Uri = "#" + ENCRYPTION_ELEMENT_ID});
            ek.KeyInfo.AddClause(new KeyInfoName{Value = KEY_NAME});

            var edElement = new EncryptedData
            {
                Type = EncryptedXml.XmlEncElementUrl, 
                Id = ENCRYPTION_ELEMENT_ID,
                EncryptionMethod = new EncryptionMethod(EncryptedXml.XmlEncAES256Url)
            };
            edElement.KeyInfo.AddClause(new KeyInfoEncryptedKey(ek));
            edElement.CipherData.CipherValue = encryptedElement;

            EncryptedXml.ReplaceElement(elementEncrypt, edElement, false);

            var settings = new XmlWriterSettings {Indent = true, Encoding = Encoding.UTF8};
            using (TextWriter tw = File.CreateText(Path.Combine(FolderPath, fileName)))
                using (var writer = XmlWriter.Create(tw, settings)) 
                    xmlFile.Save(writer);


            using (var writer = new BinaryWriter(File.Create(FilePath)))
            {
                writer.Write(int.MaxValue);
                writer.Write(float.MaxValue);
                writer.Write(double.MaxValue);
                writer.Write(rsaKey.ExportCspBlob(true));
            }

            rsaKey.Clear();
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

            if (xmlFile.GetElementsByTagName("EncryptedData")[0] is not XmlElement) return ToXDocument(xmlFile);
            if (!File.Exists(Path.Combine(FolderPath, FILE_NAME))) return ToXDocument(xmlFile);

            var rsaKey = new RSACryptoServiceProvider(new CspParameters {KeyContainerName = CONTAINER_NAME});
            using (var reader = new BinaryReader(File.OpenRead(Path.Combine(FolderPath, FILE_NAME))))
            {
                _ = reader.ReadInt32();
                _ = reader.ReadSingle();
                _ = reader.ReadDouble();
                rsaKey.ImportCspBlob(reader.ReadBytes(596));
            }

            var eXml = new EncryptedXml(xmlFile);
            eXml.AddKeyNameMapping(KEY_NAME, rsaKey);
            eXml.DecryptDocument();

            rsaKey.Clear();
            return ToXDocument(xmlFile);
        }
    }
}
