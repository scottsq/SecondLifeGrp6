using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VS_SLG6.Api.Interfaces;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Services;

namespace VS_SLG6.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController, Route("api/[controller]")]
    public class ProposalController : ControllerBaseExtended
    {
        private IProposalService _service;
        private IProposalControllerAccess _controllerAccess;

        public ProposalController(IProposalService service, IProposalControllerAccess controllerAccess)
        {
            _service = service;
            _controllerAccess = controllerAccess;
        }

        [HttpGet("?id={id}&originId={originId}&targetId={targetId}&states={states}&orderBy={orderBy}&reverse={reverse}&from={from}&max={max}")]
        public ActionResult<List<Proposal>> List(int id = -1, int originId = -1, int targetId = -1, State[] states = null, string orderBy = null, bool reverse = false, int from = 0, int max = 10)
        {
            if (!_controllerAccess.CanGet(GetUserFromContext(HttpContext), id, originId, targetId)) return Unauthorized();
            return _service.Find(id, originId, targetId, states, orderBy, reverse, from, max);
        }

        [HttpPost]
        public ActionResult<Proposal> Add(Proposal p)
        {
            if (!_controllerAccess.CanAdd(GetUserFromContext(HttpContext), p)) return Unauthorized();
            return ReturnResult(_service.Add(p));
        }

        [HttpPatch("{id}")]
        public ActionResult<Proposal> Patch(int id, [FromBody] JsonPatchDocument<Proposal> patchDoc)
        {
            var p = _service.Get(id).Value;
            if (p == null) return NoContent();
            if (!_controllerAccess.CanEdit(GetUserFromContext(HttpContext), p)) return Unauthorized();
            if (patchDoc == null) return BadRequest(ModelState);
            return ReturnResult(_service.Patch(p, patchDoc));
        }

        [HttpDelete("{id}")]
        public ActionResult<Proposal> Delete(int id)
        {
            var p = _service.Get(id).Value;
            if (p == null) return NoContent();
            if (!_controllerAccess.CanEdit(GetUserFromContext(HttpContext), p)) return Unauthorized();
            return ReturnResult(_service.Remove(p));
        }
    }
}
