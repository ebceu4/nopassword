using System;
using System.IO;

namespace NoPassword.General
{
    public interface IEncryption
    {
        void Encrypt(string value, Stream output);
    }
}