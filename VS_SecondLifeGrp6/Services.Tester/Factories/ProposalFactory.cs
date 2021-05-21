using System;
using System.Collections.Generic;
using System.Linq;
using VS_SLG6.Model.Entities;

namespace Services.Tester.Factories
{
    public static class ProposalFactory
    {
        public static Proposal Origin1Target2ActiveProposal = new Proposal();
        public static Proposal Origin2Target1ActiveProposal = new Proposal();
        public static Proposal Origin1Target3AcceptedProposal = new Proposal();
        public static Proposal Origin2Target3ClosedProposal = new Proposal();

        // Generating errors --------------------
        public static Proposal UnknownOriginProposal = new Proposal();
        public static Proposal UnknownTargetProposal = new Proposal();
        public static Proposal UnknownProductProposal = new Proposal();
        public static Proposal UnknownStateProposal = new Proposal();
        public static Proposal NegativePriceProposal = new Proposal();

        public static void InitFactory()
        {
            ProductFactory.InitFactory();

            // Origin, Targe, Period, State, Product, Price 
            var list = All();
            for (int i=0; i<list.Count; i++)
            {
                var p = list[i];
                p.Id = i;
                p.Origin = UserFactory.GenericUser1;
                p.Target = UserFactory.GenericUser3;
                p.Period = TimeSpan.FromDays(7);
                p.Price = i + 1;
                p.Product = ProductFactory.GenericProduct1;
                p.State = State.ACTIVE;
            }
            Origin1Target2ActiveProposal.Target = Origin2Target3ClosedProposal.Origin = Origin2Target1ActiveProposal.Origin = UserFactory.GenericUser2;
            Origin2Target1ActiveProposal.Target = UserFactory.GenericUser1;
            UnknownOriginProposal.Origin = UnknownTargetProposal.Target = UserFactory.UnknownUser;
            UnknownProductProposal.Product = ProductFactory.UnknownProduct;
            UnknownStateProposal.State = (State)(-1);
            NegativePriceProposal.Price = -1;
            Origin1Target3AcceptedProposal.State = State.ACCEPTED;
            Origin2Target3ClosedProposal.State = State.CLOSED;
        }

        public static List<Proposal> All()
        {
            return GenericList().Concat(ErrorList()).ToList();
        }

        public static List<Proposal> GenericList()
        {
            return new List<Proposal> {
                Origin1Target2ActiveProposal,
                Origin2Target1ActiveProposal,
                Origin1Target3AcceptedProposal,
                Origin2Target3ClosedProposal
            };
        }

        public static List<Proposal> ErrorList()
        {
            return new List<Proposal> {
                UnknownOriginProposal,
                UnknownTargetProposal,
                UnknownProductProposal,
                UnknownStateProposal,
                NegativePriceProposal
            };
        }
    }
}
