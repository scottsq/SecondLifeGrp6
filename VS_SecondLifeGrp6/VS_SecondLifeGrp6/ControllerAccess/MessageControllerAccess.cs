using VS_SLG6.Api.Interfaces;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;

namespace VS_SLG6.Api.ControllerAccess
{
    public class MessageControllerAccess : GenericControllerAccess<Message>, IMessageControllerAccess
    {
        public MessageControllerAccess(IRepository<Message> repo) : base(repo) { }

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

        public bool CanGet(ContextUser ctxUser, int id, int idOrigin = -1, int idDest = -1)
        {
            if (id < 0) return HasId(idOrigin, ctxUser) || HasId(idDest, ctxUser);
            return CanGet(ctxUser, _repo.FindOne(id));
        }
    }
}
