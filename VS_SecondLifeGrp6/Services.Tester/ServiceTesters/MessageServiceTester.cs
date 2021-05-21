using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services.Tester.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Interfaces;
using VS_SLG6.Services.Services;
using VS_SLG6.Services.Validators;

namespace Services.Tester.ServiceTesters
{
    [TestClass]
    public class MessageServiceTester : GenericServiceTester<Message>
    {
        public MessageServiceTester()
        {
            MessageFactory.InitFactory();
            InitTests();
        }

        private void InitTests()
        {
            Func<object[], Message> findOneFunc = x => _workingObjects.Find(m => m.Id == Convert.ToInt32(x[0]));
            InitBehavior(findOneFunc, MessageFactory.GenericList());

            var uRepo = InitUserRepo();
            _validator = new MessageValidator(_repo.Object, uRepo.Object);
            _service = new MessageService(_repo.Object, _validator);
            _errorObjects = MessageFactory.ErrorList();
            _nullFields = new List<string> { nameof(Message.Content), nameof(Message.Receipt), nameof(Message.Sender) };
            _fieldOrderBy = nameof(Message.CreationDate);
        }

        private Mock<IRepository<User>> InitUserRepo()
        {
            var uRepo = new Mock<IRepository<User>>();
            uRepo.Setup(x => x.FindOne(It.IsAny<object[]>())).Returns<object[]>(x =>
            {
                var val = Convert.ToInt32(x[0]);
                var message = _defaultObjects.FirstOrDefault(m => m.Sender.Id == val || m.Receipt.Id == val);
                if (message == null) return null;
                return message.Sender.Id == val ? message.Sender : message.Receipt;
            });
            return uRepo;
        }

        [TestMethod]
        public void Add_WithMinCreationDateMessage_ThenToday()
        {
            var res = _service.Add(MessageFactory.MinCreationDateMessage).Value.CreationDate;
            Assert.AreEqual(DateTime.Today.ToString("d"), res.ToString("d"));
        }

        [TestMethod]
        public void Find_WithGenericUser1AndGenericUser2_Then1Message()
        {
            var res = (_service as IMessageService).Find(idOrigin: UserFactory.GenericUser1.Id, idDest: UserFactory.GenericUser2.Id);
            Assert.IsTrue(res.Count == 1 
                && res[0].Sender.Id == MessageFactory.User1ToUser2Message.Sender.Id 
                && res[0].Receipt.Id == MessageFactory.User1ToUser2Message.Receipt.Id
            );
        }

        [TestMethod]
        public void Find_WithUnknownDest_ThenAll()
        {
            var res = (_service as IMessageService).Find(idDest: UserFactory.UnknownUser.Id);
            Assert.IsTrue(res.Count == _workingObjects.Count());
        }

        [TestMethod]
        public void Find_WithOriginId5_ThenEmpty()
        {
            var res = (_service as IMessageService).Find(idOrigin: 5);
            Assert.IsFalse(res.Any());
        }
    }
}
