using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace EmpireX.Save
{
    public class SimpleEncryptor : IEncryptor
    {
        private const string KEY = "EmpireX_Secret_Key_2026!";

        public string Encrypt(string data)
        {
            return AesEncrypt(data, KEY);
        }

        public string Decrypt(string data)
        {
            return AesDecrypt(data, KEY);
        }
        
        private string AesEncrypt(string plainText, string key)
        {
            using (Aes aes = Aes.Create())
            {
                byte[] keyBytes = new byte[32];
                Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(32)), keyBytes, 32);
                aes.Key = keyBytes;
                aes.IV = new byte[16];

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        private string AesDecrypt(string cipherText, string key)
        {
            using (Aes aes = Aes.Create())
            {
                byte[] keyBytes = new byte[32];
                Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(32)), keyBytes, 32);
                aes.Key = keyBytes;
                aes.IV = new byte[16];

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream(Convert.FromBase64String(cipherText)))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
