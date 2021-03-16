using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;

namespace VS_SLG6.Services.Services
{
    public class Product_Proposal : IProduct_Proposal
    {
        private IRepository<Proposal> _repoProposal;

        public Product_Proposal(IRepository<Proposal> repoProposal)
        {
            _repoProposal = repoProposal;
        }

        public List<Proposal> GetAcceptedProposalByUser(int id)
        {
            var p = new ProposalService(_repoProposal, null);
            return p.GetAcceptedProposalByUser(id);
        }
    }
}
