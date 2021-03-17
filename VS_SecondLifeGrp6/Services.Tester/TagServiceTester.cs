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
        private Tag _tag1 = new Tag();
        private Tag _tag2 = new Tag();

        public TagServiceTester()
        {
            CreateInstances();
            InitBehavior(_tag1, _tag2);
            InitTests();
        }

        private void CreateInstances()
        {
            _tag1.Id = 0;
            _tag1.Name = "Tag 1";
            _tag2.Id = 1;
            _tag2.Name = "Tag 2";
        }

        private void InitTests()
        {
            _validator = new TagValidator(_repo.Object, new ValidationModel<bool>());
            _repo.Setup(x => x.FindOne(It.IsAny<int>())).Returns<int>(x => {
                return _workingObjects.Find(u => u.Id == x);
            });
            _service = new GenericService<Tag>(_repo.Object, _validator);
            nullFields = new List<string> { "Name" };
        }
    }
}
