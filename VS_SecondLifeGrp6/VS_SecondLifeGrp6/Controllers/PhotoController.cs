using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Services;

namespace VS_SLG6.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController, Route("api/[controller]")]
    public class PhotoController : ControllerBaseExtended
    {
        private IService<Photo> _service;

        public PhotoController(IService<Photo> service, IUserService serviceUser): base(serviceUser)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<Photo>> List()
        {
            var res = _service.List();
            if (res.Count == 0) return NoContent();
            return res;
        }

        [HttpGet("{id}")]
        public ActionResult<Photo> GetPhoto(int id)
        {
            var photo = _service.Get(id);
            if (photo == null) return BadRequest();
            return photo;
        }

        [AllowAnonymous]
        [HttpGet("product/{id}")]
        public ActionResult<List<Photo>> GetProductPhotos(int id)
        {
            var photos = ((PhotoService)_service).GetByProduct(id);
            return photos;
        }

        [HttpPost]
        public ActionResult<Photo> Add(Photo p)
        {
            var contextUser = GetUserFromContext(HttpContext);
            if (contextUser.Id != p?.Product?.Owner?.Id) return Unauthorized();

            var res = _service.Add(p);
            if (res.Errors.Count > 0) return BadRequest(res);
            return res.Value;
        }

        [HttpPatch("{id}")]
        public ActionResult<Photo> Patch(int id, [FromBody] JsonPatchDocument<Photo> patchDoc)
        {
            if (patchDoc == null) return BadRequest(ModelState);

            var contextUser = GetUserFromContext(HttpContext);
            if (contextUser.Id != _service.Get(id)?.Product.Owner.Id) return Unauthorized();

            var photo = _service.Patch(id, patchDoc);
            return photo;
        }

        [HttpDelete]
        public ActionResult<Photo> Delete(int id)
        {
            var photo = _service.Get(id);
            if (photo == null) BadRequest(string.Format(NOT_EXIST, nameof(Photo)));

            var contextUser = GetUserFromContext(HttpContext);
            if (contextUser.Id != photo.Product.Owner.Id) return Unauthorized();

            _service.Remove(photo);
            return Ok(photo);
            

        }
    }
}
