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
    [ApiController, Route("api/[controller]")]
    public class ProductController : ControllerBaseExtended
    {
        private IProductService _service;
        private IControllerAccess<Product> _controllerAccess;

        public ProductController(IProductService service, IControllerAccess<Product> controllerAccess)
        {
            _service = service;
            _controllerAccess = controllerAccess;
        }

        [HttpGet()]
        public ActionResult<List<Product>> List(int id = -1, int userId = -1, string keys = null, string date = null, string orderBy = null, bool reverse = false, int from = 0, int max = 10)
        {
            return _service.Find(id, userId, keys, date, orderBy, reverse, from, max);
        }

        [HttpGet("withphotos")]
        public ActionResult<List<ProductWithPhoto>> ListWithPhotos(int id = -1, int userId = -1, string keys = null, string date = null, string orderBy = null, bool reverse = false, int from = 0, int max = 10)
        {
            return _service.FindWithPhoto(id, userId, keys, date, orderBy, reverse, from, max);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public ActionResult<Product> Add(Product p)
        {
            if (!_controllerAccess.CanAdd(GetUserFromContext(HttpContext), p)) return Unauthorized();
            return ReturnResult(_service.Add(p));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPatch("{id}")]
        public ActionResult<Product> Patch(int id, [FromBody] JsonPatchDocument<Product> patchDoc)
        {
            var product = _service.Get(id).Value;
            if (product == null) return NoContent();
            if (!_controllerAccess.CanEdit(GetUserFromContext(HttpContext), product)) return Unauthorized();
            return ReturnResult(_service.Patch(product, patchDoc));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public ActionResult<Product> Delete(int id)
        {
            var product = _service.Get(id).Value;
            if (product == null) return Unauthorized();
            if (!_controllerAccess.CanDelete(GetUserFromContext(HttpContext), product)) return Unauthorized();
            return ReturnResult(_service.Remove(product));
        }
    }
}
