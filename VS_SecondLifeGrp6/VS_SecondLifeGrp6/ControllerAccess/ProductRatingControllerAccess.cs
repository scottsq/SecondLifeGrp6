using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;

namespace VS_SLG6.Api.ControllerAccess
{
    public class ProductRatingControllerAccess : GenericControllerAccess<ProductRating>
    {
        public ProductRatingControllerAccess(IRepository<ProductRating> repo) : base(repo) { }

        public override bool CanAdd(ContextUser ctxUser, ProductRating obj)
        {
            return obj?.User != null && HasId(obj.User.Id, ctxUser);
        }

        public override bool CanEdit(ContextUser ctxUser, ProductRating obj)
        {
            return CanAdd(ctxUser, obj);
        }

        public override bool CanDelete(ContextUser ctxUser, ProductRating obj)
        {
            return CanAdd(ctxUser, obj);
        }
    }
}
