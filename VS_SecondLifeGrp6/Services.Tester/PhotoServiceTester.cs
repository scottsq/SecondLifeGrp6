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
    public class PhotoServiceTester : GenericServiceTester<Photo>
    {

        public PhotoServiceTester()
        {
            PhotoFactory.InitFactory();
            InitTests();
        }

        private void InitTests()
        {
            Func<object[], Photo> findOneFunc = x => _workingObjects.Find(p => p.Id == Convert.ToInt32(x[0]));
            InitBehavior(findOneFunc, PhotoFactory.Product1Photo, PhotoFactory.Product2Photo, PhotoFactory.Product3Photo);

            var pRepo = InitProductRepo();
            _validator = new PhotoValidator(_repo.Object, pRepo.Object);
            _service = new PhotoService(_repo.Object, _validator);
            _errorObjects = new List<Photo> { PhotoFactory.BlankUrlPhoto, PhotoFactory.UnknownProductPhoto };
            _nullFields = new List<string> { "Product", "Url" };
            _fieldOrderBy = nameof(Photo.Id);
        }

        private Mock<IRepository<Product>> InitProductRepo()
        {
            var pRepo = new Mock<IRepository<Product>>();
            pRepo.Setup(x => x.FindOne(It.IsAny<object[]>())).Returns<object[]>(x =>
            {
                var val = Convert.ToInt32(x[0]);
                var photo = _workingObjects.FirstOrDefault(p => p.Product.Id == Convert.ToInt32(x[0]));
                if (photo == null) return null;
                return photo.Product;
            });
            return pRepo;
        }

        [TestMethod]
        public void Find_WithProduct1_ThenPhoto1()
        {
            var res = (_service as PhotoService).Find(productId: ProductFactory.GenericProduct1.Id);
            Assert.IsTrue(res.Count == 1 && res[0].Id == PhotoFactory.Product1Photo.Id);
        }
    }
}
