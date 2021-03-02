using System.Collections.Generic;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using System;
using VS_SLG6.Services.Models;
using VS_SLG6.Services.Validators;

namespace VS_SLG6.Services.Services
{
    public class UserService : GenericService<User>, IService<User>
    {
        public UserService(IRepository<User> repo, IValidator<User> validator) : base(repo, validator)
        {
        }
    }
}
