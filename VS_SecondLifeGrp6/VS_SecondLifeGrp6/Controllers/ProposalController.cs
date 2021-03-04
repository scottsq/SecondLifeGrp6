using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Services;

namespace VS_SLG6.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class ProposalController : ControllerBase
    {
        private IProposalService _service;

        public ProposalController(IProposalService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<Proposal>> List()
        {
            var res = _service.List();
            if (res.Count == 0) return NoContent();
            return res;
        }

        [HttpGet("{id}")]
        public ActionResult<Proposal> GetProposal(int id)
        {
            var proposal = _service.Get(id);
            if (proposal == null) return BadRequest();
            return proposal;
        }

        [HttpGet("user/{id}")]
        public ActionResult<List<Proposal>> ListByUserId(int id)
        {
            return _service.ListByUserId(id);
        }

        [HttpGet("user/{id}/active")]
        public ActionResult<List<Proposal>> ListByUserIdAndActive(int id)
        {
            return _service.ListByUserIdAndActive(id);
        }

        [HttpPost]
        public ActionResult<Proposal> Add(Proposal p)
        {
            var res = _service.Add(p);
            if (res.Errors.Count > 0) return BadRequest(res);
            return res.Value;
        }

        [HttpPatch("{id}")]
        public ActionResult<Proposal> Patch(int id, [FromBody] JsonPatchDocument<Proposal> patchDoc)
        {
            if (patchDoc == null) return BadRequest(ModelState);
            var proposal = _service.Patch(id, patchDoc);
            return proposal;
        }

        [HttpDelete]
        public ActionResult<Proposal> Delete(int id)
        {
            var proposal = _service.Get(id);
            if (proposal != null)
            {
                _service.Remove(proposal);
                return Ok(proposal);
            }
            else return BadRequest("Invalid Product");

        }
    }
}
