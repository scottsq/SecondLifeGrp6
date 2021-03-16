using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;
using VS_SLG6.Services.Services;
using VS_SLG6.Services.Validators;

namespace Services.Tester
{
    [TestClass]
    public class ProductServiceTester : GenericServiceTester<Product>
    {
        private Product product = new Product();
        private Product p1 = new Product();
        private Product p2 = new Product();
        private User u1 = new User();
        private User u2 = new User();

        public ProductServiceTester()
        {
            u1.Id = 0;
            u2.Id = 1;

            product.Id = 2;
            product.Name = "Product 2";
            product.Description = "Woah une desc!";
            product.Owner = u1;
            product.Price = 99.99;
            product.CreationDate = DateTime.Now;

            p1 = Clone(product, p1);
            p1.Id = 0; p1.Name = "Product 0";
            p2 = Clone(product, p2);
            p2.Id = 1; p2.Name = "!~Somewhat Unique Name~!"; p2.Owner = u2;

            InitBehavior(p1, p2);
            InitTests();
        }

        private void InitTests()
        {
            var uRepo = new Mock<IRepository<User>>();
            uRepo.Setup(x => x.FindOne(It.IsAny<int>())).Returns<int>(x =>
            {
                if (x == u1.Id) return u1;
                if (x == u2.Id) return u2;
                return null;
            });

            _validator = new ProductValidator(_repo.Object, new ValidationModel<bool>(), uRepo.Object);
            _repo.Setup(x => x.FindOne(It.IsAny<object[]>())).Returns<object[]>(x => {
                return _workingObjects.Find(m => m.Id == Int32.Parse(x[0].ToString()));
            });

            var propRepo = new Mock<IRepository<Proposal>>();
            var prodTagRepo = new Mock<IRepository<ProductTag>>();

            var listProductTags = new List<ProductTag>();
            var tag1 = new Tag();
            tag1.Id = 0; tag1.Name = "Tag 0";
            var tag2 = new Tag();
            tag2.Id = 1; tag2.Name = "Tag 1";
            var tag3 = new Tag();
            tag3.Id = 2; tag3.Name = "Tag 2";
            var pt = new ProductTag(); pt.Tag = tag1; pt.Product = p1;
            var pt2 = new ProductTag(); pt2.Tag = tag2; pt2.Product = p1;
            var pt3 = new ProductTag(); pt3.Tag = tag1; pt3.Product = product;
            var pt4 = new ProductTag(); pt4.Tag = tag2; pt4.Product = product;
            var pt5 = new ProductTag(); pt5.Tag = tag3; pt5.Product = p2;
            listProductTags.Add(pt); listProductTags.Add(pt2);
            listProductTags.Add(pt3); listProductTags.Add(pt4);
            listProductTags.Add(pt5);

            propRepo.Setup(x => x.FindAll(It.IsAny<Expression<Func<Proposal, bool>>>())).Returns<Expression<Func<Proposal, bool>>>(x =>
            {
                var res = new List<Proposal>();
                var proposal = new Proposal();
                proposal.Id = 0; proposal.Product = p1;
                var proposal2 = new Proposal();
                proposal2.Id = 1; proposal2.Product = p2;
                res.Add(proposal);
                res.Add(proposal2);
                return res.Where(x.Compile()).ToList();
            });
            prodTagRepo.Setup(x => x.FindAll(It.IsAny<Expression<Func<ProductTag, bool>>>())).Returns<Expression<Func<ProductTag, bool>>>(x =>
            {
                return listProductTags.Where(x.Compile()).ToList();
            });
            prodTagRepo.Setup(x => x.All()).Returns(() =>
            {
                return listProductTags;
            });

            _service = new ProductService(_repo.Object, _validator, propRepo.Object, prodTagRepo.Object);
            nullFields = new List<string> { "Name", "Description", "Owner", "Price" };
        }

        [TestMethod]
        public void GetLatest_ThenNotEmptyAndNotMoreThan10()
        {
            var res = ((ProductService)_service).GetLatest();
            Assert.IsTrue(res.Count > 0 && res.Count < 11);
        }

        [TestMethod]
        public void GetLatest_With2_ThenNotP2()
        {
            p2.CreationDate = DateTime.Now.AddYears(-5);
            product.CreationDate = DateTime.Now.AddYears(-2);
            _service.Add(p2); _service.Add(product);
            var res = ((ProductService)_service).GetLatest(2);
            Assert.AreEqual(0, res.Where(x => x.Id == p2.Id).Count());
        }

        [TestMethod]
        public void GetUserProducts_WithU1_ThenNotEmpty()
        {
            var res = ((ProductService)_service).GetUserProducts(u1.Id);
            Assert.AreNotEqual(0, res.Count);
        }

        [TestMethod]
        public void GetUserProducts_WithU2_ThenEmpty()
        {
            var res = ((ProductService)_service).GetUserProducts(u2.Id);
            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        public void GetProductsByKeys_WithProduct_ThenNotEmpty()
        {
            var res = ((ProductService)_service).GetProductsByKeys("Product");
            Assert.AreNotEqual(0, res.Count);
        }

        [TestMethod]
        public void GetProductsByKeys_WithP2Name_Then1Result()
        {
            _service.Add(p2);
            var res = ((ProductService)_service).GetProductsByKeys(p2.Name.Split(" "));
            Assert.AreEqual(1, res.Count);
        }

        [TestMethod]
        public void GetProductsByKeys_WithNull_ThenNotEmpty()
        {
            var res = ((ProductService)_service).GetProductsByKeys(null);
            Assert.AreNotEqual(0, res.Count);
        }

        [TestMethod]
        public void GetProductsByInterest_WithU1_ThenNotEmpty()
        {
            var res = ((ProductService)_service).GetProductsByInterest(u1.Id);
            Assert.AreNotEqual(0, res.Count);
        }

        [TestMethod]
        public void GetProductsByInterest_WithU2_ThenEmpty()
        {
            var res = ((ProductService)_service).GetProductsByInterest(u2.Id);
            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        public void Add_WithNegativePrice_ThenValidationError()
        {
            product.Price = -1;
            var res = _service.Add(product);
            Assert.AreNotEqual(0, res.Errors.Count);
        }
    }
}
