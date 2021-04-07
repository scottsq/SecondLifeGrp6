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
    public class ProductRatingValidator : GenericValidator<ProductRating>, IValidator<ProductRating>
    {
        private IRepository<Product> _repoProduct;
        private IRepository<User> _repoUser;

        public ProductRatingValidator(IRepository<ProductRating> repo, ValidationModel<bool> validationModel, IRepository<Product> repoProduct, IRepository<User> repoUser) : base(repo, validationModel) 
        {
            _repoProduct = repoProduct;
            _repoUser = repoUser;
        }

        public override ValidationModel<bool> CanAdd(ProductRating obj)
        {
            _constraintsObject = new ConstraintsObject
            {
                PropsNonNull = new List<string> { nameof(obj.User), nameof(obj.Product) }
            };

            // Basic check on fields (null, blank, size)
            _validationModel = base.CanAdd(obj);
            if (!_validationModel.Value) return _validationModel;

            // Check stars between 1 and 5
            if (obj.Stars < 1 || obj.Stars > 5) _validationModel.Errors.Add("Rating Stars must be between 1 and 5.");

            // Check if User exists
            var u = _repoUser.FindOne(obj.User.Id);
            if (u == null) _validationModel.Errors.Add("Rating User doesn't exist.");
            else obj.User = u;

            // Check if Product exists
            var p = _repoProduct.FindOne(obj.Product.Id);
            if (p == null) _validationModel.Errors.Add("Rating Product doesn't exist.");
            else obj.Product = p;

            // Check if Rating already exists
            if (_repo.All(x => x.Product.Id == obj.Product.Id && x.User.Id == obj.User.Id).Count > 0)
            {
                _validationModel.Errors.Add("Rating on this Product already exists for this User.");
            }

            // Format Comment (can be optional that's why we don't check it in parent)
            if (obj.Comment != null && StringIsEmptyOrBlank(obj, "Comment").Value) obj.Comment = null;
            else if (obj.Comment != null) obj.Comment = obj.Comment.Trim();

            _validationModel.Value = _validationModel.Errors.Count == 0;
            return _validationModel;
        }
    }
}
