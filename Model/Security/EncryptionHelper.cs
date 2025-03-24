using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Tenant.API.Base.Model.Security
{
    public class EncryptionHelper
    {
        private const string ENCRYPTION_KEY = "yDTsiiDYSBJcBbCcj4TOLmG0uFTVdCP12LiTWwKyxPkIMmEWaAz2rZR8lQJPZ0HSYbo2kkTJSGU6l5Vh49xRx6Rp6xtXxyvBHD2B6WdugKjBbl1ZZkDDXnm2CnKSmjhz";

        /// <summary>
        /// Encrypt the specified clearText.
        /// </summary>
        /// <returns>The encrypt.</returns>
        /// <param name="clearText">Clear text.</param>
        public static string Encrypt(string clearText)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(ENCRYPTION_KEY, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }

                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        /// <summary>
        /// Decrypt the specified cipherText.
        /// </summary>
        /// <returns>The decrypt.</returns>
        /// <param name="cipherText">Cipher text.</param>
        public static string Decrypt(string cipherText)
        {
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(ENCRYPTION_KEY, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }

                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }

            return cipherText;
        }
    }
}
