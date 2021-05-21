using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VS_SLG6.Model.Entities;

namespace Services.Tester.Factories
{
    public static class UserFactory
    {
        public static User GenericUser1 = new User();
        public static User GenericUser2 = new User();
        public static User GenericUser3 = new User();

        // Generating errors --------------------
        public static User BlankNameUser = new User();
        public static User BlankLoginUser = new User();
        public static User BlankPasswordUser = new User();
        public static User TooLongNameUser = new User();
        public static User TooLongLoginUser = new User();
        public static User TooLongPasswordUser = new User();
        public static User InvalidMailUser = new User();
        public static User UnknownUser = new User();

        public static void InitFactory()
        {
            var list = All();
            var props = typeof(UserFactory).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            for (int i=0; i<list.Count; i++)
            {
                var propName = props[i].Name;
                var u = list[i];
                u.Id = i;
                u.Login = u.Name = u.Password = propName;
                u.Email = String.Format("{0}@mail.com", propName);
            }
            BlankLoginUser.Login = BlankNameUser.Name = BlankPasswordUser.Password = String.Empty;
            TooLongLoginUser.Login = TooLongNameUser.Name = TooLongPasswordUser.Password = new string('a', Int16.MaxValue);
            InvalidMailUser.Email = "Invalid.Mail.Format";
            UnknownUser.Id = -1;
        }

        public static List<User> All()
        {
            return GenericList().Concat(ErrorList()).ToList();
        }

        public static List<User> GenericList()
        {
            return new List<User> {
                GenericUser1,
                GenericUser2,
                GenericUser3
            };
        }

        public static List<User> ErrorList()
        {
            return new List<User> {
                BlankNameUser,
                BlankLoginUser,
                BlankPasswordUser,
                TooLongLoginUser,
                TooLongNameUser,
                TooLongPasswordUser,
                InvalidMailUser
            };
        }
    }
}
