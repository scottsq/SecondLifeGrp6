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

        public TagController(IService<Tag> service, IUserService serviceUser): base(serviceUser)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<Tag>> List()
        {
            var res = _service.List();
            if (res.Count == 0) return NoContent();
            return res;
        }

        [HttpGet("{id}")]
        public ActionResult<Tag> GetTag(int id)
        {
            var tag = _service.Get(id);
            if (tag == null) return BadRequest();
            return tag;
        }

        [HttpPost]
        public ActionResult<Tag> Add(Tag u)
        {
            var res = _service.Add(u);
            if (res.Errors.Count > 0) return BadRequest(res);
            return res.Value;
        }

        [HttpPatch("{id}")]
        public ActionResult<Tag> Patch(int id, [FromBody] JsonPatchDocument<Tag> patchDoc)
        {
            // Devrait être accessible seulement à un admin par exemple
            // Mais je n'ai pas mis en place de rôle donc par défaut on renvoie Unauthorized -> évolution possible
            return Unauthorized();

            if (patchDoc == null) return BadRequest(ModelState);
            var tag = _service.Patch(id, patchDoc);
            return tag;
        }

        [HttpDelete]
        public ActionResult<Tag> Delete(int id)
        {
            // Devrait être accessible seulement à un admin par exemple
            // Mais je n'ai pas mis en place de rôle donc par défaut on renvoie Unauthorized -> évolution possible
            return Unauthorized();

            var tag = _service.Get(id);
            if (tag != null)
            {
                _service.Remove(tag);
                return Ok(tag);
            }
            else return BadRequest("Invalid Product");

        }
    }
}
