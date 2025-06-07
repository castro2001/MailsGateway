using System.Text;
using System.Security.Cryptography;

namespace Shared.Helper
{
    public static class CryptoHelper
    {
        private static readonly string key = "12345678901234567890123456789012"; // 32 caracteres ASCII puros

        public static string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.GenerateIV();

            var encryptor = aes.CreateEncryptor();
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            var result = Convert.ToBase64String(aes.IV.Concat(cipherBytes).ToArray());
            return Uri.EscapeDataString(result); // Para usarlo seguro en la URL
        }

        public static string Decrypt(string cipherText)
        {
            var fullCipher = Convert.FromBase64String(Uri.UnescapeDataString(cipherText));

            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);

            var iv = fullCipher.Take(16).ToArray();
            var cipher = fullCipher.Skip(16).ToArray();

            aes.IV = iv;
            var decryptor = aes.CreateDecryptor();
            var plainBytes = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);

            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
