using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Tester.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using VS_SLG6.Api;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Services;
using VS_SLG6.Services.Validators;

namespace Services.Tester.ServiceTesters
{
    [TestClass]
    public class UserServiceTester : GenericServiceTester<User>
    {
        public UserServiceTester()
        {
            UserFactory.InitFactory();
            InitTests();
        }

        public void InitTests()
        {
            Func<object[], User> findOneFunc = x => _workingObjects.Find(u => u.Id == Convert.ToInt32(x[0]));
            InitBehavior(findOneFunc, UserFactory.GenericList());

            _validator = new UserValidator(_repo.Object);
            var appSettings = Options.Create(new AppSettings { Key = "TestKey" });
            _service = new UserService(_repo.Object, _validator, appSettings);
            _errorObjects = UserFactory.ErrorList();
            _nullFields = new List<string> { "Login", "Name", "Password", "Email" };
            _fieldOrderBy = nameof(User.Name);
        }

        [TestMethod]
        public void Find_WithName2_Then1ResultAndGenericUser2()
        {
            var u = UserFactory.GenericUser2;
            var res = (_service as UserService).Find(name: u.Name);
            Assert.IsTrue(res.Count == 1 && res[0].Id == u.Id);
        }

        [TestMethod]
        public void Find_WithLoginAYAYA_ThenEmpty()
        {
            var res = (_service as UserService).Find(login: "AYAYA");
            Assert.IsFalse(res.Any());
        }
    }
}
