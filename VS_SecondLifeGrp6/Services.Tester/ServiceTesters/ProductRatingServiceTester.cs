using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Tester.Factories;
using Services.Tester.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Services;
using VS_SLG6.Services.Validators;

namespace Services.Tester.ServiceTesters
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
            InitBehavior(findOneFunc, ProductRatingFactory.GenericList());

            var pRepo = RepoHelper<ProductRating>.GetMockedSubRepo<Product>(_defaultObjects, nameof(ProductRating.Product));
            var uRepo = RepoHelper<ProductRating>.GetMockedSubRepo<User>(_defaultObjects, nameof(ProductRating.User));

            _validator = new ProductRatingValidator(_repo.Object, pRepo.Object, uRepo.Object);
            _service = new ProductRatingService(_repo.Object, _validator);
            _errorObjects = ProductRatingFactory.ErrorList();
            _nullFields = new List<string> { nameof(ProductRating.Product), nameof(ProductRating.User) };
            _fieldOrderBy = nameof(ProductRating.Stars);
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

        [TestMethod]
        public void Find_With5Stars_Then1ResultAndFiveStarsProduct1User1Rating()
        {
            var id = ProductRatingFactory.FiveStarsProduct1User1Rating.Id;
            var res = (_service as ProductRatingService).Find(stars: 5);
            Assert.IsTrue(res.Count == 1 && res[0].Id== id);
        }

        [TestMethod]
        public void Find_WithIdProduct8_ThenEmpty()
        {
            var res = (_service as ProductRatingService).Find(idProduct: 8);
            Assert.IsFalse(res.Any());
        }
    }
}
