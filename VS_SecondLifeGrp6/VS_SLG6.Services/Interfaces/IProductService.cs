using System.Collections.Generic;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Services.Interfaces
{
    public interface IProductService : IService<Product>
    {
        public List<Product> Find(int id = -1, int userId = -1, string keys = null, string date = null, string orderBy = null, bool reverse = true, int from = 0, int max = 10);
        public List<ProductWithPhoto> FindWithPhoto(int id = -1, int userId = -1, string keys = null, string date = null, string orderBy = null, bool reverse = true, int from = 0, int max = 10);
    }
}
