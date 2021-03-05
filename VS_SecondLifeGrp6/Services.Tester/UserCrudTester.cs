using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Services;

namespace Services.Tester
{
    [TestClass]
    public class UserCrudTester : GenericCrudTester<User>
    {
        public UserCrudTester()
        {
            var u = new User();
            u.Id = 0;
            u.Login = "test";
            var u2 = new User();
            u2.Id = 1;
            u2.Login = "test2";
            InitBehavior(u, u2);
            InitTests();
        }

        public void InitTests()
        {
            _repo.Setup(x => x.FindOne(It.IsAny<object[]>())).Returns<object[]>(x => { 
                return _workingObjects.Find(u => u.Id == Int32.Parse(x[0].ToString()));
            });

            _service = new UserService(_repo.Object, _validator.Object);

            _validator.Setup(x => x.canAdd(It.IsAny<User>())).Returns<User>(x =>
            {
                if (x == null) return false;
                return _workingObjects.FindAll(u => u.Login == x.Login).Count == 0;
            });

            
        }

        [TestMethod]
        public void Patch_WithObject0LoginBecomesObject1Login_ThenObject1Login()
        {
            var o = new Operation<User>();
            o.op = "replace";
            o.path = "/login";
            o.value = _defaultObjects[1].Login;
            var p = new JsonPatchDocument<User>(new List<Operation<User>> { _operation }, new DefaultContractResolver());
            _service.Patch(0, p);
            Assert.AreEqual(_defaultObjects[1].Login, _workingObjects[0].Login);
        }
    }
}
