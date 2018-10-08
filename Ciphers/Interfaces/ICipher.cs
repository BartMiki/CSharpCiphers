using System;
using System.Collections.Generic;
using System.Text;

namespace Ciphers.Interfaces
{
    public interface ICipher
    {
        string Encrypt(string message);
        string Decrypt(string encrypted);
    }
}
