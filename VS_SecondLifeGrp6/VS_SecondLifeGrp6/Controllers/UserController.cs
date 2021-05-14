using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VS_SLG6.Api.Controllers;
using VS_SLG6.Api.Interfaces;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Interfaces;

namespace VS_SLG6.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController, Route("api/[controller]")]
    public class UserController : ControllerBaseExtended
    {
        private IUserService _service;
        private IUserControllerAccess _controllerAccess;

        public UserController(IUserService service, IUserControllerAccess controllerAccess)
        {
            _service = service;
            _controllerAccess = controllerAccess;
        }

        [HttpGet("?id={id}&email={email}&name={name}&orderBy={orderBy}&reverse={reverse}&from={from}&max={max}")]
        public ActionResult<List<User>> List(int id = -1, bool detailed = false, string email = null, string name = null, string orderBy = null, bool reverse = false, int from = 0, int max = 10)
        {
            if (!_controllerAccess.CanGet(GetUserFromContext(HttpContext), id))
            {
                    return _service.FindAndReduce(id, email, null, name, orderBy, reverse, from, max);
            }
            return _service.Find(id, email, null, name, orderBy, reverse, from, max);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult<User> Add(User u)
        {
            return ReturnResult(_service.Add(u));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<LoginResponse> Login(User u)
        {
            var res = _service.Login(u);
            if (res == null) return BadRequest();
            return res;
        }

        [HttpPatch("{id}")]
        public ActionResult<User> Patch(int id, [FromBody] JsonPatchDocument<User> patchDoc)
        {
            var user = _service.Get(id).Value;
            if (user == null) return NoContent();
            if (!_controllerAccess.CanEdit(GetUserFromContext(HttpContext), user)) return Unauthorized();
            if (patchDoc == null) return BadRequest(ModelState);
            return ReturnResult(_service.Patch(user, patchDoc));
        }

        [HttpDelete("{id}")]
        public ActionResult<User> Delete(int id)
        {
            var user = _service.Get(id).Value;
            if (user == null) return NoContent();
            if (!_controllerAccess.CanEdit(GetUserFromContext(HttpContext), user)) return Unauthorized();
            return ReturnResult(_service.Remove(user));
        }
    }
}
