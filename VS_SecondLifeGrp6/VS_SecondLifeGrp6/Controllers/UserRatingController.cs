using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VS_SLG6.Api.Controllers;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Services;

namespace VS_SLG6.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController, Route("api/[controller]")]
    public class UserRatingController : ControllerBaseExtended
    {
        private IUserRatingService _service;

        public UserRatingController(IUserRatingService service, IUserService serviceUser): base(serviceUser)
        {
            _service = service;
        }

        #region GET
        [HttpGet]
        public ActionResult<List<UserRating>> List()
        {
            var res = _service.List();
            if (res.Count == 0) return NoContent();
            return res;
        }

        [HttpGet("{id}")]
        public ActionResult<UserRating> GetRating(int id)
        {
            var rating = _service.Get(id);
            if (rating == null) return BadRequest();
            return rating;
        }

        [HttpGet("origin/{idOrigin}/target/{idTarget}")]
        public ActionResult<UserRating> GetUserRating(int idOrigin, int idTarget)
        {
            return _service.GetUserRating(idOrigin, idTarget);
        }
       
        [HttpGet("target/{id}")]
        public ActionResult<List<UserRating>> GetProductRatings(int id)
        {
            return _service.GetRatings(id);
        }

        [AllowAnonymous]
        [HttpGet("target/{id}/average")]
        public ActionResult<double> GetAverageUserRating(int id)
        {
            return _service.GetAverageRating(id);
        }
        #endregion


        #region POST
        [HttpPost]
        public ActionResult<UserRating> Add(UserRating r)
        {
            var contextUser = GetUserFromContext(HttpContext);
            if (contextUser.Id != r?.Origin?.Id) return Unauthorized();

            var res = _service.Add(r);
            if (res.Errors.Count > 0) return BadRequest(res);
            return res.Value;
        }
        #endregion


        #region PATCH
        [HttpPatch("{id}")]
        public ActionResult<UserRating> Patch(int id, [FromBody] JsonPatchDocument<UserRating> patchDoc)
        {
            if (patchDoc == null) return BadRequest(ModelState);

            var contextUser = GetUserFromContext(HttpContext);
            if (contextUser.Id != _service.Get(id)?.Origin.Id) return Unauthorized();

            var user = _service.Patch(id, patchDoc);
            return user;
        }
        #endregion

        
        #region DELETE
        [HttpDelete]
        public ActionResult<UserRating> Delete(int id)
        {
            var rating = _service.Get(id);
            if (rating == null) return BadRequest(string.Format(NOT_EXIST, nameof(UserRating)));

            var contextUser = GetUserFromContext(HttpContext);
            if (contextUser.Id != _service.Get(id)?.Origin.Id) return Unauthorized();

            _service.Remove(rating);
            return Ok(rating);
            
        }
        #endregion
    }
}
