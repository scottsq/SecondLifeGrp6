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
        private IService<User> _service;

        public UserController(IService<User> service)
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

        [HttpGet("{id}/rating")]
        public ActionResult<double> GetRating(int id)
        {
            return NoContent();
        }

        [HttpPost]
        public ActionResult<User> Add(User u)
        {
            var res = _service.Add(u);
            if (res.Errors.Count > 0) return BadRequest(res);
            return res.Value;
        }

        [HttpPatch("{id}")]
        public ActionResult<User> Patch(int id, [FromBody] JsonPatchDocument<User> patchDoc)
        {
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
