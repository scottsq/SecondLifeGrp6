using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using VS_SLG6.Services.Interfaces;

namespace VS_SLG6.Services.Models
{
    public class AccessManager : IAccessManager
    {

        // sha256 pour le hashage dans une autre classe + interface

        public string GetHashString(string inputString)
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

        public bool AreHashEqual(string input, string saved)
        {
            return input == saved;
            /*var test1 = GetHashString("toto");
            var test2 = test1 == GetHashString("toto");
            byte[] hashBytes = Convert.FromBase64String(saved);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(input, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i]) return false;
            }
            return true;*/
        }

        public string GetStringSha256Hash(string text)
        {
            if (String.IsNullOrEmpty(text)) return String.Empty;

            using (var sha = new SHA256Managed())
            {
                byte[] textData = Encoding.UTF8.GetBytes(text);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }
    }
}
