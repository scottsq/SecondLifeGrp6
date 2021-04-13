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

namespace VS_SLG6.Services.Services
{
    public class UserService : GenericService<User>, IUserService
    {
        public readonly AppSettings _appsettings;

        public UserService(IRepository<User> repo, IValidator<User> validator, IOptions<AppSettings> appsettings) : base(repo, validator)
        {
            _appsettings = appsettings.Value;
        }

        public ValidationModel<User> FindByMail(string email)
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

        public LoginResponse Login(User u)
        {
            var user = _repo.FindOne(x => u.Login == x.Login);
            var accessManager = new AccessManager();

            if (user != null && accessManager.AreHashEqual(accessManager.GetStringSha256Hash(u.Password), user.Password))
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
