using System.IO;
using System.Security.Cryptography;

namespace NoPassword.General
{
    public class AesEncryption : IEncryption
    {
        public void Encrypt(string value, Stream output)
        {
            using (var aesAlg = new AesManaged())
            {
                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var csEncrypt = new CryptoStream(output, encryptor, CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(value);
                    }
                }
            }
        }
    }
}