using System.Collections.Generic;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Services.Services
{
    public interface IMessageService : IService<Message>
    {
        public List<Message> Find(int idOrigin = -1, int idDest = -1, bool twoWays = false, int from = 0, int max = 10);
    }
}
