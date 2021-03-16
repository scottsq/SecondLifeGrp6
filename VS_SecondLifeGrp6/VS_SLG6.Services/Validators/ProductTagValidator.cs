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
        private IRepository<Product> _repoProduct;
        private IRepository<Tag> _repoTag;

        public ProductTagValidator(IRepository<ProductTag> repo, ValidationModel<bool> validationModel, IRepository<Product> repoProduct, IRepository<Tag> repoTag) : base(repo, validationModel) 
        {
            _repoProduct = repoProduct;
            _repoTag = repoTag;
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
            var t = _repoTag.FindOne(obj.Tag.Id);
            if (t == null) _validationModel.Errors.Add("ProductTag Tag doesn't exist.");
            else obj.Tag = t;
            // Check if Product exists
            var p = _repoProduct.FindOne(obj.Product.Id);
            if (p == null) _validationModel.Errors.Add("ProductTag Product doesn't exist.");
            else obj.Product = p;
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
