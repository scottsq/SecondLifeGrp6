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
    public class ProductTagServiceTester : GenericServiceTester<ProductTag>
    { 
        public ProductTagServiceTester()
        {
            ProductTagFactory.InitFactory();
            InitTests();
        }

        private void InitTests()
        {
            Func<object[], ProductTag> findOneFunc = x => _workingObjects.Find(p => p.Id == Convert.ToInt32(x[0]));
            InitBehavior(findOneFunc, ProductTagFactory.GenericList());

            var pRepo = RepoHelper<ProductTag>.GetMockedSubRepo<Product>(_defaultObjects, nameof(ProductTag.Product));
            var tRepo = RepoHelper<ProductTag>.GetMockedSubRepo<Tag>(_defaultObjects, nameof(ProductTag.Tag));

            _validator = new ProductTagValidator(_repo.Object, pRepo.Object, tRepo.Object);
            _service = new ProductTagService(_repo.Object, _validator);
            _errorObjects = ProductTagFactory.ErrorList();
            _nullFields = new List<string> { "Product", "Tag" };
            _fieldOrderBy = nameof(ProductTag.Id);
        }

        [TestMethod]
        public void Find_WithGenericTag1_Then2ResultsAndGenericTag1()
        {
            var id = TagFactory.GenericTag1.Id;
            var res = (_service as ProductTagService).Find(tagId: id);
            Assert.IsTrue(res.Count == 2 && res[0].Tag.Id == id && res[1].Tag.Id == id);
        }

        [TestMethod]
        public void Find_WithGenericProduct2_Then1ResultAndGenericProduct2()
        {
            var id = ProductFactory.GenericProduct2.Id;
            var res = (_service as ProductTagService).Find(productId: id);
            Assert.IsTrue(res.Count == 1 && res[0].Product.Id == id);
        }

        [TestMethod]
        public void Find_WithTagId6_ThenEmpty()
        {
            var res = (_service as ProductTagService).Find(tagId: 6);
            Assert.IsFalse(res.Any());
        }
    }
}
