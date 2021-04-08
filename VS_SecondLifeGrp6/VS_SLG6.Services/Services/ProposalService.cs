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

        public ValidationModel<List<Proposal>> GetAcceptedProposalByUser(int id)
        {
            var list = _repo.All(x => x.State == State.ACCEPTED && (x.Target.Id == id || x.Origin.Id == id)); 
            return SetValidation(list);
        }

        public ValidationModel<List<Proposal>> ListByUserId(int id)
        {
            var list = _repo.All(x => x.Target.Id == id || x.Origin.Id == id); 
            return SetValidation(list);
        }

        public ValidationModel<List<Proposal>> ListByUserIdAndActive(int id)
        {
            var list = _repo.All(x => (x.Target.Id == id || x.Origin.Id == id) && x.State == State.ACTIVE);
            return SetValidation(list);
        }

        private ValidationModel<List<Proposal>> SetValidation(List<Proposal> list)
        {
            var res = new ValidationModel<List<Proposal>>();
            if (list.Count == 0)
            {
                res.Value = list;
                return res;
            }
            var v = _validator.CanGet(list[0]);
            if (!v.Value) res.Errors = v.Errors;
            else res.Value = list;
            return res;
        }

        public ValidationModel<Proposal> UpdateProposal(int id, State state)
        {
            var p = Get(id);
            if (p.Errors.Count > 0) {
                _validationModel.Errors = p.Errors;
                return _validationModel;
            }
            if (p.Value == null)
            {
                _validationModel.Errors.Add("Proposal not found");
                return _validationModel;
            }

            var check = _validator.CanEdit(p.Value);
            if (!check.Value) _validationModel.Errors = check.Errors;
            else
            {
                p.Value.State = state;
                _repo.Update(p.Value);
                _validationModel.Value = p.Value;
            }
            return _validationModel;
        }
    }
}
