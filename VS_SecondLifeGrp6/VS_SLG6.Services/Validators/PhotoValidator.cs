using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;
using VS_SLG6.Services.Services;

namespace VS_SLG6.Services.Validators
{
    public class PhotoValidator : GenericValidator<Photo>, IValidator<Photo>
    {
        private IRepository<Product> _repoProduct;

        public PhotoValidator(IRepository<Photo> repo, ValidationModel<bool> validationModel, IRepository<Product> repoProduct): base(repo, validationModel)
        {
            _repoProduct = repoProduct;
        }

        public override ValidationModel<bool> CanAdd(Photo obj)
        {
            _validationModel.Value = false;
            // check if null
            if (obj == null)
            {
                _validationModel.Errors.Add("Cannot add null Photo.");
                return _validationModel;
            }
            // check if fields are null
            if (obj.Product == null || obj.Url == null)
            {
                _validationModel.Errors.Add("Cannot have null fields for Photo.");
                return _validationModel;
            }
            // check url
            var check = StringIsEmptyOrBlank(obj, "Url");
            if (check.Value) AppendFormattedErrors(check.Errors, "Photo {0} cannot be empty.");
            // check product
            var p = _repoProduct.FindOne(obj.Product.Id);
            if (p == null) _validationModel.Errors.Add("Unknown product.");
            else obj.Product = p;
            // check if already exists
            if (_repo.All(x => x.Product.Id == obj.Product.Id && x.Url == obj.Url).Count > 0)
            {
                _validationModel.Errors.Add("A Photo with this url already exists for this product.");
            }
            _validationModel.Value = _validationModel.Errors.Count == 0;
            return _validationModel;
        }
    }
}
