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
    public class PhotoServiceTester : GenericServiceTester<Photo>
    {
        private Photo _photo = new Photo();
        private Photo _photo1 = new Photo();
        private Photo _photo2 = new Photo();
        private Product _p1 = new Product();
        private Product _p2 = new Product();

        public PhotoServiceTester()
        {
            CreateInstances();
            InitBehavior(_photo1, _photo2);
            InitTests();
        }

        private void CreateInstances()
        {
            _p1.Id = 0; _p1.Owner = new User(); _p1.Owner.Id = 0;
            _p2.Id = 1; _p2.Owner = new User(); _p2.Owner.Id = 1;

            _photo.Id  = 2; _photo.Product  = _p1; _photo.Url  = "url1";
            _photo1.Id = 0; _photo1.Product = _p1; _photo1.Url = "url2";
            _photo2.Id = 1; _photo2.Product = _p2; _photo2.Url = "url3";
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

            _validator = new PhotoValidator(_repo.Object, new ValidationModel<bool>(), pRepo.Object);
            _repo.Setup(x => x.FindOne(It.IsAny<int>())).Returns<int>(x => {
                return _workingObjects.Find(m => m.Id == x);
            });

            _service = new PhotoService(_repo.Object, _validator);
            nullFields = new List<string> { "Product", "Url" };
        }

        [TestMethod]
        public void Add_WithUnknownProduct_ThenError()
        {
            var p = new Product(); p.Id = -1;
            _photo.Product = p;
            var res = _service.Add(_photo);
            Assert.AreNotEqual(0, res.Errors.Count);
        }

        [TestMethod]
        public void GetByProduct_WithP1_ThenNotEmpty()
        {
            Assert.AreNotEqual(0, ((PhotoService)_service).Find(_p1.Id));
        }

        [TestMethod]
        public void GetByProduct_WithP2_ThenEmpty()
        {
            Assert.AreNotEqual(0, ((PhotoService)_service).Find(_p2.Id));
        }

        [TestMethod]
        public void Add_WithBlankUrl_ThenValidationError()
        {
            _photo.Url = "    ";
            Assert.AreNotEqual(0, _service.Add(_photo).Errors.Count);
        }
    }
}
