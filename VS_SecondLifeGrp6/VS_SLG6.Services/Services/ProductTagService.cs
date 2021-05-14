using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Interfaces;
using VS_SLG6.Services.Validators;

namespace VS_SLG6.Services.Services
{
    public class ProductTagService : GenericService<ProductTag>, IProductTagService
    {
        public ProductTagService(IRepository<ProductTag> repo, IValidator<ProductTag> validator) : base(repo, validator)
        {
        }

        public List<ProductTag> Find(int id = -1, int productId = -1, string orderBy = null, bool reverse = false, int from = 0, int max = 10)
        {
            return _repo.All(
                GenerateCondition(id, productId),
                GenerateOrderByCondition(orderBy),
                reverse, from, max    
            );
        }

        public static Expression<Func<ProductTag, bool>> GenerateCondition(int id = -1, int productId = -1)
        {
            Expression<Func<ProductTag, bool>> condition = x => true;
            if (id > -1) condition.And(x => x.Id == id);
            if (productId > -1) condition.And(x => x.Product.Id == productId);
            return condition;
        }
    }
}
