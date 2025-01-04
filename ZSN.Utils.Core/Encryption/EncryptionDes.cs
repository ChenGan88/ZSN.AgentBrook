using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ZSN.Utils.Core
{
    public class EncryptionDes
    {
        private static string Key = "X!xO@kA)";

        public static string Encrypt(string pToEncrypt)
        {
            if (string.IsNullOrWhiteSpace(pToEncrypt))
                return string.Empty;
            try
            {
                DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider();
                byte[] bytes = Encoding.Default.GetBytes(pToEncrypt);
                cryptoServiceProvider.Key = Encoding.ASCII.GetBytes(EncryptionDes.Key);
                cryptoServiceProvider.IV = Encoding.ASCII.GetBytes(EncryptionDes.Key);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, cryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write);
                cryptoStream.Write(bytes, 0, bytes.Length);
                cryptoStream.FlushFinalBlock();
                StringBuilder stringBuilder = new StringBuilder();
                foreach (byte num in memoryStream.ToArray())
                    stringBuilder.AppendFormat("{0:X2}", (object)num);
                stringBuilder.ToString();
                return stringBuilder.ToString();
            }
            catch
            {
                return "";
            }
        }

        public static string Decrypt(string pToDecrypt)
        {
            if (string.IsNullOrWhiteSpace(pToDecrypt))
                return string.Empty;
            try
            {
                DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider();
                byte[] buffer = new byte[pToDecrypt.Length / 2];
                for (int index = 0; index < pToDecrypt.Length / 2; ++index)
                {
                    int int32 = Convert.ToInt32(pToDecrypt.Substring(index * 2, 2), 16);
                    buffer[index] = (byte)int32;
                }
                cryptoServiceProvider.Key = Encoding.ASCII.GetBytes(EncryptionDes.Key);
                cryptoServiceProvider.IV = Encoding.ASCII.GetBytes(EncryptionDes.Key);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, cryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Write);
                cryptoStream.Write(buffer, 0, buffer.Length);
                cryptoStream.FlushFinalBlock();
                StringBuilder stringBuilder = new StringBuilder();
                return Encoding.Default.GetString(memoryStream.ToArray());
            }
            catch
            {
                return "";
            }
        }
    }
}
