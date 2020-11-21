using EGameCafe.Application.Common.Exceptions;
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

        //SHA1 hashcode for group sharing link 
        public Task<string> SHA1hashGenerator(string randomString)
        {
            using (SHA1 sha1Hash = SHA1.Create())
            {

                string id = Guid.NewGuid().ToString() + randomString;

                byte[] bytes = sha1Hash.ComputeHash(Encoding.UTF8.GetBytes(id));

                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return Task.FromResult(builder.ToString());
            }
        }

        //AES encryption
        public string EncryptData(string textData, string Encryptionkey)
        {
            RijndaelManaged objrij = new RijndaelManaged();

            objrij.Mode = CipherMode.CBC;

            objrij.Padding = PaddingMode.PKCS7;

            objrij.KeySize = 0x80;

            objrij.BlockSize = 0x80;

            byte[] passBytes = Encoding.UTF8.GetBytes(Encryptionkey);

            byte[] EncryptionkeyBytes = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            int len = passBytes.Length;
            if (len > EncryptionkeyBytes.Length)
            {
                len = EncryptionkeyBytes.Length;
            }

            Array.Copy(passBytes, EncryptionkeyBytes, len);

            objrij.Key = EncryptionkeyBytes;
            objrij.IV = EncryptionkeyBytes;

            ICryptoTransform objtransform = objrij.CreateEncryptor();
            byte[] textDataByte = Encoding.UTF8.GetBytes(textData);

            return Convert.ToBase64String(objtransform.TransformFinalBlock(textDataByte, 0, textDataByte.Length));
        }

        public string DecryptData(string EncryptedText, string Encryptionkey)
        {
            try
            {
                RijndaelManaged objrij = new RijndaelManaged();
                objrij.Mode = CipherMode.CBC;
                objrij.Padding = PaddingMode.PKCS7;

                objrij.KeySize = 0x80;
                objrij.BlockSize = 0x80;

                byte[] encryptedTextByte = Convert.FromBase64String(EncryptedText);
                byte[] passBytes = Encoding.UTF8.GetBytes(Encryptionkey);
                byte[] EncryptionkeyBytes = new byte[0x10];

                int len = passBytes.Length;
                if (len > EncryptionkeyBytes.Length)
                {
                    len = EncryptionkeyBytes.Length;
                }

                Array.Copy(passBytes, EncryptionkeyBytes, len);
                objrij.Key = EncryptionkeyBytes;
                objrij.IV = EncryptionkeyBytes;
                byte[] TextByte = objrij.CreateDecryptor().TransformFinalBlock(encryptedTextByte, 0, encryptedTextByte.Length);

                return Encoding.UTF8.GetString(TextByte);
            }
            catch (Exception)
            {
                throw new InvalidTokenException("لینک دعوت صحیح نمی باشد");
            }
        }
    }
}
