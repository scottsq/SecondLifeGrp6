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
    public class MessageController : ControllerBaseExtended
    {
        private IMessageService _service;
        private IMessageControllerAccess _controllerAccess;

        public MessageController(IMessageService service, IMessageControllerAccess controllerAccess)
        {
            _service = service;
            _controllerAccess = controllerAccess;
        }

        [HttpGet()]
        public ActionResult<List<Message>> List(int id = -1, int idOrigin = -1, int idDest = -1, bool twoWays = false, string orderBy = nameof(Message.CreationDate), bool reverse = false, int from = 0, int max = 10)
        {
            if (!_controllerAccess.CanGet(GetUserFromContext(HttpContext), id, idOrigin, idDest)) return Unauthorized();
            return _service.Find(id, idOrigin, idDest, twoWays, orderBy, reverse, from, max);
        }

        [HttpPost]
        public ActionResult<Message> Add(Message m)
        {
            if (!_controllerAccess.CanAdd(GetUserFromContext(HttpContext), m)) return Unauthorized();
            return ReturnResult(_service.Add(m));
        }

        [HttpPatch("{id}")]
        public ActionResult<Message> Patch(int id, [FromBody] JsonPatchDocument<Message> patchDoc)
        {
            var message = _service.Find(id: id)[0];
            if (message == null) return NoContent();

            if (!_controllerAccess.CanEdit(GetUserFromContext(HttpContext), message)) return Unauthorized();
            if (patchDoc == null) return BadRequest(ModelState);

            return ReturnResult(_service.Patch(message, patchDoc));
        }

        [HttpDelete("{id}")]
        public ActionResult<Message> Delete(int id)
        {
            var message = _service.Find(id: id)[0];
            if (message == null) return NoContent();
            if (!_controllerAccess.CanDelete(GetUserFromContext(HttpContext), message)) return Unauthorized();
            return ReturnResult(_service.Remove(message));
        }
    }
}
