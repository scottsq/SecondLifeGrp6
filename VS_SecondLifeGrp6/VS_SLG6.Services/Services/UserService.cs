using System.Collections.Generic;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using System;
using VS_SLG6.Services.Validators;
using System.Linq;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using VS_SLG6.Api;
using VS_SLG6.Services.Models;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;
using LinqKit;

namespace VS_SLG6.Services.Services
{
    public class UserService : GenericService<User>, IUserService
    {
        public readonly AppSettings _appsettings;

        public UserService(IRepository<User> repo, IValidator<User> validator, IOptions<AppSettings> appsettings) : base(repo, validator)
        {
            _appsettings = appsettings.Value;
        }

        public List<User> Find(int id = -1, string email = null, string login = null, string name = null, string orderBy = nameof(User.Name), bool reverse = false, int from = 0, int max = 10)
        {
            var list = _repo.All(GenerateCondition(id, email, login, name), from, max);
            if (orderBy == nameof(User.Name))
            {
                if (reverse) list = list.OrderByDescending(x => x.Name).ToList();
                else list = list.OrderBy(x => x.Name).ToList();
            }
            return list;
        }

        public static Expression<Func<User, bool>> GenerateCondition(int id = -1, string email = null, string login = null, string name = null)
        {
            Expression<Func<User, bool>> condition = x => true;
            if (id > -1) condition.And(x => x.Id == id);
            if (email != null) condition.And(x => x.Email == email);
            if (login != null) condition.And(x => x.Login == login);
            if (name != null) condition.And(x => x.Name == name);
            return condition;
        }

        public LoginResponse Login(User u)
        {
            var user = _repo.FindOne(x => u.Login == x.Login);

            if (user != null && PasswordManager.GetStringSha256Hash(u.Password) == user.Password)
            {
                var loginResponse = new LoginResponse();
                loginResponse.Id = user.Id;

                var tokenhandler = new JwtSecurityTokenHandler();
                var keys = Encoding.ASCII.GetBytes(_appsettings.Key);
                var tokendescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("user_id", user.Id.ToString()),
                        new Claim("user_role", user.Role.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(60),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keys), SecurityAlgorithms.HmacSha256Signature)

                };
                var tokens = tokenhandler.CreateToken(tokendescriptor);

                loginResponse.Token = tokenhandler.WriteToken(tokens);
                return loginResponse;
            }
            return null;
        }
    }
}
