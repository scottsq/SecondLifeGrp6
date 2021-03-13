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
    public class ProposalServiceTester : GenericServiceTester<Proposal>
    {
        private Proposal proposal = new Proposal();
        private Proposal pr1 = new Proposal();
        private Proposal pr2 = new Proposal();
        private Product p1;
        private Product p2;
        private User u1;
        private User u2;
        private User u3;

        public ProposalServiceTester()
        {
            u1 = new User(); u1.Id = 0;
            u2 = new User(); u2.Id = 1;
            u3 = new User(); u3.Id = 2;

            p1 = new Product(); p1.Id = 0;
            p2 = new Product(); p2.Id = 1;

            proposal.Id = 2;
            proposal.Origin = u1;
            proposal.Period = new TimeSpan(30, 0, 0, 0, 0);
            proposal.Price = 50;
            proposal.Product = p1;
            proposal.State = State.ACTIVE;
            proposal.Target = u2;

            pr1 = Clone(proposal, pr1);
            pr1.Id = 0;
            pr1.Origin = u2;
            pr1.Target = u1;

            pr2.Id = 1;
            pr2.Origin = u1;
            pr2.Period = new TimeSpan(7, 0, 0, 0, 0);
            pr2.Price = 33;
            pr2.Product = p2;
            pr2.State = State.ACCEPTED;
            pr2.Target = u3;

            InitBehavior(pr1, pr2);
            InitTests();
        }

        private void InitTests()
        {
            var pService = new Mock<ProductService>(new Mock<IRepository<Product>>().Object, new Mock<IValidator<Product>>().Object, null, null);
            pService.Setup(x => x.Get(It.IsAny<int>())).Returns<int>(x =>
            {
                if (x == p1.Id) return p1;
                if (x == p2.Id) return p2;
                return null;
            });
            var uService = new Mock<UserService>(new Mock<IRepository<User>>().Object, new Mock<IValidator<User>>().Object);
            uService.Setup(x => x.Get(It.IsAny<int>())).Returns<int>(x =>
            {
                if (x == u1.Id) return u1;
                if (x == u2.Id) return u2;
                if (x == u3.Id) return u3;
                return null;
            });

            _validator = new ProposalValidator(_repo.Object, new ValidationModel<bool>(), pService.Object, uService.Object);
            _repo.Setup(x => x.FindOne(It.IsAny<object[]>())).Returns<object[]>(x => {
                return _workingObjects.Find(m => m.Id == Int32.Parse(x[0].ToString()));
            });

            _service = new ProposalService(_repo.Object, _validator);
            nullFields = new List<string> { "Origin", "Product", "Target" };
        }

        [TestMethod]
        public override void Add_WithObject0_ThenValidationError() {
            // Not revelant because we can have multiple time same kind of proposal
        }

        [TestMethod]
        public void GetAcceptedProposalByUser_WithU1_ThenEmpty()
        {
            var res = ((ProposalService)_service).GetAcceptedProposalByUser(u1.Id);
            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        public void GetAcceptedProposalByUser_WithU2_ThenEmpty()
        {
            var res = ((ProposalService)_service).GetAcceptedProposalByUser(u2.Id);
            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        public void GetAcceptedProposalByUser_WithU3_ThenNotEmpty()
        {
            _service.Add(pr2);
            ((ProposalService)_service).UpdateProposal(pr2.Id, State.ACCEPTED);
            var res = ((ProposalService)_service).GetAcceptedProposalByUser(u3.Id);
            Assert.AreNotEqual(0, res.Count);
        }

        [TestMethod]
        public void GetAcceptedProposalByUser_WithU3_ThenAccepted()
        {
            _service.Add(pr2);
            var res = ((ProposalService)_service).GetAcceptedProposalByUser(u3.Id);
            for (var i=0; i<res.Count; i++) Assert.AreEqual(State.ACCEPTED, res[i].State);
        }

        [TestMethod]
        public void ListByUserId_WithU1_ThenNotEmpty()
        {
            var res = ((ProposalService)_service).ListByUserId(u1.Id);
            Assert.AreNotEqual(0, res.Count);
        }

        [TestMethod]
        public void ListByUserId_WithU2_ThenNotEmpty()
        {
            var res = ((ProposalService)_service).ListByUserId(u2.Id);
            Assert.AreNotEqual(0, res.Count);
        }

        [TestMethod]
        public void ListByUserId_WithU3_ThenEmpty()
        {
            var res = ((ProposalService)_service).ListByUserId(u3.Id);
            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        public void ListByUserIdAndActive_WithU1_ThenNotEmpty()
        {
            var res = ((ProposalService)_service).ListByUserIdAndActive(u1.Id);
            Assert.AreNotEqual(0, res.Count);
        }

        [TestMethod]
        public void ListByUserIdAndActive_WithU2_ThenNotEmpty()
        {
            var res = ((ProposalService)_service).ListByUserIdAndActive(u2.Id);
            Assert.AreNotEqual(0, res.Count);
        }

        [TestMethod]
        public void ListByUserIdAndActive_WithU3_ThenEmpty()
        {
            _service.Add(pr2);
            ((ProposalService)_service).UpdateProposal(pr2.Id, State.ACCEPTED);
            var res = ((ProposalService)_service).ListByUserIdAndActive(u3.Id);
            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        public void UpdateProposal_WithPr1AndAccepted_ThenAccepted()
        {
            var res = ((ProposalService)_service).UpdateProposal(pr1.Id, State.ACCEPTED);
            Assert.AreEqual(State.ACCEPTED, res.Value.State);
        }

        [TestMethod]
        public void UpdateProposal_WithPr2AndAccepted_ThenValidationError()
        {
            var res = ((ProposalService)_service).UpdateProposal(pr2.Id, State.ACCEPTED);
            Assert.AreNotEqual(0, res.Errors.Count);
        }
    }
}
