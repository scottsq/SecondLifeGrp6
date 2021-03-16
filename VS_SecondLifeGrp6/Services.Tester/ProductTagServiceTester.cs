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
        private ProductTag productTag = new ProductTag();
        private ProductTag pt1 = new ProductTag();
        private ProductTag pt2 = new ProductTag();
        private Product p1 = new Product();
        private Product p2 = new Product();
        private Tag t1 = new Tag();
        private Tag t2 = new Tag();

        public ProductTagServiceTester()
        {
            p1.Id = 0; p2.Id = 1;
            t1.Id = 0; t2.Id = 1;

            productTag.Id = 2;
            productTag.Product = p1;
            productTag.Tag = t2;

            pt1.Id = 0; pt1.Product = p1; pt1.Tag = t1;
            pt2.Id = 1; pt2.Product = p2; pt2.Tag = t2;

            InitBehavior(pt1, pt2);
            InitTests();
        }

        private void InitTests()
        {
            var pService = new Mock<ProductService>(new Mock<IRepository<Product>>().Object, new Mock<IValidator<Product>>().Object, null, null);
            pService.Setup(x => x.Get(It.IsAny<int>())).Returns<int>(x =>
            {
                if (x == p1.Id) return p1;
                if (x == p2.Id) return p2;
                return null;
            });
            var tService = new Mock<GenericService<Tag>>(new Mock<IRepository<Tag>>().Object, new Mock<IValidator<Tag>>().Object);
            tService.Setup(x => x.Get(It.IsAny<int>())).Returns<int>(x =>
            {
                if (x == t1.Id) return t1;
                if (x == t2.Id) return t2;
                return null;
            });

            _validator = new ProductTagValidator(_repo.Object, new ValidationModel<bool>(), pService.Object, tService.Object);
            _repo.Setup(x => x.FindOne(It.IsAny<object[]>())).Returns<object[]>(x => {
                return _workingObjects.Find(m => m.Id == Int32.Parse(x[0].ToString()));
            });

            _service = new ProductTagService(_repo.Object, _validator);
            nullFields = new List<string> { "Product", "Tag" };
        }

        [TestMethod]
        public void GetByProductId_WithP1_ThenNotEmpty()
        {
            var res = ((ProductTagService)_service).GetByProductId(p1.Id);
            Assert.AreNotEqual(0, res.Count);
        }

        [TestMethod]
        public void GetByProductId_WithP2_ThenEmpty()
        {
            var res = ((ProductTagService)_service).GetByProductId(p2.Id);
            Assert.AreEqual(0, res.Count);
        }
    }
}
