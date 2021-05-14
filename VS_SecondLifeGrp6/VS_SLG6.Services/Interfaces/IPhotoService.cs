using System.Collections.Generic;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Services.Interfaces
{
    public interface IPhotoService : IService<Photo>
    {
        public List<Photo> Find(int id = -1, int productId = -1, string orderBy = null, bool reverse = false, int from = 0, int max = 10);
    }
}
