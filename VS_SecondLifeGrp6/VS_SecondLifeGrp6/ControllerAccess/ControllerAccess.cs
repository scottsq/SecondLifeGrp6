using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Api.ControllerAccess
{
    public class ControllerAccess<T> : IControllerAccess<T>
    {
        public virtual bool CanAdd(ContextUser ctxUser, T obj)
        {
            return true;
        }

        public virtual bool CanDelete(ContextUser ctxUser, T obj)
        {
            return true;
        }

        public virtual bool CanEdit(ContextUser ctxUser, T obj)
        {
            return true;
        }

        public virtual bool CanGet(ContextUser ctxUser, T obj)
        {
            return true;
        }

        public ContextUser GetUserFromContext(HttpContext context)
        {
            var claims = context?.User?.Claims;
            if (claims == null) return null;

            int id = -1;
            int.TryParse(claims.FirstOrDefault(x => x.Type == "user_id")?.Value, out id);

            int role = 1;
            int.TryParse(claims.FirstOrDefault(x => x.Type == "user_role")?.Value, out role);
            if (role < 0 || role > Enum.GetValues(typeof(Roles)).Length) role = 1;

            return new ContextUser { Id = id, Role = (Roles)role };

        }

        public bool HasRole(Roles role, ContextUser ctxUser)
        {
            if (ctxUser == null) return false;
            return ctxUser.Role == role || ctxUser.Role == Roles.ADMIN;
        }

        public bool HasId(int id, ContextUser ctxUser)
        {
            if (ctxUser == null) return false;
            return ctxUser.Id == id || ctxUser.Role == Roles.ADMIN;
        }
    }
}
