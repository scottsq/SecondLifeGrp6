using System;
using System.Collections.Generic;
using System.Linq;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;

namespace VS_SLG6.Services.Validators
{
    public class ProposalValidator : GenericValidator<Proposal>, IValidator<Proposal>
    {
        private IRepository<Product> _repoProduct;
        private IRepository<User> _repoUser;

        public ProposalValidator(IRepository<Proposal> repo, IRepository<Product> repoProduct, IRepository<User> repoUser) : base(repo) 
        {
            _repoProduct = repoProduct;
            _repoUser = repoUser;
        }

        public override List<string> CanAdd(Proposal obj)
        {
            var listErrors = IsObjectValid(obj);
            if (listErrors.Any()) return listErrors;

            // Check if Product exists
            var p = _repoProduct.FindOne(obj.Product.Id);
            if (p == null) listErrors.Add("Proposal Product doesn't exist.");
            else obj.Product = p;

            return listErrors;
        }

        public override List<string> IsObjectValid(Proposal obj, ConstraintsObject constraintsObject = null)
        {
            var listProps = new List<string> { nameof(Proposal.Origin), nameof(Proposal.Product), nameof(Proposal.Target) };
            constraintsObject = new ConstraintsObject
            {
                FieldsNotNull = listProps
            };
            // Basic check on fields (null, blank, size)
            var listErrors = base.IsObjectValid(obj, constraintsObject);
            if (listErrors.Any()) return listErrors;

            // Check negative price
            if (obj.Price < 0) listErrors.Add("Proposal Price cannot be negative.");

            // Check if Origin exists
            var o = _repoUser.FindOne(obj.Origin.Id);
            if (o == null) listErrors.Add("Proposal Origin doesn't exist.");
            else obj.Origin = o;

            // Check if Target exists
            var t = _repoUser.FindOne(obj.Target.Id);
            if (t == null) listErrors.Add("Proposal Target doesn't exist.");
            else obj.Target = t;

            // Check state
            if (!Enum.IsDefined(typeof(State), obj.State)) listErrors.Add("Proposal State doesn't exist.");

            return listErrors;
        }
    }
}
