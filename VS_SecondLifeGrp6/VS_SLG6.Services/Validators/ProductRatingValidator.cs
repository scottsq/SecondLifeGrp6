using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;
using VS_SLG6.Services.Services;

namespace VS_SLG6.Services.Validators
{
    public class ProductRatingValidator : GenericValidator<ProductRating>, IValidator<ProductRating>
    {
        private IService<Product> _serviceProduct;
        private IService<User> _serviceUser;

        public ProductRatingValidator(IRepository<ProductRating> repo, ValidationModel<bool> validationModel, IService<Product> serviceProduct, IService<User> serviceUser) : base(repo, validationModel) 
        {
            _serviceProduct = serviceProduct;
            _serviceUser = serviceUser;
        }

        public override ValidationModel<bool> CanAdd(ProductRating obj)
        {
            _validationModel.Value = false;
            if (obj == null)
            {
                _validationModel.Errors.Add("Cannot add a null rating");
                return _validationModel;
            }
            // Check null fields
            if (obj.User == null || obj.Product == null)
            {
                _validationModel.Errors.Add("Rating object cannot have empty fields.");
                return _validationModel;
            }
            // Check stars between 1 and 5
            if (obj.Stars < 1 || obj.Stars > 5) _validationModel.Errors.Add("Rating Stars must be between 1 and 5.");
            // Check if User exists
            if (_serviceUser.Get(obj.User.Id) == null) _validationModel.Errors.Add("Rating User doesn't exist.");
            // Check if Product exists
            if (_serviceProduct.Get(obj.Product.Id) == null) _validationModel.Errors.Add("Rating Product doesn't exist.");
            // Check if Rating already exists
            if (_repo.FindAll(x => x.Product.Id == obj.Product.Id && x.User.Id == obj.User.Id).Count > 0)
            {
                _validationModel.Errors.Add("Rating on this Product already exists for this User.");
            }
            _validationModel.Value = _validationModel.Errors.Count == 0;
            return _validationModel;
        }
    }
}
