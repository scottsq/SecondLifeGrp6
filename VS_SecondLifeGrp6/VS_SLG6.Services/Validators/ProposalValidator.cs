﻿using System;
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
        private IService<Product> _serviceProduct;
        private IService<User> _serviceUser;

        public ProposalValidator(IRepository<Proposal> repo, ValidationModel<bool> validationModel, IService<Product> serviceProduct, IService<User> serviceUser) : base(repo, validationModel) 
        {
            _serviceProduct = serviceProduct;
            _serviceUser = serviceUser;
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
            if (_serviceUser.Get(obj.Origin.Id) == null) _validationModel.Errors.Add("Proposal Origin doesn't exist.");
            // Check if Target exists
            if (_serviceUser.Get(obj.Target.Id) == null) _validationModel.Errors.Add("Proposal Target doesn't exist.");
            // Check if Product exists
            if (_serviceProduct.Get(obj.Product.Id) == null) _validationModel.Errors.Add("Proposal Product doesn't exist.");
            // Init state as ACTIVE
            obj.State = State.ACTIVE;
            _validationModel.Value = _validationModel.Errors.Count == 0;
            return _validationModel;
        }
    }
}
