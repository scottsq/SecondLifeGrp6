using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Services;

namespace VS_SLG6.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController, Route("api/[controller]")]
    public class TagController : ControllerBaseExtended
    {
        private IService<Tag> _service;

        public TagController(IService<Tag> service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<List<Tag>> List()
        {
            return _service.List().Value;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<Tag> GetTag(int id)
        {
            var tag = _service.Get(id);
            if (tag.Value == null) return BadRequest(string.Format(NOT_EXIST, nameof(Tag)));
            return tag.Value;
        }

        [HttpPost]
        public ActionResult<Tag> Add(Tag u)
        {
            var res = _service.Add(u);
            if (res.Errors.Count > 0) return BadRequest(res.Errors);
            return res.Value;
        }

        [HttpPatch("{id}")]
        public ActionResult<Tag> Patch(int id, [FromBody] JsonPatchDocument<Tag> patchDoc)
        {
            if (patchDoc == null) return BadRequest(ModelState);

            _service.SetContextUser(GetUserFromContext(HttpContext));
            var tag = _service.Patch(id, patchDoc);
            return ReturnResult(tag);
        }

        [HttpDelete("{id}")]
        public ActionResult<Tag> Delete(int id)
        {
            _service.SetContextUser(GetUserFromContext(HttpContext));

            var tag = _service.Get(id);
            if (tag.Value == null) return BadRequest(string.Format(NOT_EXIST, nameof(Tag)));
            var check = _service.Remove(tag.Value);
            return ReturnResult(check);
        }
    }
}
