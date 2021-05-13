using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VS_SLG6.Api.ControllerAccess;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Services;

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
        [HttpGet]
        public ActionResult<List<Photo>> List()
        {
            return _service.List().Value;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<Photo> GetPhoto(int id)
        {
            var photo = _service.Get(id);
            if (photo.Value == null) return BadRequest(string.Format(NOT_EXIST, nameof(Photo)));
            return photo.Value;
        }

        [AllowAnonymous]
        [HttpGet("product/{id}")]
        public ActionResult<List<Photo>> GetProductPhotos(int id)
        {
            var photos = ((PhotoService)_service).Find(id);
            return photos;
        }

        [HttpPost]
        public ActionResult<Photo> Add(Photo p)
        {
            if (!_controllerAccess.CanAdd(GetUserFromContext(HttpContext), p)) {
                return Unauthorized();
            }
            var res = _service.Add(p);
            return ReturnResult(res);
        }

        [HttpPatch("{id}")]
        public ActionResult<Photo> Patch(int id, [FromBody] JsonPatchDocument<Photo> patchDoc)
        {
            if (patchDoc == null) return BadRequest(ModelState);

            _service.SetContextUser(GetUserFromContext(HttpContext));
            var photo = _service.Patch(id, patchDoc);
            return ReturnResult(photo);
        }

        [HttpDelete("{id}")]
        public ActionResult<Photo> Delete(int id)
        {
            _service.SetContextUser(GetUserFromContext(HttpContext));
            
            var photo = _service.Get(id);
            if (photo.Errors.Count > 0) return Unauthorized(photo.Errors);
            if (photo == null) return BadRequest(string.Format(NOT_EXIST, nameof(Photo)));

            var check = _service.Remove(photo.Value);
            return ReturnResult(check);
        }
    }
}
