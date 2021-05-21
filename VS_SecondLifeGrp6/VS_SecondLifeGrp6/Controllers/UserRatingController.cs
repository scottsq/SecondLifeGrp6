using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VS_SLG6.Api.Controllers;
using VS_SLG6.Api.Interfaces;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Interfaces;
using VS_SLG6.Services.Services;

namespace VS_SLG6.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController, Route("api/[controller]")]
    public class UserRatingController : ControllerBaseExtended
    {
        private IUserRatingService _service;
        private IControllerAccess<UserRating> _controllerAccess;

        public UserRatingController(IUserRatingService service, IControllerAccess<UserRating> controllerAccess)
        {
            _service = service;
            _controllerAccess = controllerAccess;
        }

        [AllowAnonymous]
        [HttpGet()]
        public ActionResult<List<UserRating>> List(int id = -1, int idOrigin = -1, int idTarget = -1, int stars = -1, string orderBy = null, bool reverse = false, int from = 0, int max = 10)
        {
            return _service.Find(id, idOrigin, idTarget, stars, orderBy, reverse, from, max);
        }

        [AllowAnonymous]
        [HttpGet("average")]
        public ActionResult<double> GetAverageUserRating(int idTarget)
        {
            return _service.GetAverageRating(idTarget);
        }

        [HttpPost]
        public ActionResult<UserRating> Add(UserRating r)
        {
            if (!_controllerAccess.CanAdd(GetUserFromContext(HttpContext), r)) return Unauthorized();
            return ReturnResult(_service.Add(r));
        }

        [HttpPatch("{id}")]
        public ActionResult<UserRating> Patch(int id, [FromBody] JsonPatchDocument<UserRating> patchDoc)
        {
            var userRating = _service.Get(id).Value;
            if (userRating == null) return NoContent();
            if (!_controllerAccess.CanEdit(GetUserFromContext(HttpContext), userRating)) return Unauthorized();
            if (patchDoc == null) return BadRequest(ModelState);
            return ReturnResult(_service.Patch(userRating, patchDoc));
        }

        [HttpDelete("{id}")]
        public ActionResult<UserRating> Delete(int id)
        {
            var userRating = _service.Get(id).Value;
            if (userRating == null) return NoContent();
            if (!_controllerAccess.CanDelete(GetUserFromContext(HttpContext), userRating)) return Unauthorized();
            return ReturnResult(_service.Remove(userRating));
        }
    }
}
