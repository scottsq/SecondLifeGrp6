using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Repositories.Repositories
{
    public class UserRatingRepository : GenericRepository<UserRating>, IRepository<UserRating>
    {
        protected override List<string> _includes => new List<string> { nameof(UserRating.Origin) };

        public UserRatingRepository(VS_SLG6DbContext context) : base(context) { }

        public override UserRating FindOne(int id)
        {
            return base.FindOne(id);
        }
    }
}
