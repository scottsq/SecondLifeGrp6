using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Services;

namespace VS_SLG6.Api.Controllers
{
    public class ControllerBaseExtended : ControllerBase
    {
        public const string NOT_EXIST = "{0} does not exist.";
        private static IUserService _service;

        public ControllerBaseExtended(IUserService service)
        {
            _service = service;
        }

        public static User GetUserFromContext(HttpContext context)
        {
            int res = -1;
            int.TryParse(context?.User?.Claims?.FirstOrDefault(x => x.Type == "user_id")?.Value, out res);
            return _service.Get(res);
        }
    }
}
