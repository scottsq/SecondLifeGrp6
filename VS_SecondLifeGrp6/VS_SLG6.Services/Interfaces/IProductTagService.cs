using System.Collections.Generic;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Services.Services
{
    public interface IProductTagService : IService<ProductTag>
    {
        public List<ProductTag> Find(int productId = -1, int from = 0, int max = 10);
    }
}
