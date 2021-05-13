using System.Collections.Generic;
using VS_SLG6.Model;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Repositories.Repositories
{
    public class UserRepository : GenericRepository<User>, IRepository<User>
    {
        protected override List<string> _includes => new List<string> { nameof(User.Role) };

        public UserRepository(VS_SLG6DbContext context) : base(context) { }
    }
}
