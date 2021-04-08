using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Models;

namespace VS_SLG6.Services.Services
{
    public interface IProposalService : IService<Proposal>
    {
        public ValidationModel<List<Proposal>> GetAcceptedProposalByUser(int id);
        public ValidationModel<List<Proposal>> ListByUserId(int id);
        public ValidationModel<List<Proposal>> ListByUserIdAndActive(int id);
        public ValidationModel<Proposal> UpdateProposal(int id, State state);
    }
}
