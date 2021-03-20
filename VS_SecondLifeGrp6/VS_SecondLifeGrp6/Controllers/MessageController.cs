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
    [ApiController, Route("api/[controller]")]
    public class MessageController : ControllerBase
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
            if (res.Count == 0) return NoContent();
            return res;
        }

        [HttpGet("{id}")]
        public ActionResult<Message> GetMessage(int id)
        {
            var message = _service.Get(id);
            if (message == null) return BadRequest();
            return message;
        }
        
        [HttpGet("{idOrigin}")]
        public ActionResult<List<Message>> GetConversations(int idOrigin)
        {
            return _service.ListConversations(idOrigin);
        }

        [HttpGet("{idOrigin}/{idDest}")]
        public ActionResult<List<Message>> GetConversation(int idOrigin, int idDest)
        {
            return _service.GetConversation(idOrigin, idDest);
        }
        #endregion

        #region POST
        [HttpPost]
        public ActionResult<Message> Add(Message u)
        {
            var res = _service.Add(u);
            if (res.Errors.Count > 0) return BadRequest(res);
            return res.Value;
        }
        #endregion

        #region PATCH
        [HttpPatch("{id}")]
        public ActionResult<Message> Patch(int id, [FromBody] JsonPatchDocument<Message> patchDoc)
        {
            if (patchDoc == null) return BadRequest(ModelState);
            var message = _service.Patch(id, patchDoc);
            return message;
        }
        #endregion

        #region DELETE
        [HttpDelete]
        public ActionResult<Message> Delete(int id)
        {
            var message = _service.Get(id);
            if (message != null)
            {
                _service.Remove(message);
                return Ok(message);
            }
            else return BadRequest("Invalid Product");

        }
        #endregion
    }
}
