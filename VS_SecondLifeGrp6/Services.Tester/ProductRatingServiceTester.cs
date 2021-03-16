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
        private ProductRating rating = new ProductRating();
        private ProductRating r1 = new ProductRating();
        private ProductRating r2 = new ProductRating();
        private Product p1 = new Product();
        private Product p2 = new Product();
        private User u1 = new User();
        private User u2 = new User();

        public ProductRatingServiceTester()
        {
            p1.Id = 0; p2.Id = 1;
            u1.Id = 0; u2.Id = 1;

            rating.Id = 2;
            rating.Product = p1;
            rating.User = u1;
            rating.Stars = 5;

            r1.Id = 0;
            r1.Product = p1;
            r1.User = u1;
            r1.Stars = 1;

            r2.Id = 1;
            r2.Product = p2;
            r2.User = u2;
            r2.Stars = 3;

            InitBehavior(r1, r2);
            InitTests();
        }

        private void InitTests()
        {
            var pRepo = new Mock<IRepository<Product>>();
            pRepo.Setup(x => x.FindOne(It.IsAny<int>())).Returns<int>(x =>
            {
                if (x == p1.Id) return p1;
                if (x == p2.Id) return p2;
                return null;
            });
            var uRepo = new Mock<IRepository<User>>();
            uRepo.Setup(x => x.FindOne(It.IsAny<int>())).Returns<int>(x =>
            {
                if (x == u1.Id) return u1;
                if (x == u2.Id) return u2;
                return null;
            });

            _validator = new ProductRatingValidator(_repo.Object, new ValidationModel<bool>(), pRepo.Object, uRepo.Object);
            _repo.Setup(x => x.FindOne(It.IsAny<object[]>())).Returns<object[]>(x => {
                return _workingObjects.Find(m => m.Id == Int32.Parse(x[0].ToString()));
            });

            _service = new ProductRatingService(_repo.Object, _validator);
            nullFields = new List<string> { "Product", "User" };
        }

        [TestMethod]
        public void GetProductRating_WithP1_ThenNot0()
        {
            var res = ((ProductRatingService)_service).GetProductRating(p1.Id);
            Assert.AreNotEqual(0, res);
        }

        [TestMethod]
        public void GetProductRating_WithP2_Then0()
        {
            var res = ((ProductRatingService)_service).GetProductRating(p2.Id);
            Assert.AreEqual(0, res);
        }

        [TestMethod]
        public void GetUserRatings_WithU1_ThenNotEmpty()
        {
            var res = ((ProductRatingService)_service).GetUserRatings(u1.Id);
            Assert.AreNotEqual(0, res.Count);
        }

        [TestMethod]
        public void GetUserRatings_WithU2_ThenEmpty()
        {
            var res = ((ProductRatingService)_service).GetUserRatings(u2.Id);
            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        public void GetRatings_WithP1_ThenNotEmpty()
        {
            var res = ((ProductRatingService)_service).GetRatings(p1.Id);
            Assert.AreNotEqual(0, res.Count);
        }

        [TestMethod]
        public void GetRatings_WithP2_ThenEmpty()
        {
            var res = ((ProductRatingService)_service).GetRatings(p2.Id);
            Assert.AreEqual(0, res.Count);
        }
    }
}
