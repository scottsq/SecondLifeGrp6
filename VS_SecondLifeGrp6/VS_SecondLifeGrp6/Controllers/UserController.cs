using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using VS_SLG6.Api;
using VS_SLG6.Api.Controllers;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Models;
using VS_SLG6.Services.Services;

namespace VS_SLG6.Controllers
{
    [Authorize()]
    [ApiController, Route("api/[controller]")]
    public class UserController : ControllerBaseExtended
    {
        private IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<User>> List()
        {
            return _service.List().Value;
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            var user = _service.Get(id);
            if (user.Value == null) return BadRequest(string.Format(NOT_EXIST, nameof(Model.Entities.User)));
            return user.Value;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult<User> Add(User u)
        {
            var res = _service.Add(u);
            if (res.Errors.Count > 0) return BadRequest(res.Errors);
            return res.Value;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<LoginResponse> Login(User u)
        {
            var res = _service.Login(u);
            if (res == null) return BadRequest();
            return res;
        }

        [AllowAnonymous]
        [HttpPost("reset")]
        public ActionResult<string> Reset(string email)
        {
            // Euuuh spa une faille de sécurité IMMENSE ça ?
            // Faudrait renvoyer un hash et envoyer le mail via l'api en vrai j'pense
            return _service.ResetEmail(email);
        }

        [AllowAnonymous]
        [HttpPost("senduser")]
        public ActionResult<string> SendUser(string mail)
        {
            // Je sais pas si on est censé s'en servir de cette route, j'crois pas en tout cas
            var res = _service.FindByMail(mail);
            if (res.Errors.Count > 0) return BadRequest(res.Errors);
            return res.Value.Login;
        }

        [HttpPatch("{id}")]
        public ActionResult<User> Patch(int id, [FromBody] JsonPatchDocument<User> patchDoc)
        {
            if (patchDoc == null) return BadRequest(ModelState);
            _service.SetContextUser(GetUserFromContext(HttpContext));
            var user = _service.Patch(id, patchDoc);
            return ReturnResult(user);
        }

        [HttpDelete("{id}")]
        public ActionResult<User> Delete(int id)
        {
            _service.SetContextUser(GetUserFromContext(HttpContext));
            var user = _service.Get(id);
            if (user == null) return BadRequest(string.Format(NOT_EXIST, nameof(Model.Entities.User)));
            var check = _service.Remove(user.Value);
            return ReturnResult(check);       
        }
    }
}
