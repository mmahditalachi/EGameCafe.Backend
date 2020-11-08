using EGameCafe.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EGameCafe.Infrastructure.Services
{
    public class IdGeneratorService : IIdGenerator
    {
        public Task<string> BasicIdGenerator(IDateTime dateTime, string configureValue)
        {
            DateTime curreentTime = dateTime.Now;

            string id = curreentTime.Hour.ToString("D2") + curreentTime.Minute.ToString("D2") + curreentTime.Second.ToString("D2") + configureValue;

            using (SHA256 sha256Hash = SHA256.Create())
            {

                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(id));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return Task.FromResult(builder.ToString());
            }
        }
    }
}
