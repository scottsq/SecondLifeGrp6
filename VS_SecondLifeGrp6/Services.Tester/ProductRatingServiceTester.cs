using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services.Tester.Factories;
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
    public class ProductRatingServiceTester : GenericServiceTester<ProductRating>
    {
        public ProductRatingServiceTester()
        {
            ProductRatingFactory.InitFactory();
            InitTests();
        }

        private void InitTests()
        {
            Func<object[], ProductRating> findOneFunc = x => _workingObjects.Find(p => p.Id == Convert.ToInt32(x[0]));
            InitBehavior(findOneFunc, ProductRatingFactory.FiveStarsProduct1User1Rating, ProductRatingFactory.ThreeStarsProduct1User2Rating, ProductRatingFactory.TwoStarsProduct2User1Rating);

            var pRepo = InitProductRepo();
            var uRepo = InitUserRepo();

            _validator = new ProductRatingValidator(_repo.Object, pRepo.Object, uRepo.Object);
            _service = new ProductRatingService(_repo.Object, _validator);
            _errorObjects = new List<ProductRating> { ProductRatingFactory.NegativeStarsProductRating, ProductRatingFactory.SixStarsProductRating, ProductRatingFactory.UnknownProductProductRating, ProductRatingFactory.UnknownUserProductRating };
            _nullFields = new List<string> { nameof(ProductRating.Product), nameof(ProductRating.User) };
            _fieldOrderBy = nameof(ProductRating.Stars);
        }

        private Mock<IRepository<Product>> InitProductRepo()
        {
            var pRepo = new Mock<IRepository<Product>>();
            pRepo.Setup(x => x.FindOne(It.IsAny<object[]>())).Returns<object[]>(x =>
            {
                var val = Convert.ToInt32(x[0]);
                var productRating = _defaultObjects.FirstOrDefault(p => p.Product.Id == val);
                if (productRating == null) return null;
                return productRating.Product;
            });
            return pRepo;
        }

        private Mock<IRepository<User>> InitUserRepo()
        {
            var uRepo = new Mock<IRepository<User>>();
            uRepo.Setup(x => x.FindOne(It.IsAny<object[]>())).Returns<object[]>(x =>
            {
                var val = Convert.ToInt32(x[0]);
                var productRating = _defaultObjects.FirstOrDefault(p => p.User.Id == val);
                if (productRating == null) return null;
                return productRating.User;
            });
            return uRepo;
        }

        [TestMethod]
        public void GetAverageRating_WithProduct3_Then0()
        {
            var res = (_service as ProductRatingService).GetAverageRating(ProductFactory.GenericProduct3.Id);
            Assert.AreEqual(0, res);
        }

        [TestMethod]
        public void GetAverageRating_WithProduct1_Then4()
        {
            var res = (_service as ProductRatingService).GetAverageRating(ProductFactory.GenericProduct1.Id);
            Assert.AreEqual(4, res);
        }

        [TestMethod]
        public void Find_WithProduct1_Then2Results()
        {
            var id = ProductFactory.GenericProduct1.Id;
            var res = (_service as ProductRatingService).Find(idProduct: id);
            Assert.IsTrue(res.Count == 2 && res[0].Product.Id == id && res[1].Product.Id == id);
        }

        [TestMethod]
        public void Find_WithUnknownProduct_ThenEmpty()
        {
            var res = (_service as ProductRatingService).Find(idProduct: 50);
            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        public void Find_WithUser1_Then2Results()
        {
            var id = UserFactory.GenericUser1.Id;
            var res = (_service as ProductRatingService).Find(idUser: id);
            Assert.IsTrue(res.Count == 2 && res[0].User.Id == id && res[1].User.Id == id);
        }
    }
}
