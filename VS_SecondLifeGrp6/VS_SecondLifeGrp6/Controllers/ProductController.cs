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

        public ProductController(IProductService service, IUserService serviceUser): base(serviceUser)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<Product>> List()
        {
            var res = _service.List();
            for (var i = 0; i < res.Count; i++) System.Console.WriteLine(res[i].ToString());
            if (res.Count == 0) return NoContent();
            return res;
        }

        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id)
        {
            var product = _service.Get(id);
            if (product == null) return BadRequest();
            return product;
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
            var contextUser = GetUserFromContext(HttpContext);
            if (contextUser.Id != p?.Owner?.Id) return Unauthorized();

            var res = _service.Add(p);
            if (res.Errors.Count > 0) return BadRequest(res);
            return res.Value;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPatch("{id}")]
        public ActionResult<Product> Patch(int id, [FromBody] JsonPatchDocument<Product> patchDoc)
        {
            if (patchDoc == null) return BadRequest(ModelState);

            var contextUser = GetUserFromContext(HttpContext);
            if (contextUser.Id != _service.Get(id)?.Owner.Id) return Unauthorized();
            
            var Product = _service.Patch(id, patchDoc);
            return Product;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete]
        public ActionResult<Product> Delete(int id)
        {
            var product = _service.Get(id);
            if (product == null) return BadRequest(string.Format(NOT_EXIST, nameof(Product)));

            var contextUser = GetUserFromContext(HttpContext);
            if (contextUser.Id != product.Owner.Id) return Unauthorized();

            _service.Remove(product);
            return Ok(product);
        }
    }
}
