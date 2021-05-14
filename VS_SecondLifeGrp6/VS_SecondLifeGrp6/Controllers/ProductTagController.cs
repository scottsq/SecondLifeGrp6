using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using VS_SLG6.Api.Interfaces;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Services;

namespace VS_SLG6.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController, Route("api/[controller]")]
    public class ProductTagController : ControllerBaseExtended
    {
        private IProductTagService _service;
        private IControllerAccess<ProductTag> _controllerAccess;

        public ProductTagController(IProductTagService service, IControllerAccess<ProductTag> controller)
        {
            _service = service;
            _controllerAccess = controller;
        }

        [HttpGet("?id={id}&productId={productId}&orderBy={orderBy}&reverse={reverse}&from={from}&max={max}")]
        public ActionResult<List<ProductTag>> List(int id = -1, int productId = -1, string orderBy = null, bool reverse = false, int from = 0, int max = 10)
        {
            return _service.Find(id, productId, orderBy, reverse, from, max);
        }

        [HttpPost]
        public ActionResult<ProductTag> Add(ProductTag productTag)
        {
            if (!_controllerAccess.CanAdd(GetUserFromContext(HttpContext), productTag)) return Unauthorized();
            return ReturnResult(_service.Add(productTag));
        }

        [HttpPatch("{id}")]
        public ActionResult<ProductTag> Patch(int id, [FromBody] JsonPatchDocument<ProductTag> patchDoc)
        {
            var productTag = _service.Get(id).Value;
            if (productTag == null) return NoContent();
            if (!_controllerAccess.CanEdit(GetUserFromContext(HttpContext), productTag)) return Unauthorized();
            if (patchDoc == null) return BadRequest(ModelState);
            return ReturnResult(_service.Patch(productTag, patchDoc));
        }

        [HttpDelete("{id}")]
        public ActionResult<ProductTag> Delete(int id)
        {
            var productTag = _service.Get(id).Value;
            if (productTag == null) return NoContent();
            if (!_controllerAccess.CanDelete(GetUserFromContext(HttpContext), productTag)) return Unauthorized();
            return ReturnResult(_service.Remove(productTag));
        }
    }
}
