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
        private Product _product = new Product();
        private Product _product1 = new Product();
        private Product _product2 = new Product();
        private User _u1 = new User();
        private User _u2 = new User();
        private User _u3 = new User();

        public ProductServiceTester()
        {
            CreateInstances();
            InitBehavior(_product1, _product2);
            InitTests();
        }

        private void CreateInstances()
        {
            _u1.Id = 0;
            _u2.Id = 1;
            _u3.Id = 2;

            _product.Id = 2;
            _product.Name = "Product 2";
            _product.Description = "Woah une desc!";
            _product.Owner = _u1;
            _product.Price = 99.99;
            _product.CreationDate = DateTime.Now;

            _product1 = Clone(_product, _product1);
            _product1.Id = 0; 
            _product1.Name = "Product 0";

            _product2 = Clone(_product, _product2);
            _product2.Id = 1;
            _product2.Name = "!~Somewhat Unique Name~!"; 
            _product2.Owner = _u2;
        }

        private void InitTests()
        {
            var uRepo = new Mock<IRepository<User>>();
            uRepo.Setup(x => x.FindOne(It.IsAny<int>())).Returns<int>(x =>
            {
                if (x == _u1.Id) return _u1;
                if (x == _u2.Id) return _u2;
                return null;
            });

            _validator = new ProductValidator(_repo.Object, new ValidationModel<bool>(), uRepo.Object);
            _repo.Setup(x => x.FindOne(It.IsAny<int>())).Returns<int>(x => {
                return _workingObjects.Find(m => m.Id == x);
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
            var pt = new ProductTag(); pt.Tag = tag1; pt.Product = _product1;
            var pt2 = new ProductTag(); pt2.Tag = tag2; pt2.Product = _product1;
            var pt3 = new ProductTag(); pt3.Tag = tag1; pt3.Product = _product;
            var pt4 = new ProductTag(); pt4.Tag = tag2; pt4.Product = _product;
            var pt5 = new ProductTag(); pt5.Tag = tag3; pt5.Product = _product2;
            var pt6 = new ProductTag(); pt6.Tag = tag1; pt6.Product = _product2;
            listProductTags.Add(pt); listProductTags.Add(pt2);
            listProductTags.Add(pt3); listProductTags.Add(pt4);
            listProductTags.Add(pt5); listProductTags.Add(pt6);

            propRepo.Setup(x => x.All(It.IsAny<Expression<Func<Proposal, bool>>>())).Returns<Expression<Func<Proposal, bool>>>(x =>
            {
                var res = new List<Proposal>();
                var proposal = new Proposal();
                proposal.Id = 0; proposal.Product = _product1; proposal.Origin = _u1; proposal.State = State.ACTIVE; proposal.Target = _u3;
                var proposal2 = new Proposal();
                proposal2.Id = 1; proposal2.Product = _product2; proposal2.Origin = _u1; proposal2.State = State.ACCEPTED; proposal2.Target = _u3;
                res.Add(proposal);
                res.Add(proposal2);
                if (x == null) x = t => true;
                return res.Where(x.Compile()).ToList();
            });
            prodTagRepo.Setup(x => x.All(It.IsAny<Expression<Func<ProductTag, bool>>>())).Returns<Expression<Func<ProductTag, bool>>>(x =>
            {
                return listProductTags.Where(x.Compile()).ToList();
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
            _product2.CreationDate = DateTime.Now.AddYears(-5);
            _product.CreationDate = DateTime.Now.AddYears(-2);
            _service.Add(_product2); _service.Add(_product);
            var res = ((ProductService)_service).GetLatest(2);
            Assert.AreEqual(0, res.Where(x => x.Id == _product2.Id).Count());
        }

        [TestMethod]
        public void GetUserProducts_WithU1_ThenNotEmpty()
        {
            var res = ((ProductService)_service).GetUserProducts(_u1.Id);
            Assert.AreNotEqual(0, res.Count);
        }

        [TestMethod]
        public void GetUserProducts_WithU2_ThenEmpty()
        {
            var res = ((ProductService)_service).GetUserProducts(_u2.Id);
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
            _service.Add(_product2);
            var res = ((ProductService)_service).GetProductsByKeys(_product2.Name.Split(" "));
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
            var res = ((ProductService)_service).GetProductsByInterest(_u1.Id);
            Assert.AreNotEqual(0, res.Count);
        }

        [TestMethod]
        public void GetProductsByInterest_WithU2_ThenEmpty()
        {
            var res = ((ProductService)_service).GetProductsByInterest(_u2.Id);
            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        public void Add_WithNegativePrice_ThenValidationError()
        {
            _product.Price = -1;
            var res = _service.Add(_product);
            Assert.AreNotEqual(0, res.Errors.Count);
        }
    }
}
