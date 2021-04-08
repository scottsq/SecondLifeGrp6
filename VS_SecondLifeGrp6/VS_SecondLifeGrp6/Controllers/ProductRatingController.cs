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

        public ProductRatingController(IProductRatingService service)
        {
            _service = service;
        }

        #region GET
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<List<ProductRating>> List()
        {
            return _service.List().Value;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<ProductRating> GetRating(int id)
        {
            var rating = _service.Get(id);
            if (rating.Value == null) return BadRequest(string.Format(NOT_EXIST, nameof(ProductRating)));
            return rating.Value;
        }

        [AllowAnonymous]
        [HttpGet("{idProduct}/user/{idUser}")]
        public ActionResult<ProductRating> GetUserRating(int idProduct, int idUser)
        {
            var res = _service.GetUserRating(idProduct, idUser);
            if (res == null) return BadRequest(string.Format(NOT_EXIST, nameof(ProductRating)));
            return res;
        }
       
        [AllowAnonymous]
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
            _service.SetContextUser(GetUserFromContext(HttpContext));
            var res = _service.Add(r);
            return ReturnResult(res);
        }
        #endregion


        #region PATCH
        [HttpPatch("{id}")]
        public ActionResult<ProductRating> Patch(int id, [FromBody] JsonPatchDocument<ProductRating> patchDoc)
        {
            if (patchDoc == null) return BadRequest(ModelState);

            _service.SetContextUser(GetUserFromContext(HttpContext));
            var user = _service.Patch(id, patchDoc);
            return ReturnResult(user);
        }
        #endregion


        #region DELETE
        [HttpDelete("{id}")]
        public ActionResult<ProductRating> Delete(int id)
        {
            var rating = _service.Get(id);
            if (rating == null) return BadRequest(string.Format(NOT_EXIST, nameof(ProductRating)));

            _service.SetContextUser(GetUserFromContext(HttpContext));
            var check = _service.Remove(rating.Value);
            return ReturnResult(check);            
        }
        #endregion
    }
}
