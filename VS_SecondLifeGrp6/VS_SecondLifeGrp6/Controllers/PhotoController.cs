using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Services;

namespace VS_SLG6.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class PhotoController : ControllerBase
    {
        private IService<Photo> _service;

        public PhotoController(IService<Photo> service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<Photo>> List()
        {
            var res = _service.List();
            if (res.Count == 0) return NoContent();
            return res;
        }

        [HttpGet("{id}")]
        public ActionResult<Photo> GetPhoto(int id)
        {
            var photo = _service.Get(id);
            if (photo == null) return BadRequest();
            return photo;
        }

        [HttpPost]
        public ActionResult<Photo> Add(Photo u)
        {
            var res = _service.Add(u);
            if (res.Errors.Count > 0) return BadRequest(res);
            return res.Value;
        }

        [HttpPatch("{id}")]
        public ActionResult<Photo> Patch(int id, [FromBody] JsonPatchDocument<Photo> patchDoc)
        {
            if (patchDoc == null) return BadRequest(ModelState);
            var photo = _service.Patch(id, patchDoc);
            return photo;
        }

        [HttpDelete]
        public ActionResult<Photo> Delete(int id)
        {
            var Product = _service.Get(id);
            if (Product != null)
            {
                _service.Remove(Product);
                return Ok(Product);
            }
            else return BadRequest("Invalid Product");

        }
    }
}
