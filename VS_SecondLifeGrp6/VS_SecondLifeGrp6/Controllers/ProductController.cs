using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Services;

namespace VS_SLG6.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class ProductController : ControllerBaseExtended
    {
        private IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<Product>> List()
        {
            return _service.List().Value;
        }

        [HttpGet("user/{id}/withphoto")]
        public ActionResult<List<ProductWithPhoto>> ListForUserWithPhoto(int id)
        {
            return _service.GetProductForUserWithPhotos(id);
        }

        [HttpGet("withphoto")]
        public ActionResult<List<ProductWithPhoto>> ListWithPhoto()
        {
            return _service.GetProductWithPhotos();
        }

        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id)
        {
            var product = _service.Get(id);
            if (product.Value == null) return BadRequest(string.Format(NOT_EXIST, nameof(Product)));
            return product.Value;
        }

        [HttpGet("latest")]
        public ActionResult<List<Product>> GetLatestProducts()
        {
            var list = _service.GetLatest();
            if (list.Count == 0) return NoContent();
            return list;
        }

        [HttpGet("search/{id}")]
        public ActionResult<List<Product>> GetProductsByUser(int id)
        {
            return _service.GetUserProducts(id);
        }

        [HttpGet("user/search/keys/{keys}")]
        public ActionResult<List<Product>> GetProductsByTags(string[] keys)
        {
            return _service.GetProductsByKeys(keys);
        }

        [HttpGet("user/{id}/like")]
        public ActionResult<List<Product>> GetProductsByInterest(int id)
        {
            return _service.GetProductsByInterest(id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public ActionResult<Product> Add(Product p)
        {
            _service.SetContextUser(GetUserFromContext(HttpContext));
            var res = _service.Add(p);
            return ReturnResult(res);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPatch("{id}")]
        public ActionResult<Product> Patch(int id, [FromBody] JsonPatchDocument<Product> patchDoc)
        {
            if (patchDoc == null) return BadRequest(ModelState);
            _service.SetContextUser(GetUserFromContext(HttpContext));
            var product = _service.Patch(id, patchDoc);
            return ReturnResult(product);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public ActionResult<Product> Delete(int id)
        {
            _service.SetContextUser(GetUserFromContext(HttpContext));
            var product = _service.Get(id);
            if (product.Errors.Count > 0) return Unauthorized(product.Errors);
            if (product.Value == null) return BadRequest(string.Format(NOT_EXIST, nameof(Product)));

            var check = _service.Remove(product.Value);
            return ReturnResult(check);
        }
    }
}
