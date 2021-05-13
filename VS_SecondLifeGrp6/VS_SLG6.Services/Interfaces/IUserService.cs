using System.Collections.Generic;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Services.Services
{
    public interface IUserService : IService<User>
    {
        public List<User> Find(int id=-1, string email=null, string login=null, string name=null, string orderBy = nameof(User.Name), bool reverse = false, int from = 0, int max = 10);
        public LoginResponse Login(User u);
    }
}
