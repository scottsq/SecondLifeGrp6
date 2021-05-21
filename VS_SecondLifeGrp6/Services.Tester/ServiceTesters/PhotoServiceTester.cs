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
            InitBehavior(findOneFunc, PhotoFactory.GenericList());

            var pRepo = RepoHelper<Photo>.GetMockedSubRepo<Product>(_defaultObjects, nameof(Photo.Product));
            _validator = new PhotoValidator(_repo.Object, pRepo.Object);
            _service = new PhotoService(_repo.Object, _validator);
            _errorObjects = PhotoFactory.ErrorList();
            _nullFields = new List<string> { nameof(Photo.Product), nameof(Photo.Url) };
            _fieldOrderBy = nameof(Photo.Id);
        }

        [TestMethod]
        public void Find_WithProduct1_ThenPhoto1()
        {
            var res = (_service as PhotoService).Find(productId: ProductFactory.GenericProduct1.Id);
            Assert.IsTrue(res.Count == 1 && res[0].Id == PhotoFactory.Product1Photo.Id);
        }

        [TestMethod]
        public void Find_WithProductId9_ThenEmpty()
        {
            var res = (_service as PhotoService).Find(productId: 9);
            Assert.IsFalse(res.Any());
        }
    }
}
