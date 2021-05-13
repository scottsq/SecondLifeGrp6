using VS_SLG6.Model.Entities;

namespace VS_SLG6.Api.ControllerAccess
{
    public class ProposalControllerAccess : ControllerAccess<Proposal>
    {
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
    }
}
