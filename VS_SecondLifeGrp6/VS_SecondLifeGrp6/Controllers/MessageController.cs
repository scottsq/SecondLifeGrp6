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
    public class MessageController : ControllerBaseExtended
    {
        private IMessageService _service;

        public MessageController(IMessageService service, IUserService userService): base(userService)
        {
            _service = service;
        }

        #region GET
        [HttpGet]
        public ActionResult<List<Message>> List()
        {
            // Devrait être accessible seulement à un admin par exemple
            // Mais je n'ai pas mis en place de rôle donc par défaut on renvoie Unauthorized -> évolution possible
            return Unauthorized();
            var res = _service.List();
            if (res.Count == 0) return NoContent();
            return res;
        }

        [HttpGet("{id}")]
        public ActionResult<Message> GetMessage(int id)
        {
            var message = _service.Get(id);
            if (message == null) return BadRequest();

            var contextUser = GetUserFromContext(HttpContext);
            if (contextUser.Id != message.Sender.Id) return Unauthorized();

            return message;
        }
        
        [HttpGet("{idOrigin}")]
        public ActionResult<List<Message>> GetConversations(int idOrigin)
        {
            var contextUser = GetUserFromContext(HttpContext);
            if (contextUser.Id != idOrigin) return Unauthorized();

            return _service.ListConversations(idOrigin);
        }

        [HttpGet("{idOrigin}/{idDest}")]
        public ActionResult<List<Message>> GetConversation(int idOrigin, int idDest)
        {
            var contextUser = GetUserFromContext(HttpContext);
            if (contextUser.Id != idOrigin) return Unauthorized();

            return _service.GetConversation(idOrigin, idDest);
        }
        #endregion

        #region POST
        [HttpPost]
        public ActionResult<Message> Add(Message m)
        {
            var contextUser = GetUserFromContext(HttpContext);
            if (contextUser.Id != m?.Sender?.Id) return Unauthorized();

            var res = _service.Add(m);
            if (res.Errors.Count > 0) return BadRequest(res);
            return res.Value;
        }
        #endregion

        #region PATCH
        [HttpPatch("{id}")]
        public ActionResult<Message> Patch(int id, [FromBody] JsonPatchDocument<Message> patchDoc)
        {
            if (patchDoc == null) return BadRequest(ModelState);

            var contextUser = GetUserFromContext(HttpContext);
            if (contextUser.Id != _service.Get(id)?.Sender.Id) return Unauthorized();

            var message = _service.Patch(id, patchDoc);
            return message;
        }
        #endregion

        #region DELETE
        [HttpDelete]
        public ActionResult<Message> Delete(int id)
        {
            var message = _service.Get(id);
            if (message == null) BadRequest(string.Format(NOT_EXIST, nameof(Message)));
            
            var contextUser = GetUserFromContext(HttpContext);
            if (contextUser.Id != message.Sender.Id) return Unauthorized();

            _service.Remove(message);
            return Ok(message);

        }
        #endregion
    }
}
