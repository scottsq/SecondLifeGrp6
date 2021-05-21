using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VS_SLG6.Api.Interfaces;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Interfaces;

namespace VS_SLG6.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController, Route("api/[controller]")]
    public class TagController : ControllerBaseExtended
    {
        private ITagService _service;
        private IControllerAccess<Tag> _controllerAccess;

        public TagController(ITagService service, IControllerAccess<Tag> controllerAccess)
        {
            _service = service;
            _controllerAccess = controllerAccess;
        }

        [AllowAnonymous]
        [HttpGet()]
        public ActionResult<List<Tag>> List(int id = -1, string name = null, string orderBy = null, bool reverse = false, int from = 0, int max = 10)
        {
            return _service.Find(id, name, orderBy, reverse, from, max);
        }

        [HttpPost]
        public ActionResult<Tag> Add(Tag tag)
        {
            if (!_controllerAccess.CanAdd(GetUserFromContext(HttpContext), tag)) return Unauthorized();
            return ReturnResult(_service.Add(tag));
        }

        [HttpPatch("{id}")]
        public ActionResult<Tag> Patch(int id, [FromBody] JsonPatchDocument<Tag> patchDoc)
        {
            var tag = _service.Get(id).Value;
            if (tag == null) return NoContent();
            if (!_controllerAccess.CanEdit(GetUserFromContext(HttpContext), tag)) return Unauthorized();
            if (patchDoc == null) return BadRequest(ModelState);
            return ReturnResult(_service.Patch(tag, patchDoc));
        }

        [HttpDelete("{id}")]
        public ActionResult<Tag> Delete(int id)
        {
            var tag = _service.Get(id).Value;
            if (tag == null) return NoContent();
            if (!_controllerAccess.CanEdit(GetUserFromContext(HttpContext), tag)) return Unauthorized();
            return ReturnResult(_service.Remove(tag));
        }
    }
}
