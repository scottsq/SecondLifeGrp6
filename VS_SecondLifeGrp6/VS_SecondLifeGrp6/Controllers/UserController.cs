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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController, Route("api/[controller]")]
    public class UserController : ControllerBaseExtended
    {
        private IUserService _service;

        public UserController(IUserService service): base(service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<User>> List()
        {
            var res = _service.List();
            if (res.Count == 0) return NoContent();
            return res;
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            var user = _service.Get(id);
            if (user == null) return BadRequest();
            return user;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult<User> Add(User u)
        {
            var res = _service.Add(u);
            if (res.Errors.Count > 0) return BadRequest(res);
            return res.Value;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<LoginResponse> Login(User u)
        {
            var res = _service.Login(u);
            if (res.Token == null) return BadRequest();
            return res;
        }

        [AllowAnonymous]
        [HttpPost("reset")]
        public ActionResult<string> Reset(string email)
        {
            return _service.ResetEmail(email);
        }

        [AllowAnonymous]
        [HttpPost("senduser")]
        public ActionResult<string> SendUser(string mail)
        {
            var res = _service.FindByMail(mail);
            if (res.Errors.Count > 0) return BadRequest(res.Errors);
            return res.Value.Login;
        }

        [HttpPatch("{id}")]
        public ActionResult<User> Patch(int id, [FromBody] JsonPatchDocument<User> patchDoc)
        {
            if (patchDoc == null) return BadRequest(ModelState);

            var contextUser = GetUserFromContext(HttpContext);
            if (contextUser.Id != id) return Unauthorized();

            var user = _service.Patch(id, patchDoc);
            return user;
        }

        [HttpDelete]
        public ActionResult<User> Delete(int id)
        {
            var user = _service.Get(id);
            if (user == null) return BadRequest(string.Format(NOT_EXIST, nameof(Model.Entities.User)));

            var contextUser = GetUserFromContext(HttpContext);
            if (contextUser.Id != id) return Unauthorized();
            
            _service.Remove(user);
            return Ok(user);            
        }
    }
}
