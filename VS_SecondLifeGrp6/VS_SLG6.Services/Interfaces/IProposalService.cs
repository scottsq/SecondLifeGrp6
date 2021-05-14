using System.Collections.Generic;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Models;

namespace VS_SLG6.Services.Interfaces
{
    public interface IProposalService : IService<Proposal>
    {
        public List<Proposal> Find(int id = -1, int originId = -1, int targetId = -1, State[] states = null, string orderBy = null, bool reverse = false, int from = 0, int max = 10);
        public ValidationModel<Proposal> Update(int id, State state);
    }
}
