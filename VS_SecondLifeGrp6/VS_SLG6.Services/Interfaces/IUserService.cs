using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Models;

namespace VS_SLG6.Services.Services
{
    public interface IUserService : IService<User>
    {
        public ValidationModel<User> FindByMail(string email);
        public string ResetEmail(string email);
        public int Login(User u);
    }
}
