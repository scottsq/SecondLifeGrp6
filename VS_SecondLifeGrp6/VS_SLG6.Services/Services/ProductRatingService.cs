using System.Collections.Generic;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using System;
using VS_SLG6.Services.Validators;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;
using VS_SLG6.Services.Interfaces;

namespace VS_SLG6.Services.Services
{
    public class ProductRatingService : GenericService<ProductRating>, IProductRatingService
    {
        public ProductRatingService(IRepository<ProductRating> repo, IValidator<ProductRating> validator) : base(repo, validator)
        {
        }

        public double GetAverageRating(int id)
        {
            var list = _repo.All(x => x.Product.Id == id);
            if (list.Count == 0) return 0;
            return list.Average(x => x.Stars);
        }

        public List<ProductRating> Find(int id = -1, int idProduct = -1, int idUser = -1, string orderBy = null, bool reverse = false, int from = 0, int max = 10)
        {
            return _repo.All(
                GenerateCondition(id, idProduct, idUser),
                GenerateOrderByCondition(orderBy),
                reverse, from, max
            );
        }

        public static Expression<Func<ProductRating, bool>> GenerateCondition(int id = -1, int idProduct = -1, int idUser = -1)
        {
            Expression<Func<ProductRating, bool>> condition = x => true;
            if (id > -1) condition.And(x => x.Id == id);
            if (idProduct > -1) condition.And(x => x.Product.Id == idProduct);
            if (idUser > -1) condition.And(x => x.User.Id == idUser);
            return condition;
        }
    }
}
