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
    public class ProductTagServiceTester : GenericServiceTester<ProductTag>
    {
        private ProductTag _productTag = new ProductTag();
        private ProductTag _productTag1 = new ProductTag();
        private ProductTag _productTag2 = new ProductTag();
        private Product _p1 = new Product();
        private Product _p2 = new Product();
        private Tag _t1 = new Tag();
        private Tag _t2 = new Tag();

        public ProductTagServiceTester()
        {
            CreateInstances();
            InitBehavior(_productTag1, _productTag2);
            InitTests();
        }

        private void CreateInstances()
        {
            _p1.Id = 0; _p2.Id = 1;
            _t1.Id = 0; _t2.Id = 1;

            _productTag.Id  = 2; _productTag.Product  = _p1; _productTag.Tag  = _t2;
            _productTag1.Id = 0; _productTag1.Product = _p1; _productTag1.Tag = _t1;
            _productTag2.Id = 1; _productTag2.Product = _p2; _productTag2.Tag = _t2;
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

            var tRepo = new Mock<IRepository<Tag>>();
            tRepo.Setup(x => x.FindOne(It.IsAny<int>())).Returns<int>(x =>
            {
                if (x == _t1.Id) return _t1;
                if (x == _t2.Id) return _t2;
                return null;
            });

            _validator = new ProductTagValidator(_repo.Object, new ValidationModel<bool>(), pRepo.Object, tRepo.Object);

            _repo.Setup(x => x.FindOne(It.IsAny<int>())).Returns<int>(x => {
                return _workingObjects.Find(m => m.Id == x);
            });

            _service = new ProductTagService(_repo.Object, _validator);
            nullFields = new List<string> { "Product", "Tag" };
        }

        [TestMethod]
        public void Add_WithUnknownProduct_ThenError()
        {
            var p = new Product(); p.Id = -1;
            _productTag.Product = p;
            var res = _service.Add(_productTag);
            Assert.AreNotEqual(0, res.Errors.Count);
        }

        [TestMethod]
        public void Add_WithUnknownTag_ThenError()
        {
            var t = new Tag(); t.Id = -1;
            _productTag.Tag = t;
            var res = _service.Add(_productTag);
            Assert.AreNotEqual(0, res.Errors.Count);
        }

        [TestMethod]
        public void GetByProductId_WithP1_ThenNotEmpty()
        {
            var res = ((ProductTagService)_service).GetByProductId(_p1.Id);
            Assert.AreNotEqual(0, res.Count);
        }

        [TestMethod]
        public void GetByProductId_WithP2_ThenEmpty()
        {
            var res = ((ProductTagService)_service).GetByProductId(_p2.Id);
            Assert.AreEqual(0, res.Count);
        }
    }
}
