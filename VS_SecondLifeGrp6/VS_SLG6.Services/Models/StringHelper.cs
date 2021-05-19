using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace VS_SLG6.Services.Models
{
    public class StringHelper
    {
        public static string GetStringSha256Hash(string text)
        {
            if (String.IsNullOrEmpty(text)) return String.Empty;

            using (var sha = new SHA256Managed())
            {
                byte[] textData = Encoding.UTF8.GetBytes(text);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }

        public static ValidationModel<bool> StringIsEmptyOrBlank<T>(T obj, params string[] properties)
        {
            var res = new ValidationModel<bool>();
            res.Value = false;
            var list = GetPropsValues(obj, properties);
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != null && Regex.Replace(list[i], " +", "").Length == 0)
                {
                    res.Value = true;
                    res.Errors.Add(properties[i]);
                }
            }
            return res;
        }

        public static ValidationModel<bool> StringIsLongerThanMax<T>(T obj, int max, params string[] properties)
        {
            var res = new ValidationModel<bool>();
            res.Value = false;
            var list = GetPropsValues(obj, properties);
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != null && list[i].Length > max)
                {
                    res.Value = true;
                    res.Errors.Add(properties[i]);
                }
            }
            return res;
        }

        public static List<string> GetPropsValues<T>(T obj, params string[] properties)
        {
            var props = new List<PropertyInfo>(obj.GetType().GetProperties());
            return props.Aggregate(new List<string>(), (acc, item) =>
            {
                if (properties.Contains(item.Name))
                {
                    acc.Add(item.GetValue(obj)?.ToString());
                }
                return acc;
            });
        }
    }
}
