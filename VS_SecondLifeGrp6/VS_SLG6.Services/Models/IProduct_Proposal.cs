using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Services.Models
{
    public interface IProduct_Proposal
    {
        public List<Proposal> GetAcceptedProposalByUser(int id);
    }
}
