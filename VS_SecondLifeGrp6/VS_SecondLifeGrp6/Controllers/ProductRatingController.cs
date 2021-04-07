using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VS_SLG6.Api.Controllers;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Services;

namespace VS_SLG6.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController, Route("api/[controller]")]
    public class ProductRatingController : ControllerBaseExtended
    {
        private IProductRatingService _service;

        public ProductRatingController(IProductRatingService service, IUserService serviceUser): base(serviceUser)
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

        [HttpGet("{idProduct}/user/{idUser}")]
        public ActionResult<ProductRating> GetUserRating(int idProduct, int idUser)
        {
            return _service.GetUserRating(idProduct, idUser);
        }
       
        [HttpGet("product/{id}")]
        public ActionResult<List<ProductRating>> GetProductRatings(int id)
        {
            return _service.GetRatings(id);
        }

        [AllowAnonymous]
        [HttpGet("product/{id}/average")]
        public ActionResult<double> GetAverageProductRating(int id)
        {
            return _service.GetAverageRating(id);
        }
        #endregion


        #region POST
        [HttpPost]
        public ActionResult<ProductRating> Add(ProductRating r)
        {
            var contextUser = GetUserFromContext(HttpContext);
            if (contextUser.Id != r?.User?.Id) return Unauthorized();

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

            var contextUser = GetUserFromContext(HttpContext);
            if (contextUser.Id != _service.Get(id)?.User.Id) return Unauthorized();

            var user = _service.Patch(id, patchDoc);
            return user;
        }
        #endregion


        #region DELETE
        [HttpDelete]
        public ActionResult<ProductRating> Delete(int id)
        {
            var rating = _service.Get(id);
            if (rating == null) return BadRequest(string.Format(NOT_EXIST, nameof(ProductRating)));

            var contextUser = GetUserFromContext(HttpContext);
            if (contextUser.Id != rating.User.Id) return Unauthorized();

            _service.Remove(rating);
            return Ok(rating);
            
        }
        #endregion
    }
}
