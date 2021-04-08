using System;
using System.Collections.Generic;
using System.Linq;
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

        public override ValidationModel<bool> CanGet(Proposal obj)
        {
            CheckUserAuthorization(obj.Origin.Id);
            CheckUserAuthorization(obj.Target.Id);
            _validationModel.Value = _validationModel.Errors.Count < 2;
            return _validationModel;
        }

        public override ValidationModel<bool> CanAdd(Proposal obj)
        {
            _validationModel = IsObjectValid(obj);
            if (!_validationModel.Value) return _validationModel;
            CheckUserAuthorization(obj.Origin.Id);
            if (!_validationModel.Value) return _validationModel;

            // Check if Product exists
            var p = _repoProduct.FindOne(obj.Product.Id);
            if (p == null) _validationModel.Errors.Add("Proposal Product doesn't exist.");
            else obj.Product = p;

            _validationModel.Value = _validationModel.Errors.Count == 0;
            return _validationModel;
        }

        public override ValidationModel<bool> CanEdit(Proposal obj)
        {
            _validationModel = IsObjectValid(obj);
            if (!_validationModel.Value) return _validationModel;
            CheckUserAuthorization(obj.Origin.Id);
            return _validationModel;
        }

        public override ValidationModel<bool> CanDelete(Proposal obj)
        {
            _validationModel = IsObjectValid(obj);
            if (!_validationModel.Value) return _validationModel;
            CheckUserAuthorization(obj.Origin.Id);
            return _validationModel;
        }

        public override ValidationModel<bool> IsObjectValid(Proposal obj)
        {
            var listProps = new List<string> { nameof(obj.Origin), nameof(obj.Product), nameof(obj.Target) };
            _constraintsObject = new ConstraintsObject
            {
                PropsNonNull = listProps
            };
            // Basic check on fields (null, blank, size)
            _validationModel = base.IsObjectValid(obj);
            if (!_validationModel.Value) return _validationModel;

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

            // Check state
            if (!Enum.IsDefined(typeof(State), obj.State)) _validationModel.Errors.Add("Proposal State doesn't exist.");

            _validationModel.Value = _validationModel.Errors.Count == 0;
            return _validationModel;
        }
    }
}
