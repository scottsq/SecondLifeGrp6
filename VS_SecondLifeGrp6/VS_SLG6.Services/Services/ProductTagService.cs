using System.Collections.Generic;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Validators;

namespace VS_SLG6.Services.Services
{
    public class ProductTagService : GenericService<ProductTag>, IProductTagService
    {
        public ProductTagService(IRepository<ProductTag> repo, IValidator<ProductTag> validator) : base(repo, validator)
        {
        }

        public List<ProductTag> Find(int productId = -1, int from = 0, int max = 10)
        {
            return _repo.All(x => productId > -1 ? x.Product.Id == productId : true, from, max);
        }
    }
}
