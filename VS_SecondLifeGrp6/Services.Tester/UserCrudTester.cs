using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;

namespace Services.Tester
{
    [TestClass]
    public class UserCrudTester : GenericCrudTester<User>
    {
        public UserCrudTester()
        {
            var u = new User();
            u.Login = "test";
            SetDefaultObjects(u);
        }
    }
}
