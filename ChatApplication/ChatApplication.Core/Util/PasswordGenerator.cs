using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ChatApplication.Core.Util
{
    public static class PasswordGenerator
    {
        public static byte[] ComputeHash(string value, byte[] salt)
        {
            byte[] valueBytes = Encoding.UTF8.GetBytes(value);
            // Hash password
            byte[] hashedPassword = ComputeHash(valueBytes);
            // Add salt
            byte[] saltedValue = hashedPassword.Concat(salt).ToArray();
            // Hash password again with salt
            return ComputeHash(saltedValue);
        }

        public static byte[] ComputeHash(byte[] value)
        {
            using SHA256Managed sha256gen = new SHA256Managed();
            return sha256gen.ComputeHash(value);
        }

        public static bool ConfirmPassword(string password, byte[] salt, byte[] hash)
            => hash.SequenceEqual(ComputeHash(password, salt));

        public static byte[] GenerateSalt()
            => Guid.NewGuid().ToByteArray();

        public static (byte[] salt, byte[] password) ComputePassword(string password)
        {
            byte[] salt = GenerateSalt();
            return (salt, ComputeHash(password, salt));
        }

        public static string GeneratePassword(int length)
        {
            using RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] randomBytes = new byte[length];
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
