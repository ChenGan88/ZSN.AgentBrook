using System;
using System.Security.Cryptography;
using System.Text;

namespace ZSN.Utils.Core
{
    public class Encryption
    {
        private static string encryptKey = "4h!@w$rng,i#$@x1%)5^3(7*5P31/Ee0";
        private static byte[] Keys = new byte[16] { (byte)65, (byte)114, (byte)101, (byte)121, (byte)111, (byte)117, (byte)109, (byte)121, (byte)83, (byte)110, (byte)111, (byte)119, (byte)109, (byte)97, (byte)110, (byte)63 };

        public static string Encrypt(string encryptString)
        {
            if (string.IsNullOrEmpty(encryptString))
                return string.Empty;
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            rijndaelManaged.Key = Encoding.UTF8.GetBytes(Encryption.encryptKey.Substring(0, 32));
            rijndaelManaged.IV = Encryption.Keys;
            ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor();
            byte[] bytes = Encoding.UTF8.GetBytes(encryptString);
            byte[] inputBuffer = bytes;
            int inputOffset = 0;
            int length = bytes.Length;
            return Convert.ToBase64String(encryptor.TransformFinalBlock(inputBuffer, inputOffset, length));
        }

        public static string Decrypt(string decryptString)
        {
            if (string.IsNullOrEmpty(decryptString))
                return string.Empty;
            try
            {
                RijndaelManaged rijndaelManaged = new RijndaelManaged();
                rijndaelManaged.Key = Encoding.UTF8.GetBytes(Encryption.encryptKey.Substring(0, 32));
                rijndaelManaged.IV = Encryption.Keys;
                ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor();
                byte[] numArray = Convert.FromBase64String(decryptString);
                byte[] inputBuffer = numArray;
                int inputOffset = 0;
                int length = numArray.Length;
                return Encoding.UTF8.GetString(decryptor.TransformFinalBlock(inputBuffer, inputOffset, length));
            }
            catch
            {
                return "";
            }
        }
    }
}
