using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ERP.Complementos.ProcesoBehind
{
    public static class EncryptionHelper
    {
        private static readonly byte[] Salt = Encoding.ASCII.GetBytes("502903MyKeyACM");

        public static string EncryptPassword(string password, string key)
        {
            byte[] encryptedBytes;
            using (Aes aes = Aes.Create())
            {
                byte[] keyBytes = new Rfc2898DeriveBytes(key, Salt, 1000).GetBytes(32);
                aes.Key = keyBytes;
                aes.GenerateIV(); // Generar un IV único y aleatorio
                byte[] ivBytes = aes.IV;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Escribir el IV al inicio de la secuencia de memoria
                    memoryStream.Write(ivBytes, 0, ivBytes.Length);

                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] plainBytes = Encoding.UTF8.GetBytes(password);
                        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        encryptedBytes = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encryptedBytes);
        }

        public static string DecryptPassword(string encryptedPassword, string key)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedPassword);
            string decryptedPassword;
            using (Aes aes = Aes.Create())
            {
                byte[] keyBytes = new Rfc2898DeriveBytes(key, Salt, 1000).GetBytes(32);
                aes.Key = keyBytes;

                // Leer el IV desde los primeros bytes de los datos cifrados
                byte[] ivBytes = new byte[16];
                Buffer.BlockCopy(encryptedBytes, 0, ivBytes, 0, 16);
                aes.IV = ivBytes;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream(encryptedBytes))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        byte[] decryptedBytes = new byte[encryptedBytes.Length - 16];
                        int decryptedByteCount = cryptoStream.Read(decryptedBytes, 0, decryptedBytes.Length);
                        decryptedPassword = Encoding.UTF8.GetString(decryptedBytes, 0, decryptedByteCount);
                    }
                }
            }

            return decryptedPassword;
        }
    }
}