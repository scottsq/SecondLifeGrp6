using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;

namespace VS_SLG6.Api.ControllerAccess
{
    public class ProductTagControllerAccess : GenericControllerAccess<ProductTag>
    {
        public ProductTagControllerAccess(IRepository<ProductTag> repo) : base(repo) { }

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
