using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;

namespace VS_SLG6.Api.ControllerAccess
{
    public class PhotoControllerAccess : GenericControllerAccess<Photo>
    {
        public PhotoControllerAccess(IRepository<Photo> repo) : base(repo) { }

        public override bool CanAdd(ContextUser ctxUser, Photo obj)
        {
            return obj?.Product?.Owner != null && HasId(obj.Product.Owner.Id, ctxUser);
        }

        public override bool CanEdit(ContextUser ctxUser, Photo obj)
        {
            return CanAdd(ctxUser, obj);
        }

        public override bool CanDelete(ContextUser ctxUser, Photo obj)
        {
            return CanAdd(ctxUser, obj);
        }
    }
}
