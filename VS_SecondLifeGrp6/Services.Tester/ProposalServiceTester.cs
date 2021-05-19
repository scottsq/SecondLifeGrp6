﻿/*using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class ProposalServiceTester : GenericServiceTester<Proposal>
    {
        private Proposal _proposal = new Proposal();
        private Proposal _proposal1 = new Proposal();
        private Proposal _proposal2 = new Proposal();
        private Product _p1 = new Product();
        private Product _p2 = new Product();
        private User _u1 = new User();
        private User _u2 = new User();
        private User _u3 = new User();

        public ProposalServiceTester()
        {
            CreateInstances();
            InitBehavior(_proposal1, _proposal2);
            InitTests();
        }

        private void CreateInstances()
        {
            _u1.Id = 0; _u2.Id = 1; _u3.Id = 2;
            _p1.Id = 0; _p1.Owner = _u1;
            _p2.Id = 1; _p2.Owner = _u2;

            _proposal.Id = 2;
            _proposal.Origin = _u1;
            _proposal.Period = new TimeSpan(30, 0, 0, 0, 0);
            _proposal.Price = 50;
            _proposal.Product = _p1;
            _proposal.State = State.ACTIVE;
            _proposal.Target = _u2;

            _proposal1 = Clone(_proposal, _proposal1);
            _proposal1.Id = 0;
            _proposal1.Origin = _u2;
            _proposal1.Target = _u1;

            _proposal2.Id = 1;
            _proposal2.Origin = _u1;
            _proposal2.Period = new TimeSpan(7, 0, 0, 0, 0);
            _proposal2.Price = 33;
            _proposal2.Product = _p2;
            _proposal2.State = State.ACCEPTED;
            _proposal2.Target = _u3;
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

            var uRepo = new Mock<IRepository<User>>();
            uRepo.Setup(x => x.FindOne(It.IsAny<int>())).Returns<int>(x =>
            {
                if (x == _u1.Id) return _u1;
                if (x == _u2.Id) return _u2;
                if (x == _u3.Id) return _u3;
                return null;
            });

            _validator = new ProposalValidator(_repo.Object, new ValidationModel<bool>(), pRepo.Object, uRepo.Object);
            _repo.Setup(x => x.FindOne(It.IsAny<int>())).Returns<int>(x => {
                return _workingObjects.Find(m => m.Id == x);
            });

            _service = new ProposalService(_repo.Object, _validator);
            nullFields = new List<string> { "Origin", "Product", "Target" };
        }

        [TestMethod]
        public void Add_WithUnknownProduct_ThenError()
        {
            var p = new Product(); p.Id = -1;
            _proposal.Product = p;
            var res = _service.Add(_proposal);
            Assert.AreNotEqual(0, res.Errors.Count);
        }

        [TestMethod]
        public void Add_WithUnknownState_ThenError()
        {
            _proposal.State = (State)10;
            var res = _service.Add(_proposal);
            Assert.AreNotEqual(0, res.Errors.Count);
        }

        [TestMethod]
        public void Add_WithUnknownOrigin_ThenError()
        {
            var o = new User(); o.Id = -1;
            _proposal.Origin = o;
            var res = _service.Add(_proposal);
            Assert.AreNotEqual(0, res.Errors.Count);
        }

        [TestMethod]
        public void Add_WithUnknownTarget_ThenError()
        {
            var t = new User(); t.Id = -1;
            _proposal.Target = t;
            var res = _service.Add(_proposal);
            Assert.AreNotEqual(0, res.Errors.Count);
        }

        [TestMethod]
        public void GetAcceptedProposalByUser_WithU1_ThenEmpty()
        {
            var res = ((ProposalService)_service).GetAcceptedByUser(_u1.Id);
            Assert.AreEqual(0, res.Value.Count);
        }

        [TestMethod]
        public void GetAcceptedProposalByUser_WithU2_ThenEmpty()
        {
            var res = ((ProposalService)_service).GetAcceptedByUser(_u2.Id);
            Assert.AreEqual(0, res.Value.Count);
        }

        [TestMethod]
        public void GetAcceptedProposalByUser_WithU3_ThenNotEmpty()
        {
            _service.Add(_proposal2);
            ((ProposalService)_service).Update(_proposal2.Id, State.ACCEPTED);
            var res = ((ProposalService)_service).GetAcceptedByUser(_u3.Id);
            Assert.AreNotEqual(0, res.Value.Count);
        }

        [TestMethod]
        public void GetAcceptedProposalByUser_WithU3_ThenAccepted()
        {
            _service.Add(_proposal2);
            var res = ((ProposalService)_service).GetAcceptedByUser(_u3.Id);
            for (var i=0; i<res.Value.Count; i++) Assert.AreEqual(State.ACCEPTED, res.Value[i].State);
        }

        [TestMethod]
        public void ListByUserId_WithU1_ThenNotEmpty()
        {
            var res = ((ProposalService)_service).ListByUserId(_u1.Id);
            Assert.AreNotEqual(0, res.Value.Count);
        }

        [TestMethod]
        public void ListByUserId_WithU2_ThenNotEmpty()
        {
            var res = ((ProposalService)_service).ListByUserId(_u2.Id);
            Assert.AreNotEqual(0, res.Value.Count);
        }

        [TestMethod]
        public void ListByUserId_WithU3_ThenEmpty()
        {
            var res = ((ProposalService)_service).ListByUserId(_u3.Id);
            Assert.AreEqual(0, res.Value.Count);
        }

        [TestMethod]
        public void ListByUserIdAndActive_WithU1_ThenNotEmpty()
        {
            var res = ((ProposalService)_service).ListByUserIdAndActive(_u1.Id);
            Assert.AreNotEqual(0, res.Value.Count);
        }

        [TestMethod]
        public void ListByUserIdAndActive_WithU2_ThenNotEmpty()
        {
            var res = ((ProposalService)_service).ListByUserIdAndActive(_u2.Id);
            Assert.AreNotEqual(0, res.Value.Count);
        }

        [TestMethod]
        public void ListByUserIdAndActive_WithU3_ThenEmpty()
        {
            _service.Add(_proposal2);
            ((ProposalService)_service).Update(_proposal2.Id, State.ACCEPTED);
            var res = ((ProposalService)_service).ListByUserIdAndActive(_u3.Id);
            Assert.AreEqual(0, res.Value.Count);
        }

        [TestMethod]
        public void UpdateProposal_WithPr1AndAccepted_ThenAccepted()
        {
            var res = ((ProposalService)_service).Update(_proposal1.Id, State.ACCEPTED);
            Assert.AreEqual(State.ACCEPTED, res.Value.State);
        }

        [TestMethod]
        public void UpdateProposal_WithPr2AndAccepted_ThenValidationError()
        {
            var res = ((ProposalService)_service).Update(_proposal2.Id, State.ACCEPTED);
            Assert.AreNotEqual(0, res.Errors.Count);
        }
    }
}
*/