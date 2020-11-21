using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EGameCafe.Application.Common.Interfaces
{
    public interface IIdGenerator
    {
        Task<string> BasicIdGenerator(IDateTime dateTime, string configureValue = "TestApp");
        Task<string> SHA1hashGenerator(string randomString);
        string EncryptData(string textData, string Encryptionkey);
        string DecryptData(string EncryptedText, string Encryptionkey);
    }
}
