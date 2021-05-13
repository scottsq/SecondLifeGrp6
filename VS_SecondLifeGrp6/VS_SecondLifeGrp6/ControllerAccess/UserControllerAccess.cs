using VS_SLG6.Model.Entities;

namespace VS_SLG6.Api.ControllerAccess
{
    public class UserControllerAccess : ControllerAccess<User>
    {
        public override bool CanDelete(ContextUser ctxUser, User obj)
        {
            return obj != null && HasId(obj.Id, ctxUser);
        }

        public override bool CanEdit(ContextUser ctxUser, User obj)
        {
            return CanDelete(ctxUser, obj);
        }

        public override bool CanGet(ContextUser ctxUser, User obj)
        {
            return CanDelete(ctxUser, obj);
        }
    }
}
