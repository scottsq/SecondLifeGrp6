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
    public class PhotoValidator : GenericValidator<Photo>, IValidator<Photo>
    {
        private IRepository<Product> _repoProduct;

        public PhotoValidator(IRepository<Photo> repo, ValidationModel<bool> validationModel, IRepository<Product> repoProduct): base(repo, validationModel)
        {
            _repoProduct = repoProduct;
        }

        public override ValidationModel<bool> CanAdd(Photo obj)
        {
            _validationModel = IsObjectValid(obj);
            if (!_validationModel.Value) return _validationModel;
            CheckUserAuthorization(obj.Product.Owner.Id);
            if (!_validationModel.Value) return _validationModel;

            // check if already exists
            if (_repo.All(x => x.Product.Id == obj.Product.Id && x.Url == obj.Url).Count > 0)
            {
                _validationModel.Errors.Add("A Photo with this url already exists for this product.");
            }

            _validationModel.Value = _validationModel.Errors.Count == 0;
            return _validationModel;
        }

        public override ValidationModel<bool> CanEdit(Photo obj)
        {
            _validationModel = IsObjectValid(obj);
            if (!_validationModel.Value) return _validationModel;
            CheckUserAuthorization(obj.Product.Owner.Id);
            return _validationModel;
        }

        public override ValidationModel<bool> CanDelete(Photo obj)
        {
            _validationModel = IsObjectValid(obj);
            if (!_validationModel.Value) return _validationModel;
            CheckUserAuthorization(obj.Product.Owner.Id);
            _validationModel.Value = _validationModel.Errors.Count == 0;
            return _validationModel;
        }

        public override ValidationModel<bool> IsObjectValid(Photo obj)
        {
            var listProps = new List<string> { nameof(obj.Product), nameof(obj.Url) };
            _constraintsObject = new ConstraintsObject
            {
                PropsNonNull = listProps,
                PropsStringNotBlank = listProps.Where(x => x == nameof(obj.Url)).ToList()
            };

            // Basic check on fields (null, blank, size)
            _validationModel = base.IsObjectValid(obj);
            if (!_validationModel.Value) return _validationModel;

            // check product
            var p = _repoProduct.FindOne(obj.Product.Id);
            if (p == null) _validationModel.Errors.Add("Unknown Product.");
            else obj.Product = p;
           
            _validationModel.Value = _validationModel.Errors.Count == 0;
            return _validationModel;
        }
    }
}
