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

        public Models.ValidationModel<User> FindByMail(string email)
        {
            var user = _repo.All(x => x.Email == email).FirstOrDefault();
            if (user == null)
            {
                _validationModel.Value = null;
                _validationModel.Errors.Add("Email not bound to any user");
            }
            else _validationModel.Value = user;
            return _validationModel;
        }

        public string ResetEmail(string email)
        {
            if (FindByMail(email).Value != null) return GenerateCode();
            else return null;
        }

        private string GenerateCode()
        {
            var length = 6;
            var code = "";
            var charArray = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
            var r = new Random();
            for (int i = 0; i < length; i++)
            {
                code += charArray[r.Next(0, charArray.Length)];
            }
            return code;
        }
    }
}
