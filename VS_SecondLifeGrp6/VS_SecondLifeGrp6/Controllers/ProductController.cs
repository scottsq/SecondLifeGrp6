using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Services;

namespace VS_SLG6.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private IProductService _service;

        public ProductController(IProductService service)
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

        [HttpPost]
        public ActionResult<Product> Add(Product u)
        {
            var res = _service.Add(u);
            if (res.Errors.Count > 0) return BadRequest(res);
            return res.Value;
        }

        [HttpPatch("{id}")]
        public ActionResult<Product> Patch(int id, [FromBody] JsonPatchDocument<Product> patchDoc)
        {
            if (patchDoc == null) return BadRequest(ModelState);
            var Product = _service.Patch(id, patchDoc);
            return Product;
        }

        [HttpDelete]
        public ActionResult<Product> Delete(int id)
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
