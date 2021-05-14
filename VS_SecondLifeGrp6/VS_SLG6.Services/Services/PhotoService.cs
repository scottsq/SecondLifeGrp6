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
    public class PhotoService : GenericService<Photo>, IPhotoService
    {
        public PhotoService(IRepository<Photo> repo, IValidator<Photo> validator): base(repo, validator) 
        {       
        }
        public List<Photo> Find(int id = -1, int productId = -1, string orderBy = null, bool reverse = false, int from = 0, int max = 10)
        {
            var list = _repo.All(
                GenerateCondition(id, productId),
                GenerateOrderByCondition(orderBy),
                reverse, from, max
            );
            return list;
        }

        public static Expression<Func<Photo, bool>> GenerateCondition(int id = -1, int productId = -1)
        {
            Expression<Func<Photo, bool>> condition = x => true;
            if (id > -1) condition.And(x => x.Id == id);
            if (productId > -1) condition.And(x => x.Product.Id == productId);
            return condition;
        }
    }
}
