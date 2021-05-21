using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Tester.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Services;
using VS_SLG6.Services.Validators;

namespace Services.Tester.ServiceTesters
{
    [TestClass]
    public class TagServiceTester : GenericServiceTester<Tag>
    {
        public TagServiceTester()
        {
            TagFactory.InitFactory();
            InitTests();
        }

        private void InitTests()
        {
            Func<object[], Tag> findOneFunc = x => _workingObjects.Find(t => t.Id == Convert.ToInt32(x[0]));
            InitBehavior(findOneFunc, TagFactory.GenericList());

            _validator = new TagValidator(_repo.Object);
            _service = new TagService(_repo.Object, _validator);
            _errorObjects = TagFactory.ErrorList();
            _nullFields = new List<string> { "Name" };
            _fieldOrderBy = nameof(Tag.Name);
        }

        [TestMethod]
        public void Find_WithName2_Then1ResultAndGenericTag2()
        {
            var res = (_service as TagService).Find(name: "2");
            Assert.IsTrue(res.Count == 1 && res[0].Id == TagFactory.GenericTag2.Id);
        }

        [TestMethod]
        public void Find_WithNameAYAYA_ThenEmpty()
        {
            var res = (_service as TagService).Find(name: "AYAYA");
            Assert.IsFalse(res.Any());
        }
    }
}
