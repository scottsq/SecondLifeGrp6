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
    public class ProductRatingServiceTester : GenericServiceTester<ProductRating>
    {
        private ProductRating _rating = new ProductRating();
        private ProductRating _rating1 = new ProductRating();
        private ProductRating _rating2 = new ProductRating();
        private Product _p1 = new Product();
        private Product _p2 = new Product();
        private User _u1 = new User();
        private User _u2 = new User();

        public ProductRatingServiceTester()
        {
            CreateInstances();
            InitBehavior(_rating1, _rating2);
            InitTests();
        }

        private void CreateInstances()
        {
            _p1.Id = 0; _p2.Id = 1;
            _u1.Id = 0; _u2.Id = 1;

            _rating.Id = 2;
            _rating.Product = _p1;
            _rating.User = _u1;
            _rating.Stars = 5;

            _rating1.Id = 0;
            _rating1.Product = _p1;
            _rating1.User = _u1;
            _rating1.Stars = 1;

            _rating2.Id = 1;
            _rating2.Product = _p2;
            _rating2.User = _u2;
            _rating2.Stars = 3;
        }

        private void InitTests()
        {
            var pRepo = new Mock<IRepository<Product>>();
            pRepo.Setup(x => x.FindOne(It.IsAny<int>())).Returns<int>(x =>
            {
                if (x == _p1.Id) return _p1;
                if (x == _p2.Id) return _p2;
                return null;
            });
            var uRepo = new Mock<IRepository<User>>();
            uRepo.Setup(x => x.FindOne(It.IsAny<int>())).Returns<int>(x =>
            {
                if (x == _u1.Id) return _u1;
                if (x == _u2.Id) return _u2;
                return null;
            });

            _validator = new ProductRatingValidator(_repo.Object, new ValidationModel<bool>(), pRepo.Object, uRepo.Object);
            _repo.Setup(x => x.FindOne(It.IsAny<int>())).Returns<int>(x => {
                return _workingObjects.Find(m => m.Id == x);
            });

            _service = new ProductRatingService(_repo.Object, _validator);
            nullFields = new List<string> { "Product", "User" };
        }

        [TestMethod]
        public void GetProductRating_WithP1_ThenNot0()
        {
            var res = ((ProductRatingService)_service).GetProductRating(_p1.Id);
            Assert.AreNotEqual(0, res);
        }

        [TestMethod]
        public void GetProductRating_WithP2_Then0()
        {
            var res = ((ProductRatingService)_service).GetProductRating(_p2.Id);
            Assert.AreEqual(0, res);
        }

        [TestMethod]
        public void GetUserRatings_WithU1_ThenNotEmpty()
        {
            var res = ((ProductRatingService)_service).GetUserRatings(_u1.Id);
            Assert.AreNotEqual(0, res.Count);
        }

        [TestMethod]
        public void GetUserRatings_WithU2_ThenEmpty()
        {
            var res = ((ProductRatingService)_service).GetUserRatings(_u2.Id);
            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        public void GetRatings_WithP1_ThenNotEmpty()
        {
            var res = ((ProductRatingService)_service).GetRatings(_p1.Id);
            Assert.AreNotEqual(0, res.Count);
        }

        [TestMethod]
        public void GetRatings_WithP2_ThenEmpty()
        {
            var res = ((ProductRatingService)_service).GetRatings(_p2.Id);
            Assert.AreEqual(0, res.Count);
        }
    }
}
