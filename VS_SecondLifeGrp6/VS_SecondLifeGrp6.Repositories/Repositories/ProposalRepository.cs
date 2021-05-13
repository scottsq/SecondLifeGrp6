using System.Collections.Generic;
using VS_SLG6.Model;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Repositories.Repositories
{
    public class ProposalRepository : GenericRepository<Proposal>, IRepository<Proposal>
    {
        protected override List<string> _includes => new List<string> { nameof(Proposal.Product), nameof(Proposal.Origin) };

        public ProposalRepository(VS_SLG6DbContext context) : base(context) { }
    }
}
