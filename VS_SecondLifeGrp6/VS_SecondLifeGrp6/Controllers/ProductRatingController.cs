using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VS_SLG6.Api.Controllers;
using VS_SLG6.Api.Interfaces;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Services;

namespace VS_SLG6.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController, Route("api/[controller]")]
    public class ProductRatingController : ControllerBaseExtended
    {
        private IProductRatingService _service;
        private IControllerAccess<ProductRating> _controllerAccess;

        public ProductRatingController(IProductRatingService service, IControllerAccess<ProductRating> controllerAccess)
        {
            _service = service;
            _controllerAccess = controllerAccess;
        }

        [AllowAnonymous]
        [HttpGet("?id={id}&idProduct={idProduct}&idUser={idUser}&orderBy={orderBy}&reverse={reverse}&from={from}&max={max}")]
        public ActionResult<List<ProductRating>> List(int id = -1, int idProduct = -1, int idUser = -1, string orderBy = null, bool reverse = false, int from = 0, int max = 10)
        {
            return _service.Find(id, idProduct, idUser, orderBy, reverse, from, max);
        }

        [AllowAnonymous]
        [HttpGet("average?id={id}")]
        public ActionResult<double> GetAverageProductRating(int id = -1)
        {
            return _service.GetAverageRating(id);
        }

        [HttpPost]
        public ActionResult<ProductRating> Add(ProductRating r)
        {
            if (!_controllerAccess.CanAdd(GetUserFromContext(HttpContext), r)) return Unauthorized();
            return ReturnResult(_service.Add(r));
        }

        [HttpPatch("{id}")]
        public ActionResult<ProductRating> Patch(int id, [FromBody] JsonPatchDocument<ProductRating> patchDoc)
        {
            var productRating = _service.Get(id).Value;
            if (productRating == null) return NoContent();
            if (!_controllerAccess.CanEdit(GetUserFromContext(HttpContext), productRating)) return Unauthorized();
            if (patchDoc == null) return BadRequest(ModelState);
            return ReturnResult(_service.Patch(productRating, patchDoc));
        }

        [HttpDelete("{id}")]
        public ActionResult<ProductRating> Delete(int id)
        {
            var productRating = _service.Get(id).Value;
            if (productRating == null) return NoContent();
            if (!_controllerAccess.CanDelete(GetUserFromContext(HttpContext), productRating)) return Unauthorized();
            return ReturnResult(_service.Remove(productRating));            
        }
    }
}
