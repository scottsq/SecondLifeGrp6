using Microsoft.AspNetCore.Http;
using VS_SLG6.Api.ControllerAccess;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Api.Interfaces
{
    public interface IControllerAccess<T>
    {
        public bool CanAdd(ContextUser ctxUser, T obj);
        public bool CanEdit(ContextUser ctxUser, T obj);
        public bool CanGet(ContextUser ctxUser, T obj);
        public bool CanDelete(ContextUser ctxUser, T obj);
        public ContextUser GetUserFromContext(HttpContext context);
        public bool HasRole(Roles role, ContextUser ctxUser);
        public bool HasId(int id, ContextUser ctxUser);
    }
}
