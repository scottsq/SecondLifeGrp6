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
        private Message message = new Message();
        private const string BlankString = "     ";
        private User us1;
        private User ur1;
        private User ur2;

        public MessageServiceTester()
        {
            us1 = new User(); us1.Id = 0;
            ur1 = new User(); ur1.Id = 1;
            ur2 = new User(); ur2.Id = 2;
            var m1 = new Message();
            m1.Id = 0;
            m1.Content = "Test";
            m1.CreationDate = DateTime.Now;
            m1.Receipt = ur1;
            m1.Sender = us1;
            var m2 = new Message();
            m2.Id = 1;
            m2.Content = "Test 2";
            m2.CreationDate = DateTime.Now.AddDays(1);
            m2.Receipt = ur2;
            m2.Sender = us1;
            message.Id = 2;
            message.Content = "Hello World!";
            message.CreationDate = DateTime.Now;
            message.Receipt = ur1;
            message.Sender = us1;
            InitBehavior(m1, m2);
            InitTests();
        }

        private void InitTests()
        {
            var uService = new Mock<UserService>(new Mock<IRepository<User>>().Object, new Mock<IValidator<User>>().Object);
            uService.Setup(x => x.Get(It.IsAny<int>())).Returns<int>(x =>
            {
                if (x == us1.Id) return us1;
                if (x == ur1.Id) return ur1;
                if (x == ur2.Id) return ur2;
                return null;
            });

            _validator = new MessageValidator(_repo.Object, new ValidationModel<bool>(), uService.Object);
            _repo.Setup(x => x.FindOne(It.IsAny<object[]>())).Returns<object[]>(x => {
                return _workingObjects.Find(m => m.Id == Int32.Parse(x[0].ToString()));
            });

            _service = new MessageService(_repo.Object, _validator);
            nullFields = new List<string> { "Content", "Receipt", "Sender" };
        }

        [TestMethod]
        public void Add_WithNoContent_ThenError()
        {
            message.Content = BlankString;
            var res = _service.Add(message);
            Assert.AreNotEqual(0, res.Errors.Count);
        }

        [TestMethod]
        public void Add_WithNoReceipt_ThenError()
        {
            message.Receipt = null;
            var res = _service.Add(message);
            Assert.AreNotEqual(0, res.Errors.Count);
        }

        [TestMethod]
        public void Add_WithNoSender_ThenError()
        {
            message.Sender = null;
            var res = _service.Add(message);
            Assert.AreNotEqual(0, res.Errors.Count);
        }

        [TestMethod]
        public void Add_WithMinCreationDate_ThenNow()
        {
            message.CreationDate = DateTime.Now;
            _defaultObjects.Add(message);
            message.CreationDate = DateTime.MinValue;
            var res = _service.Add(message);
            var d = res.Value.CreationDate;
            Assert.AreEqual(DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day, d.Year + d.Month + d.Day);
        }

        [TestMethod]
        public void GetConversation_WithSender1AndReceipt1_ThenListNotEmpty()
        {
            var m = _service.Get(0);
            var res = ((MessageService)_service).GetConversation(m.Sender.Id, m.Receipt.Id);
            Assert.AreNotEqual(0, res.Count);
        }

        [TestMethod]
        public void GetConversation_WithSender1AndReceipt2_ThenListIsEmpty()
        {
            var res = ((MessageService)_service).GetConversation(_service.Get(0).Sender.Id, _defaultObjects[1].Receipt.Id);
            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        public void GetConversation_WithReceipt1AndSender1_ThenListNotEmpty()
        {
            var m = _service.Get(0);
            var res = ((MessageService)_service).GetConversation(m.Receipt.Id, m.Sender.Id);
            Assert.AreNotEqual(0, res.Count);
        }

        [TestMethod]
        public void GetConversations_WithSender1_ThenListNotEmpty()
        {
            var res = ((MessageService)_service).GetConversations(_service.Get(0).Sender.Id);
            Assert.AreNotEqual(0, res.Count);
        }

        [TestMethod]
        public void GetConversations_WithReceipt1_ThenListNotEmpty()
        {
            var res = ((MessageService)_service).GetConversations(_service.Get(0).Receipt.Id);
            Assert.AreNotEqual(0, res.Count);
        }

        [TestMethod]
        public void GetConversations_WithReceipt2_ThenListEmpty()
        {
            var res = ((MessageService)_service).GetConversations(_defaultObjects[1].Receipt.Id);
            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        public void LastMessage_WithSender1AndReceipt1_ThenNotNull()
        {
            var m = _service.Get(0);
            var res = ((MessageService)_service).LastMessage(m.Sender.Id, m.Receipt.Id);
            Assert.AreNotEqual(null, res);
        }

        [TestMethod]
        public void LastMessage_WithReceipt1AndSender1_ThenNotNull()
        {
            var m = _service.Get(0);
            var res = ((MessageService)_service).LastMessage(m.Receipt.Id, m.Sender.Id);
            Assert.AreNotEqual(null, res);
        }

        [TestMethod]
        public void LastMessage_WithSender1AndReceipt2_ThenNull()
        {
            var res = ((MessageService)_service).LastMessage(_service.Get(0).Sender.Id, _defaultObjects[1].Receipt.Id);
            Assert.AreEqual(null, res);
        }
    }
}
