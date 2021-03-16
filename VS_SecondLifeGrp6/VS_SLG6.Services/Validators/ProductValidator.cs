using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;
using VS_SLG6.Services.Services;

namespace VS_SLG6.Services.Validators
{
    public class ProductValidator : GenericValidator<Product>, IValidator<Product>
    {
        private IService<User> _serviceUser;

        public ProductValidator(IRepository<Product> repo, ValidationModel<bool> validationModel, IService<User> serviceUser) : base(repo, validationModel) 
        {
            _serviceUser = serviceUser;
        }

        public override ValidationModel<bool> CanAdd(Product obj)
        {
            _validationModel.Value = false;
            if (obj == null)
            {
                _validationModel.Errors.Add("Cannot add a null product"); 
                return _validationModel;
            }
            // Check null fields
            if (obj.Description == null || obj.Name == null || obj.Owner == null)
            {
                _validationModel.Errors.Add("Product object cannot have empty fields.");
                return _validationModel;
            }
            // Check empty strings
            var check = StringIsEmptyOrBlank(obj, "Description", "Name");
            if (!check.Value) AppendFormattedErrors(check.Errors, "Product {0} cannot be empty.");
            // Check negative price
            if (obj.Price < 0) _validationModel.Errors.Add("Product price cannot be negative.");
            // Check if owner exists
            if (_serviceUser.Get(obj.Owner.Id) == null) _validationModel.Errors.Add("Product Owner doesn't exist.");
            // Format date
            if (obj.CreationDate == DateTime.MinValue) obj.CreationDate = DateTime.Now;
            // Check if already exists
            if (_repo.FindAll(x => x.Name == obj.Name && x.Price == obj.Price && x.Owner.Id == obj.Owner.Id).Count > 0)
            {
                _validationModel.Errors.Add("This user already saved a similar product.");
            }
            _validationModel.Value = _validationModel.Errors.Count == 0;
            return _validationModel;
        }
    }
}
