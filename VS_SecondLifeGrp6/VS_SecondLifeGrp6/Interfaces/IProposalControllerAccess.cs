using VS_SLG6.Api.ControllerAccess;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Api.Interfaces
{
    public interface IProposalControllerAccess : IControllerAccess<Proposal>
    {
        public bool CanGet(ContextUser ctxUser, int id, int idOrigin, int idTarget);
    }
}
