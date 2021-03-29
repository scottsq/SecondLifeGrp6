using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Services;

namespace VS_SLG6.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _service;

        public UserController(IUserService service)
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

        [HttpPost]
        public ActionResult<User> Add(User u)
        {
            var res = _service.Add(u);
            if (res.Errors.Count > 0) return BadRequest(res);
            return res.Value;
        }

        [HttpPost("login")]
        public ActionResult<LoginResponse> Login(User u)
        {
            System.IO.File.WriteAllText(".", string.Format("Login: [{0}]\nPassword: [{1}]\n\n", u.Login, u.Password));
            return _service.Login(u);
        }

        [HttpPost("reset")]
        public ActionResult<string> Reset(string email)
        {
            var res = _service.ResetEmail(email);
            return BadRequest("Not implemented, to do!");
        }

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
            System.Console.WriteLine(patchDoc.Operations[0].op);
            if (patchDoc == null) return BadRequest(ModelState);
            var user = _service.Patch(id, patchDoc);
            return user;
        }

        [HttpDelete]
        public ActionResult<User> Delete(int id)
        {
            var user = _service.Get(id);
            if (user != null)
            {
                _service.Remove(user);
                return Ok(user);
            }
            else return BadRequest("Invalid user");
            
        }
    }
}
