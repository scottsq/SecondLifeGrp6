using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Services;
using VS_SLG6.Services.Validators;

namespace Services.Tester
{
    [TestClass]
    public class GenericCrudTester<T> where T : class
    {
        protected readonly Mock<IRepository<T>> _repo;
        private Mock<IValidator<T>> _validator;
        private IService<T> _service;
        protected List<T> _defaultObjects;
        
        public GenericCrudTester()
        {
            _repo = new Mock<IRepository<T>>();
            _validator = new Mock<IValidator<T>>();
            _service = new GenericService<T>(_repo.Object, _validator.Object);
        }

        public void SetDefaultObjects(params T[] objs)
        {
            _defaultObjects = objs.ToList();
            for (int i = 0; i < objs.Length; i++) _repo.Setup(x => x.Add(objs[i]));
        }

        [TestMethod]
        public void List_ThenList()
        {
            Assert.IsTrue(_defaultObjects.SequenceEqual(_service.List()));
            //CollectionAssert.AreEquivalent(_service.List(), _defaultObjects);
        }

        [TestMethod]
        public void Add_WithExistingObject_ThenValidationError()
        {
            var res = _service.Add(_defaultObjects[0]);
            Assert.AreNotEqual(0, res.Errors.Count);
        }

        [TestMethod]
        public void Add_WithNull_ThenValidationError()
        {
            var res = _service.Add(null);
            Assert.AreNotEqual(0, res.Errors.Count);
        }
    }
}
