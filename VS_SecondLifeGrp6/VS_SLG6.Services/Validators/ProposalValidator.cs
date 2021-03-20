using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;
using VS_SLG6.Services.Services;

namespace VS_SLG6.Services.Validators
{
    public class ProposalValidator : GenericValidator<Proposal>, IValidator<Proposal>
    {
        private IRepository<Product> _repoProduct;
        private IRepository<User> _repoUser;

        public ProposalValidator(IRepository<Proposal> repo, ValidationModel<bool> validationModel, IRepository<Product> repoProduct, IRepository<User> repoUser) : base(repo, validationModel) 
        {
            _repoProduct = repoProduct;
            _repoUser = repoUser;
        }

        public override ValidationModel<bool> CanAdd(Proposal obj)
        {
            _validationModel.Value = false;
            if (obj == null)
            {
                _validationModel.Errors.Add("Cannot add a null proposal");
                return _validationModel;
            }
            // Check null fields
            if (obj.Origin == null || obj.Product == null || obj.Target == null)
            {
                _validationModel.Errors.Add("Proposal object cannot have empty fields.");
                return _validationModel;
            }
            // Check negative price
            if (obj.Price < 0) _validationModel.Errors.Add("Proposal Price cannot be negative.");
            // Check if Origin exists
            var o = _repoUser.FindOne(obj.Origin.Id);
            if (o == null) _validationModel.Errors.Add("Proposal Origin doesn't exist.");
            else obj.Origin = o;
            // Check if Target exists
            var t = _repoUser.FindOne(obj.Target.Id);
            if (t == null) _validationModel.Errors.Add("Proposal Target doesn't exist.");
            else obj.Target = t;
            // Check if Product exists
            var p = _repoProduct.FindOne(obj.Product.Id);
            if (p == null) _validationModel.Errors.Add("Proposal Product doesn't exist.");
            else obj.Product = p;
            // Check state
            if (!Enum.IsDefined(typeof(State), obj.State)) _validationModel.Errors.Add("Proposal State doesn't exist.");
            _validationModel.Value = _validationModel.Errors.Count == 0;
            return _validationModel;
        }
    }
}
