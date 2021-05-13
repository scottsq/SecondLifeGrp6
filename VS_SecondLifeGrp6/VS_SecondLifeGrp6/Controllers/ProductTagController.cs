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

        public ProductTagController(IService<ProductTag> service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<ProductTag>> List()
        {
            return _service.List().Value;
        }

        [HttpGet("{id}")]
        public ActionResult<ProductTag> GetProductTag(int id)
        {
            var productTag = _service.Get(id);
            if (productTag.Value == null) return BadRequest(string.Format(NOT_EXIST, nameof(ProductTag)));
            return productTag.Value;
        }

        [AllowAnonymous]
        [HttpGet("product/{id}")]
        public ActionResult<List<ProductTag>> GetProductTags(int id)
        {
            return ((ProductTagService)_service).Find(id);
        }

        [HttpPost]
        public ActionResult<ProductTag> Add(ProductTag productTag)
        {
            _service.SetContextUser(GetUserFromContext(HttpContext));
            var res = _service.Add(productTag);
            return ReturnResult(res);
        }

        [HttpPatch("{id}")]
        public ActionResult<ProductTag> Patch(int id, [FromBody] JsonPatchDocument<ProductTag> patchDoc)
        {
            if (patchDoc == null) return BadRequest(ModelState);
            var productTag = _service.Patch(id, patchDoc);
            _service.SetContextUser(GetUserFromContext(HttpContext));
            return ReturnResult(productTag);
        }

        [HttpDelete("{id}")]
        public ActionResult<ProductTag> Delete(int id)
        {
            _service.SetContextUser(GetUserFromContext(HttpContext));
            var productTag = _service.Get(id);
            if (productTag == null) return BadRequest(string.Format(NOT_EXIST, nameof(ProductTag)));

            var check = _service.Remove(productTag.Value);
            return ReturnResult(check);
        }
    }
}
