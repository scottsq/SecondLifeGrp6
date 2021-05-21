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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController, Route("api/[controller]")]
    public class PhotoController : ControllerBaseExtended
    {
        private IService<Photo> _service;
        private IControllerAccess<Photo> _controllerAccess;

        public PhotoController(IService<Photo> service, IControllerAccess<Photo> controllerAccess)
        {
            _service = service;
            _controllerAccess = controllerAccess;
        }

        [AllowAnonymous]
        [HttpGet()]
        public ActionResult<List<Photo>> List(int id = -1, int productId = -1, string orderBy = null, bool reverse = false, int from = 0, int max = 10)
        {
            return ((IPhotoService)_service).Find(id, productId, orderBy, reverse, from, max);
        }

        [HttpPost]
        public ActionResult<Photo> Add(Photo p)
        {
            if (!_controllerAccess.CanAdd(GetUserFromContext(HttpContext), p)) return Unauthorized();            
            return ReturnResult(_service.Add(p));
        }

        [HttpPatch("{id}")]
        public ActionResult<Photo> Patch(int id, [FromBody] JsonPatchDocument<Photo> patchDoc)
        {
            var photo = _service.Get(id).Value;
            if (photo == null) return NoContent();

            if (!_controllerAccess.CanEdit(GetUserFromContext(HttpContext), photo)) return Unauthorized();
            if (patchDoc == null) return BadRequest(ModelState);

            return ReturnResult(_service.Patch(photo, patchDoc));
        }

        [HttpDelete("{id}")]
        public ActionResult<Photo> Delete(int id)
        {
            var photo = _service.Get(id).Value;
            if (photo == null) return NoContent();
            if (!_controllerAccess.CanDelete(GetUserFromContext(HttpContext), photo)) return Unauthorized();
            return ReturnResult(_service.Remove(photo));
        }
    }
}
