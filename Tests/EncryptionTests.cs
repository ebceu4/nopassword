using NoPassword.General;
using NUnit.Framework;

namespace NoPassword.Tests
{
    public class EncryptionTests
    {
        [Test]
        public void Foo()
        {
            var aesEncryption = new AesEncryptionWithRsaKeyEncryption();
            var stringToEncrypt = "String";

            var encryptedItem = aesEncryption.Encrypt(stringToEncrypt, "keystore_id");
            var decryptedString = aesEncryption.Decrypt(encryptedItem, "keystore_id");

            Assert.AreEqual(stringToEncrypt, decryptedString);
        }
    }
}