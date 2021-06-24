using System.IO;
using System.Security.Cryptography;

namespace DataEncryption
{
    /// <summary>
    /// Permet d'encrypter et de décrypter un fichier JSON avec l’algo-rythme AES
    /// </summary>
    public class JsonAes : IEncryption
    {
        public string FolderPath { get; set; }

        private const string FILE_NAME = "JSONKey.bin";

        private const int LIST_MIN = 0;

        private string FilePath => Path.Combine(FolderPath, FILE_NAME);

        /// <summary>
        /// Permet d'encrypter une partie d'un fichier JSON
        /// </summary>
        /// <param name="file">Le fichier JSON</param>
        /// <param name="fileName">Le nom du fichier JSON</param>
        /// <param name="elementName">L'élément à encrypter</param>
        public void Encrypter(object file, string fileName, string elementName = null)
        {
            if (file is not string data || fileName is null) return;

            var key = Aes.Create();
            key.Padding = PaddingMode.Zeros;

            var encryptor = key.CreateEncryptor(key.Key, key.IV);
            byte[] encryptedJson;

            using (var memoStrEncrypt = new MemoryStream())
            {
                using (var cryptoStrEncrypt = new CryptoStream(memoStrEncrypt, encryptor, CryptoStreamMode.Write))
                    using (var strWriterEncrypt = new StreamWriter(cryptoStrEncrypt)) 
                        strWriterEncrypt.Write(data);
                
                encryptedJson = memoStrEncrypt.ToArray();
            }

            File.WriteAllBytes(Path.Combine(FolderPath, fileName), encryptedJson);

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
        /// Permet de décrypter une partie d'un fichier JSON
        /// </summary>
        /// <param name="fileName">Le nom du fichier JSON</param>
        public object Decrypter(string fileName)
        {
            if (fileName is null) return null;

            if (!File.Exists(FilePath))
                return File.ReadAllText(Path.Combine(FolderPath, fileName));

            var encryptedJson =File.ReadAllBytes(Path.Combine(FolderPath, fileName)) ;
            if (encryptedJson.Length == LIST_MIN) return null;

            var key = Aes.Create();
            key.Padding = PaddingMode.Zeros;

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

            var decryptor = key.CreateDecryptor(key.Key, key.IV);

            string decryptedJson;
            using (var memoStrDecrypt = new MemoryStream(encryptedJson))
                using (var cryptoStrDecrypt = new CryptoStream(memoStrDecrypt, decryptor, CryptoStreamMode.Read))
                    using (var strReaderDecrypt = new StreamReader(cryptoStrDecrypt))
                        decryptedJson = strReaderDecrypt.ReadToEnd();

            key.Clear();
            return decryptedJson;
        }
    }
}
