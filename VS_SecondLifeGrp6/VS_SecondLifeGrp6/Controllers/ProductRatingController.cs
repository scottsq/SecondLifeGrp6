using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Services;

namespace VS_SLG6.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class ProductRatingController : ControllerBase
    {
        private IProductRatingService _service;

        public ProductRatingController(IProductRatingService service)
        {
            _service = service;
        }

        #region GET
        [HttpGet]
        public ActionResult<List<ProductRating>> List()
        {
            var res = _service.List();
            if (res.Count == 0) return NoContent();
            return res;
        }

        [HttpGet("{id}")]
        public ActionResult<ProductRating> GetRating(int id)
        {
            var rating = _service.Get(id);
            if (rating == null) return BadRequest();
            return rating;
        }

        [HttpGet("user/{id}")]
        public ActionResult<List<ProductRating>> GetUserRating(int id)
        {
            return _service.GetUserRatings(id);
        }
       
        [HttpGet("product/{id}")]
        public ActionResult<List<ProductRating>> GetProductRatings(int id)
        {
            return _service.GetRatings(id);
        }

        [HttpGet("product/{id}/average")]
        public ActionResult<double> GetAverageProductRating(int id)
        {
            return _service.GetProductRating(id);
        }
        #endregion


        #region POST
        [HttpPost]
        public ActionResult<ProductRating> Add(ProductRating r)
        {
            var res = _service.Add(r);
            if (res.Errors.Count > 0) return BadRequest(res);
            return res.Value;
        }
        #endregion


        #region PATCH
        [HttpPatch("{id}")]
        public ActionResult<ProductRating> Patch(int id, [FromBody] JsonPatchDocument<ProductRating> patchDoc)
        {
            if (patchDoc == null) return BadRequest(ModelState);
            var user = _service.Patch(id, patchDoc);
            return user;
        }
        #endregion


        #region DELETE
        [HttpDelete]
        public ActionResult<ProductRating> Delete(int id)
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
