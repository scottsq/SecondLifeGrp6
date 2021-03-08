using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;
using VS_SLG6.Services.Services;
using VS_SLG6.Services.Validators;

namespace Services.Tester
{
    [TestClass]
    public class GenericServiceTester<T> where T : class
    {
        protected Mock<IRepository<T>> _repo;
        protected IValidator<T> _validator;
        protected IService<T> _service;
        protected List<T> _defaultObjects;
        protected List<T> _workingObjects;
        
        public GenericServiceTester()
        {
            _repo = new Mock<IRepository<T>>();
            _service = new GenericService<T>(_repo.Object, _validator);
        }

        public void InitBehavior(params T[] objs)
        {
            _defaultObjects = objs.ToList();
            _workingObjects = _defaultObjects.GetRange(0, 1);
            _repo.Setup(x => x.All()).Returns(_workingObjects);
            _repo.Setup(x => x.Add(It.IsAny<T>())).Returns<T>(x => {
                _workingObjects.Add(x);
                return x;
            });
            _repo.Setup(x => x.Remove(It.IsAny<T>())).Callback<T>(x => { _workingObjects.Remove(x); });
            _repo.Setup(x => x.FindAll(It.IsAny<System.Linq.Expressions.Expression<Func<T, bool>>>())).Returns<System.Linq.Expressions.Expression<Func<T, bool>>>(x =>
            {
                return _workingObjects.Where(x.Compile()).ToList();
            });
        }

        [TestMethod]
        public void List_ThenList()
        {
            Assert.IsTrue(_workingObjects.SequenceEqual(_service.List()));
        }

        [TestMethod]
        public void Get_With0_ThenNotNull()
        {
            Assert.AreNotEqual(null, _service.Get(0));
        }

        [TestMethod]
        public void Get_WithMinus1_ThenNull()
        {
            Assert.AreEqual(null, _service.Get(-1));
        }


        [TestMethod]
        public void Add_WithObject0_ThenValidationError()
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

        [TestMethod]
        public void Add_WithObject1_ThenNoError()
        {
            var res = _service.Add(_defaultObjects[1]);
            Assert.AreEqual(0, res.Errors.Count);
        }

        [TestMethod]
        public void Remove_WithObject0_ThenListIsEmpty()
        {
            _service.Remove(_service.Get(0));
            Assert.AreEqual(0, _service.List().Count);
        }

        [TestMethod]
        public void Remove_WithNullObject_ThenListIsNotEmpty()
        {
            _service.Remove(null);
            Assert.AreNotEqual(0, _service.List().Count);
        }
    }
}
