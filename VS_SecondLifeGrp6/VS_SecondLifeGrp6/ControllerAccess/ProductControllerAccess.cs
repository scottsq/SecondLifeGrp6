using VS_SLG6.Model.Entities;

namespace VS_SLG6.Api.ControllerAccess
{
    public class ProductControllerAccess : ControllerAccess<Product>
    {
        public override bool CanAdd(ContextUser ctxUser, Product obj)
        {
            return obj?.Owner != null && HasId(obj.Owner.Id, ctxUser);
        }

        public override bool CanEdit(ContextUser ctxUser, Product obj)
        {
            return CanAdd(ctxUser, obj);
        }

        public override bool CanDelete(ContextUser ctxUser, Product obj)
        {
            return CanAdd(ctxUser, obj);
        }
    }
}
