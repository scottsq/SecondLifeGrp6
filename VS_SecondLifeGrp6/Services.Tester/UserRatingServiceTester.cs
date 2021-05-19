//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using VS_SLG6.Model.Entities;
//using VS_SLG6.Repositories.Repositories;
//using VS_SLG6.Services.Models;
//using VS_SLG6.Services.Services;
//using VS_SLG6.Services.Validators;

//namespace Services.Tester
//{
//    [TestClass]
//    public class UserRatingServiceTester : GenericServiceTester<UserRating>
//    {
//        private UserRating _rating = new UserRating();
//        private UserRating _rating1 = new UserRating();
//        private UserRating _rating2 = new UserRating();
//        private User _u1 = new User();
//        private User _u2 = new User();
//        private User _u3 = new User();

//        public UserRatingServiceTester()
//        {
//            CreateInstances();
//            InitBehavior(_rating1, _rating2);
//            InitTests();
//        }

//        private void CreateInstances()
//        {
//            _u1.Id = 0; _u2.Id = 1; _u3.Id = 2;

//            _rating.Id = 2;
//            _rating.Origin = _u1;
//            _rating.Target = _u3;
//            _rating.Stars = 5;
//            _rating.Comment = "Wow trop bien!";
                
//            _rating1.Id = 0;
//            _rating1.Origin = _u1;
//            _rating1.Target = _u2;
//            _rating1.Stars = 1;
//            _rating1.Comment = null;

//            _rating2.Id = 1;
//            _rating2.Origin = _u2;
//            _rating2.Target = _u1;
//            _rating2.Stars = 3;
//            _rating2.Comment = BLANK_STRING;
//        }

//        private void InitTests()
//        {
//            var uRepo = new Mock<IRepository<User>>();
//            uRepo.Setup(x => x.FindOne(It.IsAny<int>())).Returns<int>(x =>
//            {
//                if (x == _u1.Id) return _u1;
//                if (x == _u2.Id) return _u2;
//                if (x == _u3.Id) return _u3;
//                return null;
//            });

//            _validator = new UserRatingValidator(_repo.Object, new ValidationModel<bool>(), uRepo.Object);
//            _repo.Setup(x => x.FindOne(It.IsAny<int>())).Returns<int>(x => {
//                return _workingObjects.Find(m => m.Id == x);
//            });

//            _service = new UserRatingService(_repo.Object, _validator);
//            nullFields = new List<string> { "Origin", "Target" };
//        }

//        [TestMethod]
//        public void Add_WithRating2_ThenCommentIsNull()
//        {
//            _service.Add(_rating2);
//            var res = _service.Get(_rating2.Id);
//            Assert.AreEqual(null, res.Value.Comment);
//        }

//        [TestMethod]
//        public void Add_WithUnknownOrigin_ThenError()
//        {
//            var o = new User(); o.Id = -1;
//            _rating.Origin = o;
//            var res = _service.Add(_rating);
//            Assert.AreNotEqual(0, res.Errors.Count);
//        }

//        [TestMethod]
//        public void Add_WithUnknownTarget_ThenError()
//        {
//            var t = new User(); t.Id = -1;
//            _rating.Target = t;
//            var res = _service.Add(_rating);
//            Assert.AreNotEqual(0, res.Errors.Count);
//        }

//        [TestMethod]
//        public void GetAverageRating_WithU1_Then0()
//        {
//            var res = ((UserRatingService)_service).GetAverageRating(_u1.Id);
//            Assert.AreEqual(0, res);
//        }

//        [TestMethod]
//        public void GetAverageRating_WithU2_ThenNot0()
//        {
//            var res = ((UserRatingService)_service).GetAverageRating(_u2.Id);
//            Assert.AreNotEqual(0, res);
//        }

//        [TestMethod]
//        public void GetUserRatings_WithU1AndU2_ThenNotNull()
//        {
//            var res = ((UserRatingService)_service).GetRating(_u1.Id, _u2.Id);
//            Assert.AreNotEqual(null, res);
//        }

//        [TestMethod]
//        public void GetUserRatings_WithU2AndU3_ThenEmpty()
//        {
//            var res = ((UserRatingService)_service).GetRating(_u2.Id, _u3.Id);
//            Assert.AreEqual(null, res);
//        }

//        [TestMethod]
//        public void GetRatings_WithU1_ThenEmpty()
//        {
//            var res = ((UserRatingService)_service).ListRatings(_u1.Id);
//            Assert.AreEqual(0, res.Count);
//        }

//        [TestMethod]
//        public void GetRatings_WithU2_ThenNotEmpty()
//        {
//            var res = ((UserRatingService)_service).ListRatings(_u2.Id);
//            Assert.AreNotEqual(0, res.Count);
//        }
//    }
//}
