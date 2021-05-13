using System.Collections.Generic;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Models;

namespace VS_SLG6.Services.Services
{
    public interface IProposalService : IService<Proposal>
    {
        public List<Proposal> Find(int userId = -1, State[] states = null, int from = 0, int max = 10);
        public ValidationModel<Proposal> Update(int id, State state);
    }
}
