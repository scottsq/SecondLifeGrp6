﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class PhotoServiceTester : GenericServiceTester<Photo>
    {
        private Photo photo = new Photo();
        private Product p1;
        private Product p2;

        public PhotoServiceTester()
        {
            p1 = new Product(); p1.Id = 0;
            p2 = new Product(); p2.Id = 1;

            photo.Id = 2;
            photo.Product = p1;
            photo.Url = "url1";

            var ph1 = new Photo();
            ph1.Id = 0; ph1.Product = p1; ph1.Url = "url2";

            var ph2 = new Photo();
            ph2.Id = 1; ph2.Product = p2; ph2.Url = "url3";

            InitBehavior(ph1, ph2);
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

            _validator = new PhotoValidator(_repo.Object, new ValidationModel<bool>(), pRepo.Object);
            _repo.Setup(x => x.FindOne(It.IsAny<object[]>())).Returns<object[]>(x => {
                return _workingObjects.Find(m => m.Id == Int32.Parse(x[0].ToString()));
            });

            _service = new PhotoService(_repo.Object, _validator);
            nullFields = new List<string> { "Product", "Url" };
        }

        [TestMethod]
        public void GetByProduct_WithP1_ThenNotEmpty()
        {
            Assert.AreNotEqual(0, ((PhotoService)_service).GetByProduct(p1.Id));
        }

        [TestMethod]
        public void GetByProduct_WithP2_ThenEmpty()
        {
            Assert.AreNotEqual(0, ((PhotoService)_service).GetByProduct(p2.Id));
        }

        [TestMethod]
        public void Add_WithBlankUrl_ThenValidationError()
        {
            photo.Url = "    ";
            Assert.AreNotEqual(0, _service.Add(photo));
        }
    }
}
