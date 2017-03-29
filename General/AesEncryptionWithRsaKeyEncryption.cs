using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace NoPassword.General
{
    public class EncryptedItem
    {
        public EncryptedItem(byte[] encryptedAesKeyIvPair, byte[] data)
        {
            EncryptedAesKeyIVPair = encryptedAesKeyIvPair;
            Data = data;
        }

        public byte[] EncryptedAesKeyIVPair { get;  }
        public byte[] Data { get; }
    }

    public class AesEncryptionWithRsaKeyEncryption : IEncryption
    {
        public EncryptedItem Encrypt(string value, string windowsKeystoreId)
        {
            using (var aesAlg = new AesManaged {Padding = PaddingMode.ANSIX923})
            {
                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                var aesKeyIVPair = new byte[aesAlg.Key.Length + aesAlg.IV.Length];

                Array.Copy(aesAlg.Key, aesKeyIVPair, aesAlg.Key.Length);
                Array.Copy(aesAlg.IV, 0, aesKeyIVPair, aesAlg.Key.Length, aesKeyIVPair.Length - aesAlg.Key.Length);

                using (var memoryStream = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        var bytes = Encoding.UTF8.GetBytes(value);
                        csEncrypt.Write(bytes, 0, bytes.Length);
                        csEncrypt.FlushFinalBlock();

                        var buffer = memoryStream.GetBuffer();
                        var data = new byte[memoryStream.Length];
                        Array.Copy(buffer, data, memoryStream.Length);

                        var cspParameters = new CspParameters {KeyContainerName = windowsKeystoreId};

                        using (var csp = new RSACryptoServiceProvider(cspParameters))
                        {
                            csp.PersistKeyInCsp = true;

                            return new EncryptedItem(
                                csp.Encrypt(aesKeyIVPair, false),
                                data
                            );
                        }
                    }
                }
            }
        }

        public string Decrypt(EncryptedItem encryptedItem, string windowsKeystoreId)
        {
            var cp = new CspParameters {KeyContainerName = windowsKeystoreId};

            using (var csp = new RSACryptoServiceProvider(cp))
            {
                var aesKeyIVPair = csp.Decrypt(encryptedItem.EncryptedAesKeyIVPair, false);
                var aesKey = new byte[32];
                var aesIV = new byte[16];

                Array.Copy(aesKeyIVPair, aesKey, aesKey.Length);
                Array.Copy(aesKeyIVPair, aesKey.Length, aesIV, 0, aesKeyIVPair.Length - aesKey.Length);

                using (var aesAlg = new AesManaged {Padding = PaddingMode.ANSIX923})
                {
                    var decryptor = aesAlg.CreateDecryptor(aesKey, aesIV);
                    var ms = new MemoryStream(encryptedItem.Data);

                    using (var csEncrypt = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (var reader = new StreamReader(csEncrypt))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}