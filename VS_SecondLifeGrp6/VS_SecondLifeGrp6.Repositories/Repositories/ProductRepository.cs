using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VS_SLG6.Model;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Repositories.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IRepository<Product>
    {
        protected override List<string> _includes => new List<string> { nameof(Product.Owner) };

        public ProductRepository(VS_SLG6DbContext context) : base(context) { }

        public override Product FindOne(int id)
        {
            return _contextWithIncludes.FirstOrDefault(x => x.Id == id);
        }
    }
}
