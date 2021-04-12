using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace VS_SLG6.Services.Models
{
    public class ValidationModel<T> where T : notnull
    {
        public T Value { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public static string GetHashString(string inputString)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(inputString, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            return Convert.ToBase64String(hashBytes);
        }

        public static bool AreHashEqual(string input, string saved)
        {
            byte[] hashBytes = Convert.FromBase64String(saved);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(input, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i]) return false;
            }
            return true;
        }
    }
}
