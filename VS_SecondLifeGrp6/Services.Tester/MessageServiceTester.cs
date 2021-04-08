using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;
using VS_SLG6.Services.Services;
using VS_SLG6.Services.Validators;

namespace Services.Tester
{
    [TestClass]
    public class MessageServiceTester : GenericServiceTester<Message>
    {
        private Message _message = new Message();
        private Message _m1 = new Message();
        private Message _m2 = new Message();
        private User _uSender1 = new User();
        private User _uReceipt = new User();
        private User _uSender2 = new User();

        public MessageServiceTester()
        {
            CreateInstances();
            InitBehavior(_m1, _m2);
            InitTests();
        }

        private void CreateInstances()
        {
            _uSender1.Id = 0;
            _uReceipt.Id = 1;
            _uSender2.Id = 2;

            _m1.Id = 0;
            _m1.Content = "Test";
            _m1.CreationDate = DateTime.Now;
            _m1.Receipt = _uReceipt;
            _m1.Sender = _uSender1;

            _m2.Id = 1;
            _m2.Content = "Test 2";
            _m2.CreationDate = DateTime.Now.AddDays(1);
            _m2.Receipt = _uSender2;
            _m2.Sender = _uSender1;

            _message.Id = 2;
            _message.Content = "Hello World!";
            _message.CreationDate = DateTime.Now;
            _message.Receipt = _uReceipt;
            _message.Sender = _uSender1;
        }

        private void InitTests()
        {
            var uRepo = new Mock<IRepository<User>>();
            uRepo.Setup(x => x.FindOne(It.IsAny<int>())).Returns<int>(x =>
            {
                if (x == _uSender1.Id) return _uSender1;
                if (x == _uReceipt.Id) return _uReceipt;
                if (x == _uSender2.Id) return _uSender2;
                return null;
            });

            _validator = new MessageValidator(_repo.Object, new ValidationModel<bool>(), uRepo.Object);
            _repo.Setup(x => x.FindOne(It.IsAny<int>())).Returns<int>(x => {
                return _workingObjects.Find(m => m.Id == x);
            });

            _service = new MessageService(_repo.Object, _validator);
            nullFields = new List<string> { "Content", "Receipt", "Sender" };
        }

        [TestMethod]
        public void Add_WithBlankContent_ThenError()
        {
            _message.Content = BLANK_STRING;
            var res = _service.Add(_message);
            Assert.AreNotEqual(0, res.Errors.Count);
        }

        [TestMethod]
        public void Add_WithMinCreationDate_ThenNow()
        {
            _message.CreationDate = DateTime.Now;
            _defaultObjects.Add(_message);
            _message.CreationDate = DateTime.MinValue;
            var res = _service.Add(_message);
            var d = res.Value.CreationDate;
            Assert.AreEqual(DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day, d.Year + d.Month + d.Day);
        }

        [TestMethod]
        public void Add_WithUnknownSender_ThenError()
        {
            var u = new User(); u.Id = -1;
            _message.Sender = u;
            var res = _service.Add(_message);
            Assert.AreNotEqual(0, res.Errors.Count);
        }

        [TestMethod]
        public void Add_WithUnknownReceipt_ThenError()
        {
            var u = new User(); u.Id = -1;
            _message.Receipt = u;
            var res = _service.Add(_message);
            Assert.AreNotEqual(0, res.Errors.Count);
        }

        [TestMethod]
        public void GetConversation_WithSender1AndReceipt1_ThenListNotEmpty()
        {
            var m = _service.Get(0).Value;
            var res = ((MessageService)_service).GetConversation(m.Sender.Id, m.Receipt.Id);
            Assert.AreNotEqual(0, res.Value.Count);
        }

        [TestMethod]
        public void GetConversation_WithSender1AndReceipt2_ThenListIsEmpty()
        {
            var res = ((MessageService)_service).GetConversation(_service.Get(0).Value.Sender.Id, _defaultObjects[1].Receipt.Id);
            Assert.AreEqual(0, res.Value.Count);
        }

        [TestMethod]
        public void GetConversation_WithReceipt1AndSender1_ThenListNotEmpty()
        {
            var m = _service.Get(0).Value;
            var res = ((MessageService)_service).GetConversation(m.Receipt.Id, m.Sender.Id);
            Assert.AreNotEqual(0, res.Value.Count);
        }

        [TestMethod]
        public void ListConversations_WithSender1_ThenListNotEmpty()
        {
            var res = ((MessageService)_service).ListConversations(_service.Get(0).Value.Sender.Id);
            Assert.AreNotEqual(0, res.Value.Count);
        }

        [TestMethod]
        public void ListConversations_WithReceipt1_ThenListNotEmpty()
        {
            var res = ((MessageService)_service).ListConversations(_service.Get(0).Value.Receipt.Id);
            Assert.AreNotEqual(0, res.Value.Count);
        }

        [TestMethod]
        public void ListConversations_WithReceipt2_ThenListEmpty()
        {
            var res = ((MessageService)_service).ListConversations(_defaultObjects[1].Receipt.Id);
            Assert.AreEqual(0, res.Value.Count);
        }

        [TestMethod]
        public void LastMessage_WithSender1AndReceipt1_ThenNotNull()
        {
            var m = _service.Get(0).Value;
            var res = ((MessageService)_service).LastMessage(m.Sender.Id, m.Receipt.Id);
            Assert.AreNotEqual(null, res.Value);
        }

        [TestMethod]
        public void LastMessage_WithReceipt1AndSender1_ThenNotNull()
        {
            var m = _service.Get(0).Value;
            var res = ((MessageService)_service).LastMessage(m.Receipt.Id, m.Sender.Id);
            Assert.AreNotEqual(null, res.Value);
        }

        [TestMethod]
        public void LastMessage_WithSender1AndReceipt2_ThenNull()
        {
            var res = ((MessageService)_service).LastMessage(_service.Get(0).Value.Sender.Id, _defaultObjects[1].Receipt.Id);
            Assert.AreEqual(null, res.Value);
        }
    }
}
