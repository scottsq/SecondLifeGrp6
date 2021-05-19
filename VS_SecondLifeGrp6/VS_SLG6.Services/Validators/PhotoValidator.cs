using System.Collections.Generic;
using System.Linq;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;

namespace VS_SLG6.Services.Validators
{
    public class PhotoValidator : GenericValidator<Photo>, IValidator<Photo>
    {
        private IRepository<Product> _repoProduct;

        public PhotoValidator(IRepository<Photo> repo, IRepository<Product> repoProduct): base(repo)
        {
            _repoProduct = repoProduct;
        }

        public override List<string> CanAdd(Photo obj)
        {
            var listErrors = IsObjectValid(obj);
            if (listErrors.Any()) return listErrors;

            // check if already exists
            IsObjectExisting(listErrors, x => x.Product.Id == obj.Product.Id);
            return listErrors;
        }

        public override List<string> IsObjectValid(Photo obj, ConstraintsObject constraintsObject = null)
        {
            var listProps = new List<string> { nameof(Photo.Product), nameof(Photo.Url) };
            constraintsObject = new ConstraintsObject
            {
                FieldsNotNull = listProps,
                FieldsStringNotBlank = listProps.Where(x => x == nameof(obj.Url)).ToList()
            };

            // Basic check on fields (null, blank, size)
            var listErrors = base.IsObjectValid(obj, constraintsObject);
            if (listErrors.Any()) return listErrors;

            // check product
            var p = _repoProduct.FindOne(obj.Product.Id);
            if (p == null) listErrors.Add("Unknown Product.");
            else obj.Product = p;
            return listErrors;
        }
    }
}
