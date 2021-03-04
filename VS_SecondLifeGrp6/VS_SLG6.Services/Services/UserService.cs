using System.Collections.Generic;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using System;
using VS_SLG6.Services.Models;
using VS_SLG6.Services.Validators;
using System.Linq;

namespace VS_SLG6.Services.Services
{
    public class UserService : GenericService<User>, IUserService
    {
        public UserService(IRepository<User> repo, IValidator<User> validator) : base(repo, validator)
        {
        }

        public ValidationModel<User> FindByMail(string email)
        {
            var user = _repo.FindAll(x => x.Email == email).FirstOrDefault();
            if (user == null)
            {
                _validationModel.Value = null;
                _validationModel.Errors.Add("Email not bound to any user");
            }
            else _validationModel.Value = user;
            return _validationModel;
        }
    }
}
