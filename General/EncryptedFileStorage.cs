using System.IO;

namespace NoPassword.General
{
    public class EncryptedFileStorage : IStorage
    {
        private const string WindowsKeystoreId = "3e9dd36c2e7c401e87a993077fd29600";
        private readonly IEncryption _encryption;

        public EncryptedFileStorage(IEncryption encryption)
        {
            _encryption = encryption;
        }

        public bool ContainsKey(string key)
        {
            var path = Path.Combine(Path.GetTempPath(), key + ".tmp");
            return File.Exists(path);
        }

        public void Set(string key, string value)
        {
            var dataPath = Path.Combine(Path.GetTempPath(), key + ".tmp");
            var keyPath = Path.Combine(Path.GetTempPath(), key + ".key");

            var encryptedItem = _encryption.Encrypt(value, WindowsKeystoreId);

            using (var fileStream = File.OpenWrite(dataPath))
            {
                fileStream.Write(encryptedItem.Data, 0, encryptedItem.Data.Length);
            }

            using (var fileStream = File.OpenWrite(keyPath))
            {
                fileStream.Write(encryptedItem.EncryptedAesKeyIVPair, 0, encryptedItem.EncryptedAesKeyIVPair.Length);
            }
        }

        public string Get(string key)
        {
            var dataPath = Path.Combine(Path.GetTempPath(), key + ".tmp");
            var keyPath = Path.Combine(Path.GetTempPath(), key + ".key");

            var encryptedAesKeyIvPair = File.ReadAllBytes(keyPath);
            var data = File.ReadAllBytes(dataPath);

            return _encryption.Decrypt(new EncryptedItem(encryptedAesKeyIvPair, data), WindowsKeystoreId);
        }
    }
}