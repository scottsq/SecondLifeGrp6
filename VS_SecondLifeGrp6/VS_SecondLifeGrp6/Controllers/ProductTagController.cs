using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Services;

namespace VS_SLG6.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController, Route("api/[controller]")]
    public class ProductTagController : ControllerBaseExtended
    {
        private IService<ProductTag> _service;

        public ProductTagController(IService<ProductTag> service, IUserService serviceUser): base(serviceUser)
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

        [AllowAnonymous]
        [HttpGet("product/{id}")]
        public ActionResult<List<ProductTag>> GetProductTags(int id)
        {
            return ((ProductTagService)_service).GetByProductId(id);
        }

        [HttpPost]
        public ActionResult<ProductTag> Add(ProductTag productTag)
        {
            var contextUser = GetUserFromContext(HttpContext);
            if (contextUser.Id != productTag.Product.Owner.Id) return Unauthorized();

            var res = _service.Add(productTag);
            if (res.Errors.Count > 0) return BadRequest(res);
            return res.Value;
        }

        [HttpPatch("{id}")]
        public ActionResult<ProductTag> Patch(int id, [FromBody] JsonPatchDocument<ProductTag> patchDoc)
        {
            if (patchDoc == null) return BadRequest(ModelState);

            var contextUser = GetUserFromContext(HttpContext);
            if (contextUser.Id != _service.Get(id)?.Product.Owner.Id) return Unauthorized();

            var productTag = _service.Patch(id, patchDoc);
            return productTag;
        }

        [HttpDelete]
        public ActionResult<ProductTag> Delete(int id)
        {
            var productTag = _service.Get(id);
            if (productTag == null) return BadRequest(string.Format(NOT_EXIST, nameof(ProductTag)));

            var contextUser = GetUserFromContext(HttpContext);
            if (contextUser.Id != productTag.Product.Owner.Id) return Unauthorized();

            _service.Remove(productTag);
            return Ok(productTag);

        }
    }
}
