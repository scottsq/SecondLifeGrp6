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

        public UserRatingController(IUserRatingService service)
        {
            _service = service;
        }

        #region GET
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<List<UserRating>> List()
        {
            return _service.List().Value;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<UserRating> GetRating(int id)
        {
            var rating = _service.Get(id);
            if (rating.Value == null) return BadRequest(string.Format(NOT_EXIST, nameof(UserRating)));
            return rating.Value;
        }

        [AllowAnonymous]
        [HttpGet("origin/{idOrigin}/target/{idTarget}")]
        public ActionResult<UserRating> GetUserRating(int idOrigin, int idTarget)
        {
            return _service.GetUserRating(idOrigin, idTarget);
        }
       
        [AllowAnonymous]
        [HttpGet("target/{id}")]
        public ActionResult<List<UserRating>> GetRatings(int id)
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
            _service.SetContextUser(GetUserFromContext(HttpContext));
            var res = _service.Add(r);
            return ReturnResult(res);
        }
        #endregion


        #region PATCH
        [HttpPatch("{id}")]
        public ActionResult<UserRating> Patch(int id, [FromBody] JsonPatchDocument<UserRating> patchDoc)
        {
            if (patchDoc == null) return BadRequest(ModelState);
            _service.SetContextUser(GetUserFromContext(HttpContext));
            var user = _service.Patch(id, patchDoc);
            return ReturnResult(user);
        }
        #endregion

        
        #region DELETE
        [HttpDelete("{id}")]
        public ActionResult<UserRating> Delete(int id)
        {
            _service.SetContextUser(GetUserFromContext(HttpContext));
            var rating = _service.Get(id);
            if (rating == null) return BadRequest(string.Format(NOT_EXIST, nameof(UserRating)));
            var check = _service.Remove(rating.Value);
            return ReturnResult(check);
        }
        #endregion
    }
}
