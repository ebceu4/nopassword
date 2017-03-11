using System.IO;

namespace NoPassword.General
{
    public class EncryptedFileStorage : IStorage
    {
        private readonly IEncryption _encryption;

        public EncryptedFileStorage(IEncryption encryption)
        {
            _encryption = encryption;
        }

        public void Set(string key, string value)
        {
            var path = Path.Combine(Path.GetTempPath(), key + ".tmp");

            using (var fileStream = File.OpenWrite(path))
            {
                _encryption.Encrypt(key, fileStream);
            }
        }

        public string Get(string key)
        {
            throw new System.NotImplementedException();
        }
    }
}