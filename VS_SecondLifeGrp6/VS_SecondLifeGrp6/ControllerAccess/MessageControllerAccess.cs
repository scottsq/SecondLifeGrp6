using VS_SLG6.Model.Entities;

namespace VS_SLG6.Api.ControllerAccess
{
    public class MessageControllerAccess : ControllerAccess<Message>
    {
        public override bool CanAdd(ContextUser ctxUser, Message obj)
        {
            return obj?.Sender != null && HasId(obj.Sender.Id, ctxUser);
        }

        public override bool CanDelete(ContextUser ctxUser, Message obj)
        {
            return CanAdd(ctxUser, obj);
        }

        public override bool CanEdit(ContextUser ctxUser, Message obj)
        {
            return CanAdd(ctxUser, obj);
        }

        public override bool CanGet(ContextUser ctxUser, Message obj)
        {
            return obj?.Sender != null && obj.Receipt != null && HasId(obj.Sender.Id, ctxUser) || HasId(obj.Receipt.Id, ctxUser);
        }
    }
}
