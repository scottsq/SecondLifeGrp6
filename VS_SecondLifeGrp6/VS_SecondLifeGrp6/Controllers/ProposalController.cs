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

        public ProposalController(IProposalService service)
        {
            _service = service;
        }

        #region GET
        [HttpGet]
        public ActionResult<List<Proposal>> List()
        {
            var res = _service.List();
            if (res.Value == null) return Unauthorized(res.Errors);
            return res.Value;
        }

        [HttpGet("{id}")]
        public ActionResult<Proposal> GetProposal(int id)
        {
            _service.SetContextUser(GetUserFromContext(HttpContext));
            var proposal = _service.Get(id);
            if (proposal.Errors.Count > 0) return Unauthorized(proposal.Errors);
            if (proposal.Value == null) return BadRequest(string.Format(NOT_EXIST, nameof(Proposal)));
            return proposal.Value;
        }

        [HttpGet("user/{id}")]
        public ActionResult<List<Proposal>> ListByUserId(int id)
        {
            _service.SetContextUser(GetUserFromContext(HttpContext));
            var res = _service.ListByUserId(id);
            if (res.Errors.Count > 0) return Unauthorized(res.Errors);
            return res.Value;
        }

        [HttpGet("user/{id}/active")]
        public ActionResult<List<Proposal>> ListByUserIdAndActive(int id)
        {
            _service.SetContextUser(GetUserFromContext(HttpContext));
            var res = _service.ListByUserIdAndActive(id); 
            if (res.Errors.Count > 0) return Unauthorized(res.Errors);
            return res.Value;
        }
        #endregion


        #region POST
        [HttpPost]
        public ActionResult<Proposal> Add(Proposal p)
        {
            _service.SetContextUser(GetUserFromContext(HttpContext));
            var res = _service.Add(p);
            return ReturnResult(res);
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
            _service.SetContextUser(GetUserFromContext(HttpContext));
            var res = _service.Update(id, state);
            return ReturnResult(res);
        }
        #endregion


        #region PATCH
        [HttpPatch("{id}")]
        public ActionResult<Proposal> Patch(int id, [FromBody] JsonPatchDocument<Proposal> patchDoc)
        {
            if (patchDoc == null) return BadRequest(ModelState);

            _service.SetContextUser(GetUserFromContext(HttpContext));
            var proposal = _service.Patch(id, patchDoc);
            return ReturnResult(proposal);
        }
        #endregion


        #region DELETE
        [HttpDelete("{id}")]
        public ActionResult<Proposal> Delete(int id)
        {
            _service.SetContextUser(GetUserFromContext(HttpContext));
            var proposal = _service.Get(id);
            if (proposal.Errors.Count > 0) return Unauthorized(proposal.Errors);
            if (proposal == null) return BadRequest(string.Format(NOT_EXIST, nameof(Proposal)));
            var check = _service.Remove(proposal.Value);
            return ReturnResult(check);
        }
        #endregion
    }
}
