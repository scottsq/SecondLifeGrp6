using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Interfaces;
using VS_SLG6.Services.Models;
using VS_SLG6.Services.Validators;

namespace VS_SLG6.Services.Services
{
    public class ProposalService : GenericService<Proposal>, IProposalService
    {
        public ProposalService(IRepository<Proposal> repo, IValidator<Proposal> validator) : base(repo, validator)
        {
        }

        public ValidationModel<Proposal> Update(int id, State state)
        {
            var vmProposal = Get(id);
            if (vmProposal.HasErrors || vmProposal.Value == null) return vmProposal;

            var vmResult = GetErrors<Proposal>(_validator.CanEdit(vmProposal.Value));
            if (!vmResult.HasErrors) 
            {
                vmProposal.Value.State = state;
                _repo.Update(vmProposal.Value);
            }
            return vmProposal;
        }

        public List<Proposal> Find(int id = -1, int originId = -1, int targetId = -1, State state = State.ALL, string orderBy = null, bool reverse = false, int from = 0, int max = 10)
        {
            return _repo.All(
                GenerateCondition(id, originId, targetId, state),
                GenerateOrderByCondition(orderBy),
                reverse, from, max
            );
        }

        public static Expression<Func<Proposal, bool>> GenerateCondition(int id = -1, int originId = -1, int targetId = -1, State state = State.ALL)
        {
            Expression<Func<Proposal, bool>> condition = x => true;
            if (id > -1) condition = condition.And(x => x.Id == id);
            if (originId > -1) condition = condition.And(x => x.Origin.Id == originId);
            if (targetId > -1) condition = condition.And(x => x.Target.Id == targetId);
            if (state != State.ALL) condition = condition.And(x => x.State == state);
            return condition;
        }
    }
}
