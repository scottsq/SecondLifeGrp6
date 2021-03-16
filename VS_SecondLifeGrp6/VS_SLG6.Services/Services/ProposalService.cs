using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;
using VS_SLG6.Services.Validators;

namespace VS_SLG6.Services.Services
{
    public class ProposalService : GenericService<Proposal>, IProposalService
    {
        public ProposalService(IRepository<Proposal> repo, IValidator<Proposal> validator) : base(repo, validator)
        {
        }

        public List<Proposal> GetAcceptedProposalByUser(int id)
        {
            return _repo.All(x => x.State == State.ACCEPTED && (x.Target.Id == id || x.Origin.Id == id));
        }

        public List<Proposal> ListByUserId(int id)
        {
            return _repo.All(x => x.Target.Id == id || x.Origin.Id == id);
        }

        public List<Proposal> ListByUserIdAndActive(int id)
        {
            return _repo.All(x => (x.Target.Id == id || x.Origin.Id == id) && x.State == State.ACTIVE);
        }

        public ValidationModel<Proposal> UpdateProposal(int id, State state)
        {
            var p = Get(id);
            if (p == null) _validationModel.Errors.Add("Proposal not found");
            else
            {
                p.State = state;
                _repo.Update(p);
                _validationModel.Value = p;
            }
            return _validationModel;
        }
    }
}
