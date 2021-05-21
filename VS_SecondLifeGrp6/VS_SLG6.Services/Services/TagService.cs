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
    public class TagService: GenericService<Tag>, ITagService
    {
        public TagService(IRepository<Tag> repo, IValidator<Tag> validator): base(repo, validator) { }

        public List<Tag> Find(int id = -1, string name = null, string orderBy = null, bool reverse = false, int from = 0, int max = 10)
        {
            return _repo.All(
                GenerateCondition(id, name),
                GenerateOrderByCondition(orderBy),
                reverse, from, max
            );
        }

        public static Expression<Func<Tag, bool>> GenerateCondition(int id = -1, string name = null)
        {
            Expression<Func<Tag, bool>> condition = x => true;
            if (id > -1) condition = condition.And(x => x.Id == id);
            if (name != null) condition = condition.And(x => x.Name.Contains(name));
            return condition;
        }
    }
}
