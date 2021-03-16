using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;
using VS_SLG6.Services.Services;

namespace VS_SLG6.Services.Validators
{
    public class ProductTagValidator : GenericValidator<ProductTag>, IValidator<ProductTag>
    {
        private IService<Product> _serviceProduct;
        private IService<Tag> _serviceTag;

        public ProductTagValidator(IRepository<ProductTag> repo, ValidationModel<bool> validationModel, IService<Product> serviceProduct, IService<Tag> serviceTag) : base(repo, validationModel) 
        {
            _serviceProduct = serviceProduct;
            _serviceTag = serviceTag;
        }

        public override ValidationModel<bool> CanAdd(ProductTag obj)
        {
            _validationModel.Value = false;
            if (obj == null)
            {
                _validationModel.Errors.Add("Cannot add a null ProductTag");
                return _validationModel;
            }
            // Check null fields
            if (obj.Product == null || obj.Tag == null)
            {
                _validationModel.Errors.Add("ProductTag object cannot have empty fields.");
                return _validationModel;
            }
            // Check if Tag exists
            if (_serviceTag.Get(obj.Tag.Id) == null) _validationModel.Errors.Add("ProductTag Tag doesn't exist.");
            // Check if Product exists
            if (_serviceProduct.Get(obj.Product.Id) == null) _validationModel.Errors.Add("ProductTag Product doesn't exist.");
            // Check if already exists
            if (_repo.FindAll(x => x.Tag.Id == obj.Tag.Id && x.Product.Id == obj.Product.Id).Count > 0)
            {
                _validationModel.Errors.Add("ProductTag with similar Product and Tag already exists");
            }
            _validationModel.Value = _validationModel.Errors.Count == 0;
            return _validationModel;
        }
    }
}
