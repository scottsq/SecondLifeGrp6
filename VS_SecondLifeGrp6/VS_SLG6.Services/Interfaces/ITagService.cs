using System.Collections.Generic;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Services.Interfaces
{
    public interface ITagService : IService<Tag>
    {
        public List<Tag> Find(int id = -1, string name = null, string orderBy = null, bool reverse = false, int from = 0, int max = 10);
    }
}
