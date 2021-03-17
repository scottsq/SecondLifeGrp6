using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VS_SLG6.Model;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Repositories.Repositories
{
    public class ProductTagRepository : GenericRepository<ProductTag>, IRepository<ProductTag>
    {
        protected override List<string> _includes => new List<string> { nameof(ProductTag.Product), nameof(ProductTag.Tag) };

        public ProductTagRepository(VS_SLG6DbContext context) : base(context) { }

        public override ProductTag FindOne(int id)
        {
            return _contextWithIncludes.FirstOrDefault(x => x.Id == id);
        }
    }
}
