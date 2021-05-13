using System.Collections.Generic;
using VS_SLG6.Model;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Repositories.Repositories
{
    public class ProductRatingRepository : GenericRepository<ProductRating>, IRepository<ProductRating>
    {
        protected override List<string> _includes => new List<string> { nameof(ProductRating.Product), nameof(ProductRating.User) };

        public ProductRatingRepository(VS_SLG6DbContext context) : base(context) { }
    }
}
