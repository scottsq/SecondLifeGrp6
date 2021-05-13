using System.Collections.Generic;
using System.Linq;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;

namespace VS_SLG6.Services.Validators
{
    public class ProductRatingValidator : GenericValidator<ProductRating>, IValidator<ProductRating>
    {
        private IRepository<Product> _repoProduct;
        private IRepository<User> _repoUser;

        public ProductRatingValidator(IRepository<ProductRating> repo, IRepository<Product> repoProduct, IRepository<User> repoUser) : base(repo) 
        {
            _repoProduct = repoProduct;
            _repoUser = repoUser;
        }

        public override List<string> CanAdd(ProductRating obj)
        {
            var listErrors = IsObjectValid(obj);
            if (listErrors.Any()) return listErrors;

            // Check if Rating already exists
            IsObjectExisting(listErrors, x => x.Product.Id == obj.Product.Id && x.User.Id == obj.User.Id);
            return listErrors;
        }

        public override List<string> IsObjectValid(ProductRating obj, ConstraintsObject constraintsObject = null)
        {
            constraintsObject = new ConstraintsObject
            {
                FieldsNotNull = new List<string> { nameof(obj.User), nameof(obj.Product) }
            };

            // Basic check on fields (null, blank, size)
            var listErrors = base.IsObjectValid(obj, constraintsObject);
            if (listErrors.Any()) return listErrors;

            // Check stars between 1 and 5
            if (obj.Stars < 1 || obj.Stars > 5) listErrors.Add("Rating Stars must be between 1 and 5.");

            // Check if User exists
            var u = _repoUser.FindOne(obj.User.Id);
            if (u == null) listErrors.Add("Rating User doesn't exist.");
            else obj.User = u;

            // Check if Product exists
            var p = _repoProduct.FindOne(obj.Product.Id);
            if (p == null) listErrors.Add("Rating Product doesn't exist.");
            else obj.Product = p;

            // Format Comment (can be optional that's why we don't check it in parent)
            if (obj.Comment != null && StringIsEmptyOrBlank(obj, "Comment").Value) obj.Comment = null;
            else if (obj.Comment != null) obj.Comment = obj.Comment.Trim();

            return listErrors;
        }
    }
}
