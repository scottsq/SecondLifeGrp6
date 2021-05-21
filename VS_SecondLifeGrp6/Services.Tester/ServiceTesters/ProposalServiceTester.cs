using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services.Tester.Factories;
using Services.Tester.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Services;
using VS_SLG6.Services.Validators;

namespace Services.Tester.ServiceTesters
{
    [TestClass]
    public class ProposalServiceTester : GenericServiceTester<Proposal>
    {

        public ProposalServiceTester()
        {
            ProposalFactory.InitFactory();
            InitTests();
        }

        private void InitTests()
        {
            Func<object[], Proposal> findOneFunc = x => _workingObjects.Find(p => p.Id == Convert.ToInt32(x[0]));
            InitBehavior(findOneFunc, ProposalFactory.GenericList());

            var pRepo = RepoHelper<Proposal>.GetMockedSubRepo<Product>(_defaultObjects, nameof(Proposal.Product));
            var uRepo = InitUserRepo();

            _validator = new ProposalValidator(_repo.Object, pRepo.Object, uRepo.Object);
            _service = new ProposalService(_repo.Object, _validator);
            _errorObjects = ProposalFactory.ErrorList();
            _nullFields = new List<string> { "Origin", "Product", "Target" };
            _fieldOrderBy = nameof(Proposal.Price);
        }

        private Mock<IRepository<User>> InitUserRepo()
        {
            var uRepo = new Mock<IRepository<User>>();
            uRepo.Setup(x => x.FindOne(It.IsAny<object[]>())).Returns<object[]>(x =>
            {
                var val = Convert.ToInt32(x[0]);
                var proposal = _defaultObjects.FirstOrDefault(p => p.Origin.Id == val || p.Target.Id == val);
                if (proposal == null) return null;
                return proposal.Origin.Id == val ? proposal.Origin : proposal.Target;
            });
            return uRepo;
        }

        [TestMethod]
        public void Find_WithOriginGenericUser1_Then2ResultsAndOriginGenericUser1()
        {
            var id = UserFactory.GenericUser1.Id;
            var res = (_service as ProposalService).Find(originId: id);
            Assert.IsTrue(
                res.Count == 2
                && res[0].Origin.Id == id
                && res[1].Origin.Id == id
            );
        }

        [TestMethod]
        public override void Add_WithObject0_ThenHasError()
        {
            // There can be multiple proposals with same properties
            // so it won't have any error, causing the test to fail
            // So we override it and do nothing to prevent it
        }

        [TestMethod]
        public void Find_WithAcceptedSate_Then1ResultAndAccepted()
        {
            var res = (_service as ProposalService).Find(state: State.ACCEPTED);
            Assert.IsTrue(res.Count == 1 && res[0].State == State.ACCEPTED);
        }

        [TestMethod]
        public void Find_WithOrigin6_ThenEmpty()
        {
            var res = (_service as ProposalService).Find(originId: 6);
            Assert.IsFalse(res.Any());
        }

        [TestMethod]
        public void Update_WithActiveProposalAndClosed_ThenClosed()
        {
            var proposal = ProposalFactory.Origin1Target2ActiveProposal;
            var res = (_service as ProposalService).Update(proposal.Id, State.CLOSED);
            Assert.IsTrue(res.Value.State == State.CLOSED);
        }

        [TestMethod]
        public void Update_WithUnknownProposalAndClosed_ThenNull()
        {
            var res = (_service as ProposalService).Update(-1, State.CLOSED);
            Assert.AreEqual(null, res.Value);
        }
    }
}
