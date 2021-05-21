using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;

namespace VS_SLG6.Api.ControllerAccess
{
    public class UserRatingControllerAccess : GenericControllerAccess<UserRating>
    {
        public UserRatingControllerAccess(IRepository<UserRating> repo) : base(repo)
        {
        }

        public override bool CanAdd(ContextUser ctxUser, UserRating obj)
        {
            return obj?.Origin != null && HasId(obj.Origin.Id, ctxUser);
        }

        public override bool CanDelete(ContextUser ctxUser, UserRating obj)
        {
            return CanAdd(ctxUser, obj);
        }

        public override bool CanEdit(ContextUser ctxUser, UserRating obj)
        {
            return CanAdd(ctxUser, obj);
        }
    }
}
