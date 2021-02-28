using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Services;

namespace VS_SLG6.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private IService<Rating> _service;

        public RatingController(IService<Rating> service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<Rating>> List()
        {
            var res = _service.List();
            if (res.Count == 0) return NoContent();
            return res;
        }

        [HttpGet("{id}")]
        public ActionResult<Rating> GetRating(int id)
        {
            var rating = _service.Get(id);
            if (rating == null) return BadRequest();
            return rating;
        }

        [HttpPost]
        public ActionResult<Rating> Add(Rating r)
        {
            var res = _service.Add(r);
            if (res.Errors.Count > 0) return BadRequest(res);
            return res.Value;
        }

        [HttpPatch("{id}")]
        public ActionResult<Rating> Patch(int id, [FromBody] JsonPatchDocument<Rating> patchDoc)
        {
            if (patchDoc == null) return BadRequest(ModelState);
            var user = _service.Patch(id, patchDoc);
            return user;
        }

        [HttpDelete]
        public ActionResult<Rating> Delete(int id)
        {
            var rating = _service.Get(id);
            if (rating != null)
            {
                _service.Remove(rating);
                return Ok(rating);
            }
            else return BadRequest("Invalid rating");
            
        }
    }
}
