using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BackEndPassManag.Service
{
    public class Crypto
    {
        private static AesManaged CreateAes(string key)
        {
            AesManaged aes = new AesManaged
            {
                Key = System.Text.Encoding.UTF8.GetBytes(key), //UTF8-Encoding
                IV = System.Text.Encoding.UTF8.GetBytes("encryptionIntVec")//UT8-Encoding
            };
            return aes;
        }
        public string Encrypt(string text, string key)
        {
            using (AesManaged aes = CreateAes(key))
            {
                ICryptoTransform encryptor = aes.CreateEncryptor();
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(text);
                        }

                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }
        public string Decrypt(string text, string key)
        {
            using (AesManaged aes = CreateAes(key))
            {
                ICryptoTransform decryptor = aes.CreateDecryptor();
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(text)))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(cs))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }

            }
        }

        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
            {
                return builder.ToString().ToLower();
            }

            return builder.ToString();
        }

    }
}
