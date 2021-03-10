using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Services.Services
{
    public interface IProductTagService : IService<ProductTag>
    {
        public List<ProductTag> GetByProductId(int id);
    }
}
