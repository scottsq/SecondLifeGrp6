using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Services;

namespace VS_SLG6.Services.Models
{
    public class Product_ProductTag : IProduct_ProductTag
    {
        private IRepository<ProductTag> _repoProductTag;

        public Product_ProductTag(IRepository<ProductTag> repoProductTag)
        {
            _repoProductTag = repoProductTag;
        }

        public List<ProductTag> GetByProductId(int id)
        {
            var pt = new ProductTagService(_repoProductTag, null);
            return pt.GetByProductId(id);
        }

        public List<ProductTag> List()
        {
            var pt = new ProductTagService(_repoProductTag, null);
            return pt.List();
        }
    }
}
