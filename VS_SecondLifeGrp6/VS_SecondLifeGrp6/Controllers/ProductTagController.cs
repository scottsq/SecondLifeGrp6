using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Services;

namespace VS_SLG6.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class ProductTagController : ControllerBase
    {
        private IService<ProductTag> _service;

        public ProductTagController(IService<ProductTag> service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<ProductTag>> List()
        {
            var res = _service.List();
            if (res.Count == 0) return NoContent();
            return res;
        }

        [HttpGet("{id}")]
        public ActionResult<ProductTag> GetProductTag(int id)
        {
            var productTag = _service.Get(id);
            if (productTag == null) return BadRequest();
            return productTag;
        }

        [HttpPost]
        public ActionResult<ProductTag> Add(ProductTag u)
        {
            var res = _service.Add(u);
            if (res.Errors.Count > 0) return BadRequest(res);
            return res.Value;
        }

        [HttpPatch("{id}")]
        public ActionResult<ProductTag> Patch(int id, [FromBody] JsonPatchDocument<ProductTag> patchDoc)
        {
            if (patchDoc == null) return BadRequest(ModelState);
            var productTag = _service.Patch(id, patchDoc);
            return productTag;
        }

        [HttpDelete]
        public ActionResult<ProductTag> Delete(int id)
        {
            var productTag = _service.Get(id);
            if (productTag != null)
            {
                _service.Remove(productTag);
                return Ok(productTag);
            }
            else return BadRequest("Invalid Product");

        }
    }
}
