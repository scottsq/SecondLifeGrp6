using VS_SLG6.Model.Entities;

namespace VS_SLG6.Api.ControllerAccess
{
    public class ProductRatingControllerAccess : ControllerAccess<ProductRating>
    {
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
