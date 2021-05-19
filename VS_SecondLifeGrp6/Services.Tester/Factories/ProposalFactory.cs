using System;
using System.Collections.Generic;
using VS_SLG6.Model.Entities;

namespace Services.Tester.Factories
{
    public static class ProposalFactory
    {
        public static Proposal Origin1Target2Proposal;
        public static Proposal Origin1Target3Proposal;
        public static Proposal Origin2Target3Proposal;

        // Generating errors --------------------
        public static Proposal UnknownOriginProposal;
        public static Proposal UnknownTargetProposal;
        public static Proposal UnknownProductProposal;
        public static Proposal UnknownStateProposal;
        public static Proposal NegativePriceProposal;

        public static void InitFactory()
        {
            ProductFactory.InitFactory();

            // Origin, Targe, Period, State, Product, Price 
            var list = new List<Proposal> { Origin1Target2Proposal, Origin1Target3Proposal, Origin2Target3Proposal, UnknownOriginProposal, UnknownTargetProposal, UnknownProductProposal, UnknownStateProposal, NegativePriceProposal };
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
            Origin1Target2Proposal.Target = Origin2Target3Proposal.Origin = UserFactory.GenericUser2;
            UnknownOriginProposal.Origin = UnknownTargetProposal.Target = UserFactory.UnknownUser;
            UnknownProductProposal.Product = ProductFactory.UnknownProduct;
            UnknownStateProposal.State = (State)(-1);
            NegativePriceProposal.Price = -1;
        }
    }
}
