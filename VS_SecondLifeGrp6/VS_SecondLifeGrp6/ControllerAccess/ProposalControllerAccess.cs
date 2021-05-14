using VS_SLG6.Api.Interfaces;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;

namespace VS_SLG6.Api.ControllerAccess
{
    public class ProposalControllerAccess : GenericControllerAccess<Proposal>, IProposalControllerAccess
    {
        public ProposalControllerAccess(IRepository<Proposal> repo) : base(repo) { }

        public override bool CanAdd(ContextUser ctxUser, Proposal obj)
        {
            return obj?.Origin != null && HasId(obj.Origin.Id, ctxUser);
        }

        public override bool CanDelete(ContextUser ctxUser, Proposal obj)
        {
            return HasRole(Roles.ADMIN, ctxUser);
        }

        public override bool CanEdit(ContextUser ctxUser, Proposal obj)
        {
            return CanAdd(ctxUser, obj);
        }

        public override bool CanGet(ContextUser ctxUser, Proposal obj)
        {
            return  obj?.Origin != null && obj?.Target != null 
                    && (HasId(obj.Origin.Id, ctxUser) || HasId(obj.Target.Id, ctxUser));
        }

        public bool CanGet(ContextUser ctxUser, int id, int idOrigin, int idTarget)
        {
            if (id < 0) return HasId(idOrigin, ctxUser) || HasId(idTarget, ctxUser);
            return CanGet(ctxUser, _repo.FindOne(id));
        }
    }
}
