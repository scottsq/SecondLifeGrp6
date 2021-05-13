using System.Collections.Generic;
using VS_SLG6.Model;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Repositories.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IRepository<Product>
    {
        protected override List<string> _includes => new List<string> { nameof(Product.Owner) };

        public ProductRepository(VS_SLG6DbContext context) : base(context) { }
    }
}
