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
        private const string TooLongString = "aaabbbcccdddeeefffggghhhiiijjjkkk";
        private const string BlankString = "     ";
        private User user = new User();

        public UserCrudTester()
        {
            user.Login = "John";
            user.Password = "Smith";
            user.Email = "john.smith@lecnam.net";
            var u = new User();
            u.Id = 0;
            u.Login = "test";
            var u2 = new User();
            u2.Id = 1;
            u2.Login = "test2";
            InitBehavior(u, u2, user);
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
        public void Add_WithEmptyLogin_ThenError()
        {
            user.Login = null;
            var res = _service.Add(user);
            Assert.AreNotEqual(0, res.Errors.Count);
        }

        [TestMethod]
        public void Add_WithBlankLogin_ThenError()
        {
            user.Login = BlankString;
            var res = _service.Add(user);
            Assert.AreNotEqual(0, res.Errors.Count);
        }

        [TestMethod]
        public void Add_WithTooLongLogin_ThenError()
        {
            user.Login = TooLongString;
            var res = _service.Add(user);
            Assert.AreNotEqual(0, res.Errors.Count);
        }

        [TestMethod]
        public void Add_WithBlankPassword_ThenError()
        {
            user.Password = BlankString;
            var res = _service.Add(user);
            Assert.AreNotEqual(0, res.Errors.Count);
        }

        [TestMethod]
        public void Add_WithEmptyPassword_ThenError()
        {
            user.Password = null;
            var res = _service.Add(user);
            Assert.AreNotEqual(0, res.Errors.Count);
        }

        [TestMethod]
        public void Add_WithTooLongPassword_ThenError()
        {
            user.Password = TooLongString;
            var res = _service.Add(user);
            Assert.AreNotEqual(0, res.Errors.Count);
        }

        [TestMethod]
        public void Add_WithEmptydMail_ThenError()
        {
            user.Email = null;
            var res = _service.Add(user);
            Assert.AreNotEqual(0, res.Errors.Count);
        }

        [TestMethod]
        public void Add_WithInvalidMail_ThenError()
        {
            user.Email = TooLongString;
            var res = _service.Add(user);
            Assert.AreNotEqual(0, res.Errors.Count);
        }

        [TestMethod]
        public void Patch_WithObject1Login_ThenObject1Login()
        {
            var o = new Operation<User>();
            o.op = "replace";
            o.path = "/login";
            o.value = _defaultObjects[1].Login;
            var p = new JsonPatchDocument<User>(new List<Operation<User>> { o }, new DefaultContractResolver());
            _service.Patch(0, p);
            Assert.AreEqual(_defaultObjects[1].Login, _workingObjects[0].Login);
        }

        [TestMethod]
        public void FindByMail_WithNull_ThenError()
        {
            var res = ((UserService)_service).FindByMail(null);
            Assert.AreNotEqual(0, res.Errors.Count);
        }

        [TestMethod]
        public void FindByMail_WithUser_ThenUser()
        {
            _service.Add(user);
            var res = ((UserService)_service).FindByMail(user.Email);
            Assert.AreEqual(user, res.Value);
        }
    }
}
