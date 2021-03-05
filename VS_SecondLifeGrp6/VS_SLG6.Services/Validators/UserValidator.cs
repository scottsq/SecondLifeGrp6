using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;

namespace VS_SLG6.Services.Validators
{
    public class UserValidator : IValidator<User>
    {
        readonly IRepository<User> _repo;

        public UserValidator(IRepository<User> repo)
        {
            _repo = repo;
        }

        public bool canAdd(User obj)
        {
            if (obj == null) return false;
            return _repo.FindAll(x => x.Login == obj.Login).Count == 0;
        }

        public bool canDelete(User obj)
        {
            throw new NotImplementedException();
        }

        public bool canEdit(User obj)
        {
            throw new NotImplementedException();
        }
    }
}
