using VS_SLG6.Api.Interfaces;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;

namespace VS_SLG6.Api.ControllerAccess
{
    public class UserControllerAccess : GenericControllerAccess<User>, IUserControllerAccess
    {
        public UserControllerAccess(IRepository<User> repo): base(repo) { }

        public override bool CanDelete(ContextUser ctxUser, User obj)
        {
            return obj != null && HasId(obj.Id, ctxUser);
        }

        public override bool CanEdit(ContextUser ctxUser, User obj)
        {
            return CanDelete(ctxUser, obj);
        }
    }
}
