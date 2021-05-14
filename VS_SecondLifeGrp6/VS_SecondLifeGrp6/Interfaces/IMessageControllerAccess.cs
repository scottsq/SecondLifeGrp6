using VS_SLG6.Api.ControllerAccess;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Api.Interfaces
{
    public interface IMessageControllerAccess : IControllerAccess<Message>
    {
        public bool CanGet(ContextUser ctxUser, int id = -1, int idOrigin = -1, int idDest = -1);
    }
}
