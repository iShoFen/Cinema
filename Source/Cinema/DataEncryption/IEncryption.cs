namespace DataEncryption
{
    public interface IEncryption
    {
        public string FolderPath { get; set; }

        public void Encrypter(object file, string fileName, string elementName = null);

        public object Decrypter(string fileName);
    }
}