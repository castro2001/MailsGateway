using System.Security.Cryptography;
using System.Text;
using Domain.Entidades.Seguridad;
using Microsoft.Extensions.Options;

namespace Shared.Helper
{
    public  class CryptoHelper
    {
        private  readonly byte[] _aesKey;
        private  readonly byte[] _hmacKey;

        public  CryptoHelper(IOptions<CryptoSettings> options)
        {
            _aesKey = Encoding.UTF8.GetBytes(options.Value.AESKey);
            _hmacKey = Encoding.UTF8.GetBytes(options.Value.HMACKey);

            if (_aesKey.Length != 32) throw new ArgumentException("AESKey debe tener 32 bytes.");
            if (_hmacKey.Length != 32) throw new ArgumentException("HMACKey debe tener 32 bytes.");
        }

        public  string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = _aesKey;
            aes.GenerateIV();

            var iv = aes.IV;
            var plainBytes = Encoding.UTF8.GetBytes(plainText);

            using var encryptor = aes.CreateEncryptor();
            var cipherText = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            var ivAndCipher = iv.Concat(cipherText).ToArray();

            // Generar HMAC
            var hmac = ComputeHMAC(ivAndCipher);
            var fullPayload = ivAndCipher.Concat(hmac).ToArray();

            return Convert.ToBase64String(fullPayload);
        }

        public string Decrypt(string encryptedText)
        {
            var fullPayload = Convert.FromBase64String(encryptedText);

            if (fullPayload.Length < 16 + 32)
                throw new CryptographicException("El mensaje cifrado no es válido.");

            var iv = fullPayload.Take(16).ToArray();
            var hmac = fullPayload.Skip(fullPayload.Length - 32).ToArray();
            var cipherText = fullPayload.Skip(16).Take(fullPayload.Length - 16 - 32).ToArray();

            var ivAndCipher = iv.Concat(cipherText).ToArray();
            var computedHmac = ComputeHMAC(ivAndCipher);

            if (!computedHmac.SequenceEqual(hmac))
                throw new CryptographicException("La integridad del mensaje ha sido comprometida.");

            using var aes = Aes.Create();
            aes.Key = _aesKey;
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor();
            var plainBytes = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);

            return Encoding.UTF8.GetString(plainBytes);
        }

        private byte[] ComputeHMAC(byte[] data)
        {
            using var hmacsha256 = new HMACSHA256(_hmacKey);
            return hmacsha256.ComputeHash(data);
        }
    }
}
