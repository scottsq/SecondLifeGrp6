using System;
using System.Collections.Generic;
using System.Text;

namespace VS_SLG6.Services.Interfaces
{
    public interface IAccessManager
    {
        public String GetHashString(string inputString);
        public bool AreHashEqual(string input, string saved);
        public string GetStringSha256Hash(string text);
    }
}
