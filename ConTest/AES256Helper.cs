using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConTest
{
    public class AES256Helper
    {

        private static int iterations = 5;//iterations smaller, speed faster

        private static byte[] AESEncryptBytes(byte[] clearBytes, byte[] passBytes, byte[] saltBytes)
        {
            byte[] encryptedBytes = null;

            // create a key from the password and salt, use 32K iterations – see note
            var key = new Rfc2898DeriveBytes(passBytes, saltBytes, iterations);

            // create an AES object
            using (Aes aes = new AesManaged())
            {
                // set the key size to 256
                aes.KeySize = 256;
                aes.Key = key.GetBytes(aes.KeySize / 8);
                aes.IV = key.GetBytes(aes.BlockSize / 8);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }
            return encryptedBytes;
        }

        private static byte[] AESDecryptBytes(byte[] cryptBytes, byte[] passBytes, byte[] saltBytes)
        {
            byte[] clearBytes = null;
            var key = new Rfc2898DeriveBytes(passBytes, saltBytes, iterations);

            using (Aes aes = new AesManaged())
            {
                // set the key size to 256
                aes.KeySize = 256;
                aes.Key = key.GetBytes(aes.KeySize / 8);
                aes.IV = key.GetBytes(aes.BlockSize / 8);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cryptBytes, 0, cryptBytes.Length);
                        cs.Close();
                    }
                    clearBytes = ms.ToArray();
                }
            }
            return clearBytes;
        }

        public static string EncryptString(string clearText, string passText, string saltText)
        {
            byte[] clearBytes = Encoding.UTF8.GetBytes(clearText);
            byte[] passBytes = Encoding.UTF8.GetBytes(passText);
            byte[] saltBytes = Encoding.UTF8.GetBytes(saltText);

            return Convert.ToBase64String(AESEncryptBytes(clearBytes, passBytes, saltBytes));
        }

        public static string DecryptString(string cryptText, string passText, string saltText)
        {
            byte[] cryptBytes = Convert.FromBase64String(cryptText);
            byte[] passBytes = Encoding.UTF8.GetBytes(passText);
            byte[] saltBytes = Encoding.UTF8.GetBytes(saltText);

            return Encoding.UTF8.GetString(AESDecryptBytes(cryptBytes, passBytes, saltBytes));
        }
    }
}
