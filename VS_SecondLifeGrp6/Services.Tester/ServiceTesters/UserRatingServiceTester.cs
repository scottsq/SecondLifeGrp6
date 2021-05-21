using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services.Tester.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Services;
using VS_SLG6.Services.Validators;

namespace Services.Tester.ServiceTesters
{
    [TestClass]
    public class UserRatingServiceTester : GenericServiceTester<UserRating>
    {
        public UserRatingServiceTester()
        {
            UserRatingFactory.InitFactory();
            InitTests();
        }

        private void InitTests()
        {
            Func<object[], UserRating> findOneFunc = x => _workingObjects.Find(ur => ur.Id == Convert.ToInt32(x[0]));
            InitBehavior(findOneFunc, UserRatingFactory.GenericList());

            var uRepo = InitUserRepo();

            _validator = new UserRatingValidator(_repo.Object, uRepo.Object);
            _service = new UserRatingService(_repo.Object, _validator);
            _errorObjects = UserRatingFactory.ErrorList();
            _nullFields = new List<string> { "Origin", "Target" };
            _fieldOrderBy = nameof(UserRating.Stars);
        }

        private Mock<IRepository<User>> InitUserRepo()
        {
            var uRepo = new Mock<IRepository<User>>();
            uRepo.Setup(x => x.FindOne(It.IsAny<object[]>())).Returns<object[]>(x =>
            {
                var val = Convert.ToInt32(x[0]);
                var message = _defaultObjects.FirstOrDefault(m => m.Origin.Id == val || m.Target.Id == val);
                if (message == null) return null;
                return message.Origin.Id == val ? message.Origin : message.Target;
            });
            return uRepo;
        }

        [TestMethod]
        public void Find_WithOriginGenericUser1_Then2ResultsAndOrigin1()
        {
            var id = UserFactory.GenericUser1.Id;
            var res = (_service as UserRatingService).Find(idOrigin: id);
            Assert.IsTrue(res.Count == 2 && res[0].Origin.Id == id && res[1].Origin.Id == id);
        }

        [TestMethod]
        public void Find_WithTargetGenericUser2_Then1ResultsAndTarget2()
        {
            var id = UserFactory.GenericUser2.Id;
            var res = (_service as UserRatingService).Find(idTarget: id);
            Assert.IsTrue(res.Count == 1 && res[0].Target.Id == id);
        }

        [TestMethod]
        public void Find_WithStars7_ThenEmpty()
        {
            var res = (_service as UserRatingService).Find(stars: 7);
            Assert.IsFalse(res.Any());
        }
    }
}
