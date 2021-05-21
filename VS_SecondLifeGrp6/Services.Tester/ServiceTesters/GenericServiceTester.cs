using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Services.Tester.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Interfaces;
using VS_SLG6.Services.Services;
using VS_SLG6.Services.Validators;

namespace Services.Tester.ServiceTesters
{
    [TestClass]
    public class GenericServiceTester<T> where T : class
    {
        protected Mock<IRepository<T>> _repo;
        protected IValidator<T> _validator;
        protected IService<T> _service;

        /// <summary>
        /// Base objects, do not modify
        /// </summary>
        protected List<T> _defaultObjects;
        /// <summary>
        /// Editable shallow copy of _defaultObjects
        /// </summary>
        protected List<T> _workingObjects;
        protected List<T> _errorObjects;
        protected List<string> _nullFields = new List<string>();
        protected string _fieldOrderBy;

        public GenericServiceTester()
        {
            _repo = new Mock<IRepository<T>>();
            _service = new GenericService<T>(_repo.Object, _validator);
        }

        public void InitBehavior(Func<object[], T> findOneFunc, List<T> objs)
        {
            _defaultObjects = objs;
            // Dereferencing objects, so we don't overide default ones
            // There's probably better way to do it but should I really implement IClonable just for test?
            _workingObjects = JsonConvert.DeserializeObject<List<T>>(JsonConvert.SerializeObject(_defaultObjects));

            RepoHelper<T>.GenerateSetupAll(_repo)
                .Returns<Expression<Func<T, bool>>, Func<T, object>, bool, int, int>((condition, orderBy, reverse, from, max) => {
                    condition ??= x => true;
                    var list = _workingObjects.Where(condition.Compile()).ToList();
                    if (orderBy != null && reverse) list = list.OrderByDescending(orderBy).ToList();
                    else if (orderBy != null) list = list.OrderBy(orderBy).ToList();

                    from = Math.Max(from, 0);
                    if (from >= list.Count) return new List<T>();

                    max = Math.Min(Math.Max(max, 1), list.Count);
                    return list.Skip(from).Take(max).ToList();
                });
            _repo.Setup(x => x.Add(It.IsAny<T>())).Returns<T>(x => {
                _workingObjects.Add(x);
                return x;
            });
            _repo.Setup(x => x.Remove(It.IsAny<T>())).Callback<T>(x => { _workingObjects.Remove(x); });
            _repo.Setup(x => x.FindOne(It.IsAny<object[]>())).Returns<object[]>(findOneFunc);
        }

        [TestMethod]
        public void Get_With0_ThenNotNull()
        {
            Assert.AreNotEqual(null, _service.Get(0));
        }

        [TestMethod]
        public void Get_WithMinus1_ThenNull()
        {
            Assert.AreEqual(null, _service.Get(-1).Value);
        }

        [TestMethod]
        public virtual void Add_WithObject0_ThenHasError()
        {
            var res = _service.Add(_defaultObjects[0]);
            Assert.IsTrue(res.HasErrors);
        }

        [TestMethod]
        public void Add_WithNull_ThenHasError()
        {
            var res = _service.Add(null);
            Assert.IsTrue(res.HasErrors);
        }

        [TestMethod]
        public void Add_WithErrorObjects_ThenHasError()
        {
            for (var i = 0; i < _errorObjects.Count; i++)
            {
                var res = _service.Add(_errorObjects[i]);
                Assert.IsTrue(res.HasErrors);
            }
        }

        [TestMethod]
        public void Add_WithNullFields_ThenHasErrors()
        {
            var props = new List<PropertyInfo>(_defaultObjects[1].GetType().GetProperties());
            for (var i = 0; i < props.Count; i++)
            {
                if (_nullFields.Contains(props[i].Name))
                {
                    var saved = props[i].GetValue(_defaultObjects[1]);
                    props[i].SetValue(_defaultObjects[1], null);
                    var res = _service.Add(_defaultObjects[1]);
                    Assert.IsTrue(res.HasErrors);
                    props[i].SetValue(_defaultObjects[1], saved);
                }
            }
        }

        [TestMethod]
        public void Add_WithObject1_ThenNoError()
        {
            // Remove object 1 first to simulate no entry as object1
            _service.Remove(_workingObjects[1]);
            var res = _service.Add(_defaultObjects[1]);
            Assert.IsFalse(res.HasErrors);
        }

        [TestMethod]
        public void Remove_WithObject0_ThenNotContainObject0()
        {
            var obj = _workingObjects[0];
            _service.Remove(obj);
            CollectionAssert.DoesNotContain(_workingObjects, obj);
        }

        [TestMethod]
        public void Remove_WithNullObject_ThenHasError()
        {
            var res = _service.Remove(null);
            Assert.IsTrue(res.HasErrors);
        }

        [TestMethod]
        public void Find_WithoutParameter_ThenAll()
        {
            var res = _service.Find();
            CollectionAssert.AreEquivalent(_workingObjects, res);
        }

        [TestMethod]
        public void Find_WithOrderByIdAsc_ThenAllOrderedDes()
        {
            var res = _service.Find(null, "Id");
            var prop = typeof(T).GetProperties().FirstOrDefault(x => x.Name == "Id");
            Assert.IsNotNull(prop);
            for (int i = 1; i < res.Count; i++)
            {
                Assert.IsTrue((int)prop.GetValue(res[i]) > (int)prop.GetValue(res[i - 1]));
            }
        }

        [TestMethod]
        public void Find_WithOrderByIdDesc_ThenAllOrderedDes()
        {
            var res = _service.Find(null, "Id", true);
            var prop = typeof(T).GetProperties().FirstOrDefault(x => x.Name == "Id");
            Assert.IsNotNull(prop);
            for (int i = 1; i < res.Count; i++)
            {                
                Assert.IsTrue((int) prop.GetValue(res[i]) < (int) prop.GetValue(res[i-1]));
            }
        }

        [TestMethod]
        public void Find_WithFrom1Max2_Then2ResultsAndNotObject0()
        {
            var res = _service.Find(from: 1, max: 2);
            Assert.IsTrue(res.Count == 2 && !res.Contains(_workingObjects[0]));
        }

        [TestMethod]
        public void Find_WithFromMinus1MaxMinus1_Then1ResultAndFirstObject()
        {
            var res = _service.Find(from: -1, max: -1);
            Assert.IsTrue(res.Count == 1 && res[0] == _workingObjects[0]);
        }

        [TestMethod]
        public void Find_WithFrom10_ThenEmpty()
        {
            var res = _service.Find(from: 10);
            Assert.IsTrue(!res.Any());
        }
    }
}
