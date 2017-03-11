namespace NoPassword.General
{
    public interface IStorage
    {
        void Set(string key, string value);
        string Get(string key);
    }
}