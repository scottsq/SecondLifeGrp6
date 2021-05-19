using System;
using System.Collections.Generic;
using VS_SLG6.Model.Entities;

namespace Services.Tester.Factories
{
    public static class ProposalFactory
    {
        public static Proposal Origin1Target2Proposal = new Proposal();
        public static Proposal Origin1Target3Proposal = new Proposal();
        public static Proposal Origin2Target3Proposal = new Proposal();

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
            var list = List();
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

        public static List<Proposal> List()
        {
            return new List<Proposal> { 
                Origin1Target2Proposal, 
                Origin1Target3Proposal, 
                Origin2Target3Proposal, 
                UnknownOriginProposal, 
                UnknownTargetProposal, 
                UnknownProductProposal, 
                UnknownStateProposal, 
                NegativePriceProposal 
            };
        }
    }
}
