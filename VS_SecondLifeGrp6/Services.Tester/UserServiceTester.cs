using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Api;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Models;
using VS_SLG6.Services.Services;
using VS_SLG6.Services.Validators;

namespace Services.Tester
{
    [TestClass]
    public class UserServiceTester : GenericServiceTester<User>
    {
        private const string TOO_LONG_STRING = "aaabbbcccdddeeefffggghhhiiijjjkkk";
        private User _user = new User();
        private User _user1 = new User();
        private User _user2 = new User();

        public UserServiceTester()
        {
            CreateInstances();
            InitBehavior(_user1, _user2, _user);
            InitTests();
        }

        private void CreateInstances()
        {
            _user.Id = 2;
            _user.Login = "John";
            _user.Password = "Smith";
            _user.Email = "john.smith@lecnam.net";
            _user.Name = "John Smith";

            _user1.Id = 0; 
            _user1.Login = "test"; 
            _user1.Password = "test"; 
            _user1.Email = "test@test.test"; 
            _user1.Name = "test";

            _user2.Id = 1; 
            _user2.Login = "test2"; 
            _user2.Password = "test2"; 
            _user2.Email = "test@test.test";
            _user2.Name = "test2";
        }

        public void InitTests()
        {
            _validator = new UserValidator(_repo.Object, new ValidationModel<bool>());
            _repo.Setup(x => x.FindOne(It.IsAny<int>())).Returns<int>(x => { 
                return _workingObjects.Find(u => u.Id == x);
            });

            var appSettings = Options.Create(new AppSettings { Key = "TestKey" });
            _service = new UserService(_repo.Object, _validator, appSettings);
            nullFields = new List<string> { "Login", "Name", "Password", "Email" };
        }

        [TestMethod]
        public void Add_WithBlankName_ThenError()
        {
            _user.Name = BLANK_STRING;
            var res = _service.Add(_user);
            Assert.AreNotEqual(0, res.Errors.Count);
        }

        [TestMethod]
        public void Add_WithTooLongName_ThenError()
        {
            _user.Name = TOO_LONG_STRING;
            var res = _service.Add(_user);
            Assert.AreNotEqual(0, res.Errors.Count);
        }

        [TestMethod]
        public void Add_WithBlankLogin_ThenError()
        {
            _user.Login = BLANK_STRING;
            var res = _service.Add(_user);
            Assert.AreNotEqual(0, res.Errors.Count);
        }

        [TestMethod]
        public void Add_WithTooLongLogin_ThenError()
        {
            _user.Login = TOO_LONG_STRING;
            var res = _service.Add(_user);
            Assert.AreNotEqual(0, res.Errors.Count);
        }

        [TestMethod]
        public void Add_WithBlankPassword_ThenError()
        {
            _user.Password = BLANK_STRING;
            var res = _service.Add(_user);
            Assert.AreNotEqual(0, res.Errors.Count);
        }

        [TestMethod]
        public void Add_WithTooLongPassword_ThenError()
        {
            _user.Password = TOO_LONG_STRING;
            var res = _service.Add(_user);
            Assert.AreNotEqual(0, res.Errors.Count);
        }

        [TestMethod]
        public void Add_WithInvalidMail_ThenError()
        {
            _user.Email = TOO_LONG_STRING;
            var res = _service.Add(_user);
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
            var res = _service.Get(0);
            Assert.AreEqual(_defaultObjects[1].Login, res.Value.Login);
        }

        [TestMethod]
        public void FindByMail_WithNull_ThenError()
        {
            var res = ((UserService)_service).Find(null);
            Assert.AreNotEqual(0, res.Errors.Count);
        }

        [TestMethod]
        public void FindByMail_WithUser_ThenUser()
        {
            _service.Add(_user);
            var res = ((UserService)_service).Find(_user.Email);
            Assert.AreEqual(_user, res.Value);
        }
    }
}
