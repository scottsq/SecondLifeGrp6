using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using VS_SLG6.Api.ControllerAccess;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Models;

namespace VS_SLG6.Api.Controllers
{
    public class ControllerBaseExtended : ControllerBase
    {
        public const string NOT_EXIST = "{0} does not exist.";

        public static ContextUser GetUserFromContext(HttpContext context)
        {
            var claims = context?.User?.Claims;
            if (claims == null) return null;

            int id = -1;
            int.TryParse(claims.FirstOrDefault(x => x.Type == "user_id")?.Value, out id);

            int role = 1;
            int.TryParse(claims.FirstOrDefault(x => x.Type == "user_role")?.Value, out role);
            if (role < 0 || role > Enum.GetValues(typeof(Roles)).Length) role = 1;

            return new ContextUser { Id = id, Role = (Roles)role };

        }

        public ActionResult<T> ReturnResult<T>(ValidationModel<T> obj)
        {
            if (obj.Errors.Count > 0) return BadRequest(obj.Errors);
            if (obj.Value == null) return NoContent();
            return obj.Value;
        }
    }
}
