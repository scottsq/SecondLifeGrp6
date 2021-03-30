using System.Collections.Generic;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using System;
using VS_SLG6.Services.Models;
using VS_SLG6.Services.Validators;
using System.Linq;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using VS_SLG6.Api;

namespace VS_SLG6.Services.Services
{
    public class UserService : GenericService<User>, IUserService
    {
        public readonly AppSettings _appsettings;

        public UserService(IRepository<User> repo, IValidator<User> validator, IOptions<AppSettings> appsettings) : base(repo, validator)
        {
            _appsettings = appsettings.Value;
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

        public LoginResponse Login(User u)
        {
            var res = _repo.All(user => u.Login == user.Login && u.Password == user.Password);
            var loginResponse = new LoginResponse();
            loginResponse.Id = -1;

            if (res.Count > 0)
            {
                loginResponse.Id = res[0].Id;

                var tokenhandler = new JwtSecurityTokenHandler();
                var keys = Encoding.ASCII.GetBytes(_appsettings.Key);
                var tokendescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name,u.Login),
                        new Claim(ClaimTypes.Role,"user"),
                        new Claim(ClaimTypes.Version,"v2.1")
                    }),
                    Expires = DateTime.UtcNow.AddHours(3),
                    SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(keys), Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)

                };
                var tokens = tokenhandler.CreateToken(tokendescriptor);

                loginResponse.Token = tokenhandler.WriteToken(tokens);

                /*
                IAuthContainerModel model = new JWTContainerModel()
                {
                    Claims = new Claim[] {
                        new Claim(ClaimTypes.Name, u.Login)
                    }
                };
                IAuthService authService = new JWTService(model.SecretKey);
                string token = authService.GenerateToken(model);
                if (!authService.isTokenValid(token)) loginResponse.Token = null;
                else loginResponse.Token = token;*/
            }
            return loginResponse;
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
