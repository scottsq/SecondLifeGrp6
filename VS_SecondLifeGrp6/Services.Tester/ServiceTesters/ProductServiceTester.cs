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
    public class ProductServiceTester : GenericServiceTester<Product>
    {
        public ProductServiceTester()
        {
            ProductFactory.InitFactory();
            InitTests();
        }

        private void InitTests()
        {
            Func<object[], Product> findOneFunc = x => _workingObjects.Find(p => p.Id == Convert.ToInt32(x[0]));
            InitBehavior(findOneFunc, ProductFactory.GenericList());

            var uRepo = RepoHelper<Product>.GetMockedSubRepo<User>(_defaultObjects, nameof(Product.Owner));
            var photoRepo = RepoHelper<Product>.GetMockedSubRepo<Photo>(_defaultObjects, listAll: PhotoFactory.GenericList());
            var propRepo = RepoHelper<Product>.GetMockedSubRepo<Proposal>(_defaultObjects, listAll: ProposalFactory.GenericList());
            var prodTagRepo = RepoHelper<Product>.GetMockedSubRepo<ProductTag>(_defaultObjects, listAll: ProductTagFactory.GenericList());

            _validator = new ProductValidator(_repo.Object, uRepo.Object);
            _service = new ProductService(_repo.Object, _validator, propRepo.Object, prodTagRepo.Object, photoRepo.Object);
            _errorObjects = ProductFactory.ErrorList();
            _nullFields = new List<string> { "Name", "Description", "Owner" };
            _fieldOrderBy = nameof(Product.CreationDate);
        }

        [TestMethod]
        public void Find_WithKey2_Then1ResultAndGenericProduct2()
        {
            var res = (_service as ProductService).Find(keys: "2");
            Assert.IsTrue(res.Count == 1 && ProductFactory.GenericProduct2.Id == res[0].Id);
        }

        [TestMethod]
        public void Find_WithIdMinus5_ThenAll()
        {
            var res = (_service as ProductService).Find(id: -5);
            Assert.IsTrue(res.Count == _workingObjects.Count);
        }

        [TestMethod]
        public void Find_WithUserIdMinus5_ThenAll()
        {
            var res = (_service as ProductService).Find(userId: -5);
            Assert.IsTrue(res.Count == _workingObjects.Count);
        }

        [TestMethod]
        public void Find_WithUserId7_ThenEmpty()
        {
            var res = (_service as ProductService).Find(userId: 7);
            Assert.IsFalse(res.Any());
        }
    }
}
