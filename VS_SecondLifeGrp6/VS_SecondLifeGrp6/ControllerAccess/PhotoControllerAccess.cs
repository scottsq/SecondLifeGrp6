using VS_SLG6.Model.Entities;

namespace VS_SLG6.Api.ControllerAccess
{
    public class PhotoControllerAccess : ControllerAccess<Photo>
    {
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
