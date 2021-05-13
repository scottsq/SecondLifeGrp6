using System.Collections.Generic;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Services.Services
{
    public interface IProductService : IService<Product>
    {
        public List<Product> Find(int userId = -1, string[] keys = null, string orderBy = nameof(Product.CreationDate), bool reverse = true, int from = 0, int max = 10);
        public List<ProductWithPhoto> FindWithPhoto(int userId = -1, string[] keys = null, string orderBy = nameof(Product.CreationDate), bool reverse = true, int from = 0, int max = 10);
    }
}
