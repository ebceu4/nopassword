namespace NoPassword.General
{
    public interface IEncryption
    {
        EncryptedItem Encrypt(string value, string windowsKeystoreId);
        string Decrypt(EncryptedItem item, string windowsKeystoreId);
    }
}