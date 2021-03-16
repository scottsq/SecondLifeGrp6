using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Models;
using VS_SLG6.Services.Services;
using VS_SLG6.Services.Validators;

namespace Services.Tester
{
    [TestClass]
    public class TagServiceTester : GenericServiceTester<Tag>
    {
        private Tag t1 = new Tag();
        private Tag t2 = new Tag();

        public TagServiceTester()
        {
            t1.Id = 0;
            t1.Name = "Tag 1";
            t2.Id = 1;
            t2.Name = "Tag 2";
            InitBehavior(t1, t2);
            InitTests();
        }

        private void InitTests()
        {
            _validator = new TagValidator(_repo.Object, new ValidationModel<bool>());
            _repo.Setup(x => x.FindOne(It.IsAny<object[]>())).Returns<object[]>(x => {
                return _workingObjects.Find(u => u.Id == Int32.Parse(x[0].ToString()));
            });
            _service = new GenericService<Tag>(_repo.Object, _validator);
            nullFields = new List<string> { "Name" };
        }
    }
}
