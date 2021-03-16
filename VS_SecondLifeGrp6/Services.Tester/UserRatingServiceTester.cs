using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;
using VS_SLG6.Services.Services;
using VS_SLG6.Services.Validators;

namespace Services.Tester
{
    [TestClass]
    public class UserRatingServiceTester : GenericServiceTester<UserRating>
    {
        private UserRating rating = new UserRating();
        private UserRating r1 = new UserRating();
        private UserRating r2 = new UserRating();
        private User u1 = new User();
        private User u2 = new User();
        private User u3 = new User();

        public UserRatingServiceTester()
        {
            u1.Id = 0; u2.Id = 1; u3.Id = 2;

            rating.Id = 2;
            rating.Origin = u1;
            rating.Target = u3;
            rating.Stars = 5;

            r1.Id = 0;
            r1.Origin = u1;
            r1.Target = u2;
            r1.Stars = 1;

            r2.Id = 1;
            r2.Origin = u2;
            r2.Target = u1;
            r2.Stars = 3;

            InitBehavior(r1, r2);
            InitTests();
        }

        private void InitTests()
        {
            var uRepo = new Mock<IRepository<User>>();
            uRepo.Setup(x => x.FindOne(It.IsAny<int>())).Returns<int>(x =>
            {
                if (x == u1.Id) return u1;
                if (x == u2.Id) return u2;
                if (x == u3.Id) return u3;
                return null;
            });

            _validator = new UserRatingValidator(_repo.Object, new ValidationModel<bool>(), uRepo.Object);
            _repo.Setup(x => x.FindOne(It.IsAny<object[]>())).Returns<object[]>(x => {
                return _workingObjects.Find(m => m.Id == Int32.Parse(x[0].ToString()));
            });

            _service = new UserRatingService(_repo.Object, _validator);
            nullFields = new List<string> { "Origin", "Target" };
        }

        [TestMethod]
        public void GetAverageRating_WithU1_Then0()
        {
            var res = ((UserRatingService)_service).GetAverageRating(u1.Id);
            Assert.AreEqual(0, res);
        }

        [TestMethod]
        public void GetAverageRating_WithU2_ThenNot0()
        {
            var res = ((UserRatingService)_service).GetAverageRating(u2.Id);
            Assert.AreNotEqual(0, res);
        }

        [TestMethod]
        public void GetUserRatings_WithU1_ThenNotEmpty()
        {
            var res = ((UserRatingService)_service).GetUserRatings(u1.Id);
            Assert.AreNotEqual(0, res.Count);
        }

        [TestMethod]
        public void GetUserRatings_WithU2_ThenEmpty()
        {
            var res = ((UserRatingService)_service).GetUserRatings(u2.Id);
            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        public void GetRatings_WithU1_ThenEmpty()
        {
            var res = ((UserRatingService)_service).GetRatings(u1.Id);
            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        public void GetRatings_WithU2_ThenNotEmpty()
        {
            var res = ((UserRatingService)_service).GetRatings(u2.Id);
            Assert.AreNotEqual(0, res.Count);
        }
    }
}
