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
        private IRatingService _service;

        public RatingController(IRatingService service)
        {
            _service = service;
        }

        #region GET
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

        [HttpGet("user/{id}")]
        public ActionResult<List<Rating>> GetUserRating(int id)
        {
            return _service.GetUserRatings(id);
        }
        [HttpGet("product/{id}")]
        public ActionResult<double> GetProductRating(int id)
        {
            return _service.GetProductRating(id);
        }
        #endregion


        #region POST
        [HttpPost]
        public ActionResult<Rating> Add(Rating r)
        {
            var res = _service.Add(r);
            if (res.Errors.Count > 0) return BadRequest(res);
            return res.Value;
        }
        #endregion


        #region PATCH
        [HttpPatch("{id}")]
        public ActionResult<Rating> Patch(int id, [FromBody] JsonPatchDocument<Rating> patchDoc)
        {
            if (patchDoc == null) return BadRequest(ModelState);
            var user = _service.Patch(id, patchDoc);
            return user;
        }
        #endregion


        #region DELETE
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
        #endregion
    }
}
