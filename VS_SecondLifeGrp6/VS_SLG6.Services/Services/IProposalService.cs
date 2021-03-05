using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Models;

namespace VS_SLG6.Services.Services
{
    public interface IProposalService : IService<Proposal>
    {
        public List<Proposal> GetAcceptedProposalByUser(int id);
        public List<Proposal> ListByUserId(int id);
        public List<Proposal> ListByUserIdAndActive(int id);
        public ValidationModel<Proposal> UpdateProposal(int id, State state);
    }
}
