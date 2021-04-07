using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Services;

namespace VS_SLG6.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController, Route("api/[controller]")]
    public class ProposalController : ControllerBaseExtended
    {
        private IProposalService _service;

        public ProposalController(IProposalService service, IUserService userService): base(userService)
        {
            _service = service;
        }

        #region GET
        [HttpGet]
        public ActionResult<List<Proposal>> List()
        {
            // Devrait être accessible seulement à un admin par exemple
            // Mais je n'ai pas mis en place de rôle donc par défaut on renvoie Unauthorized -> évolution possible
            return Unauthorized();
            var res = _service.List();
            if (res.Count == 0) return NoContent();
            return res;
        }

        [HttpGet("{id}")]
        public ActionResult<Proposal> GetProposal(int id)
        {
            var proposal = _service.Get(id);

            var contextUser = GetUserFromContext(HttpContext);
            if (contextUser.Id != proposal.Origin.Id && contextUser.Id != proposal.Target.Id) return Unauthorized();

            if (proposal == null) return BadRequest();
            return proposal;
        }

        [HttpGet("user/{id}")]
        public ActionResult<List<Proposal>> ListByUserId(int id)
        {
            var contextUser = GetUserFromContext(HttpContext);
            if (contextUser.Id != id) return Unauthorized();

            return _service.ListByUserId(id);
        }

        [HttpGet("user/{id}/active")]
        public ActionResult<List<Proposal>> ListByUserIdAndActive(int id)
        {
            var contextUser = GetUserFromContext(HttpContext);
            if (contextUser.Id != id) return Unauthorized();

            return _service.ListByUserIdAndActive(id);
        }
        #endregion


        #region POST
        [HttpPost]
        public ActionResult<Proposal> Add(Proposal p)
        {
            var contextUser = GetUserFromContext(HttpContext);
            if (contextUser.Id != p?.Origin?.Id) return Unauthorized();

            var res = _service.Add(p);
            if (res.Errors.Count > 0) return BadRequest(res);
            return res.Value;
        }

        [HttpPost("accept")]
        public ActionResult<Proposal> Accept(int id)
        {
            return ChangeState(id, State.ACCEPTED);
        }

        [HttpPost("refuse")]
        public ActionResult<Proposal> Refuse(int id)
        {
            return ChangeState(id, State.REFUSED);
        }

        [HttpPost("close")]
        public ActionResult<Proposal> Close(int id)
        {
            return ChangeState(id, State.CLOSED);
        }

        public ActionResult<Proposal> ChangeState(int id, State state)
        {
            var contextUser = GetUserFromContext(HttpContext);
            if (contextUser.Id != _service.Get(id)?.Origin.Id) return Unauthorized();

            var res = _service.UpdateProposal(id, state);
            if (res.Errors.Count > 0) return BadRequest(res);
            return res.Value;
        }
        #endregion


        #region PATCH
        [HttpPatch("{id}")]
        public ActionResult<Proposal> Patch(int id, [FromBody] JsonPatchDocument<Proposal> patchDoc)
        {
            if (patchDoc == null) return BadRequest(ModelState);

            var contextUser = GetUserFromContext(HttpContext);
            if (contextUser.Id != _service.Get(id)?.Origin.Id) return Unauthorized();

            var proposal = _service.Patch(id, patchDoc);
            return proposal;
        }
        #endregion


        #region DELETE
        [HttpDelete]
        public ActionResult<Proposal> Delete(int id)
        {
            var proposal = _service.Get(id);
            if (proposal == null) return BadRequest(string.Format(NOT_EXIST, nameof(Proposal)));

            var contextUser = GetUserFromContext(HttpContext);
            if (contextUser.Id != _service.Get(id)?.Origin.Id) return Unauthorized();

            _service.Remove(proposal);
            return Ok(proposal);
        }
        #endregion
    }
}
