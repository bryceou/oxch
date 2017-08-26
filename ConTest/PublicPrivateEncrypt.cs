using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConTest
{
    public class PublicPrivateEncrypt
    {
        public static string Encrypt(string publicKey, string data)
        {
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider();
            rsaProvider.ImportCspBlob(Convert.FromBase64String(publicKey));
            byte[] plainBytes = Encoding.UTF8.GetBytes(data);
            byte[] encryptedBytes = rsaProvider.Encrypt(plainBytes, false);

            return Convert.ToBase64String(encryptedBytes);
        }

        public static string Decrypt(string privateKey, string encryptedData)
        {
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider();
            byte[] encryptedBytes = Convert.FromBase64String(encryptedData);
            rsaProvider.ImportCspBlob(Convert.FromBase64String(privateKey));
            byte[] plainBytes = rsaProvider.Decrypt(encryptedBytes, false);
            string plainText = Encoding.UTF8.GetString(plainBytes, 0, plainBytes.Length);

            return plainText;
        }

        public static void Test()
        {
            CspParameters cspParams = new CspParameters { ProviderType = 1 };
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(1024, cspParams);
            string publicKey = Convert.ToBase64String(rsaProvider.ExportCspBlob(false));//生成公钥
            string privateKey = Convert.ToBase64String(rsaProvider.ExportCspBlob(true));//生成私钥

            string encryptResult = Encrypt(publicKey, "Hello");
            string decryptResult = Decrypt(privateKey, encryptResult);
            Console.WriteLine(decryptResult);
        }
    }
}
