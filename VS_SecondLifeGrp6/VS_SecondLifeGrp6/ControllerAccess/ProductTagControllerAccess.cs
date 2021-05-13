using VS_SLG6.Model.Entities;

namespace VS_SLG6.Api.ControllerAccess
{
    public class ProductTagControllerAccess : ControllerAccess<ProductTag>
    {
        public override bool CanAdd(ContextUser ctxUser, ProductTag obj)
        {
            return obj?.Product?.Owner != null && HasId(obj.Product.Owner.Id, ctxUser);
        }

        public override bool CanDelete(ContextUser ctxUser, ProductTag obj)
        {
            return CanAdd(ctxUser, obj);
        }

        public override bool CanEdit(ContextUser ctxUser, ProductTag obj)
        {
            return CanAdd(ctxUser, obj);
        }
    }
}
