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

        public MessageController(IMessageService service)
        {
            _service = service;
        }

        #region GET
        [HttpGet]
        public ActionResult<List<Message>> List()
        {
            var res = _service.List();
            if (res.Value == null) return Unauthorized(res.Errors);
            return res.Value;
        }

        [HttpGet("{id}")]
        public ActionResult<Message> GetMessage(int id)
        {
            _service.SetContextUser(GetUserFromContext(HttpContext));
            var message = _service.Get(id);
            if (message.Errors.Count > 0) return Unauthorized(message.Errors);
            if (message.Value == null) return BadRequest(string.Format(NOT_EXIST, nameof(Message)));
            return message.Value;
        }
        
        [HttpGet("{idOrigin}")]
        public ActionResult<List<Message>> GetConversations(int idOrigin)
        {
            _service.SetContextUser(GetUserFromContext(HttpContext));
            var res = _service.ListConversations(idOrigin);
            if (res.Errors.Count > 0) return Unauthorized(res.Errors);
            return res.Value;
        }

        [HttpGet("{idOrigin}/{idDest}")]
        public ActionResult<List<Message>> GetConversation(int idOrigin, int idDest)
        {
            _service.SetContextUser(GetUserFromContext(HttpContext));
            var res = _service.GetConversation(idOrigin, idDest);
            if (res.Errors.Count > 0) return Unauthorized(res.Errors);
            return res.Value;
        }
        #endregion

        #region POST
        [HttpPost]
        public ActionResult<Message> Add(Message m)
        {
            _service.SetContextUser(GetUserFromContext(HttpContext));
            var res = _service.Add(m);
            return ReturnResult(res);
        }
        #endregion

        #region PATCH
        [HttpPatch("{id}")]
        public ActionResult<Message> Patch(int id, [FromBody] JsonPatchDocument<Message> patchDoc)
        {
            if (patchDoc == null) return BadRequest(ModelState);

            _service.SetContextUser(GetUserFromContext(HttpContext));
            var message = _service.Patch(id, patchDoc);
            return ReturnResult(message);
        }
        #endregion

        #region DELETE
        [HttpDelete("{id}")]
        public ActionResult<Message> Delete(int id)
        {
            _service.SetContextUser(GetUserFromContext(HttpContext));
            var message = _service.Get(id);
            if (message.Errors.Count > 0) return Unauthorized(message.Errors);
            if (message.Value == null) return BadRequest(string.Format(NOT_EXIST, nameof(Message)));
            
            var check = _service.Remove(message.Value);
            return ReturnResult(check);
        }
        #endregion
    }
}
