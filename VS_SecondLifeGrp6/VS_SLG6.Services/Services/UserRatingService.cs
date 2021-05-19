using System.Collections.Generic;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Validators;
using System.Linq;
using System.Linq.Expressions;
using System;
using LinqKit;
using VS_SLG6.Services.Interfaces;

namespace VS_SLG6.Services.Services
{
    public class UserRatingService : GenericService<UserRating>, IUserRatingService
    {
        public UserRatingService(IRepository<UserRating> repo, IValidator<UserRating> validator) : base(repo, validator)
        {
        }

        public double GetAverageRating(int id)
        {
            var res = _repo.All(x => x.Target.Id == id);
            if (res.Count == 0) return 0;
            return res.Average(x => x.Stars);
        }

        public List<UserRating> Find(int id = -1, int idOrigin = -1, int idTarget = -1, string orderBy = null, bool reverse = false, int from = 0, int max = 10)
        {
            var list = _repo.All(
                GenerateCondition(id, idOrigin, idTarget),
                GenerateOrderByCondition(orderBy),
                reverse, from, max
            );
            return list;
        }

        public static Expression<Func<UserRating, bool>> GenerateCondition(int id = -1, int idOrigin = -1, int idTarget = -1)
        {
            Expression<Func<UserRating, bool>> condition = x => true;
            if (id > -1) condition = condition.And(x => x.Id == id);
            if (idOrigin > -1) condition = condition.And(x => x.Origin.Id == idOrigin);
            if (idTarget > -1) condition = condition.And(x => x.Target.Id == idTarget);
            return condition;
        }
    }
}
