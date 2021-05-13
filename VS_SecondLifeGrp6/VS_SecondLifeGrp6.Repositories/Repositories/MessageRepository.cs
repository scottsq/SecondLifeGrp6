using System.Collections.Generic;
using VS_SLG6.Model;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Repositories.Repositories
{
    public class MessageRepository : GenericRepository<Message>, IRepository<Message>
    {
        protected override List<string> _includes => new List<string> { nameof(Message.Sender) };

        public MessageRepository(VS_SLG6DbContext context): base(context) { }
    }
}
