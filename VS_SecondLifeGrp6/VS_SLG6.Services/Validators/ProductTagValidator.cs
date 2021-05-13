using System.Collections.Generic;
using System.Linq;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;

namespace VS_SLG6.Services.Validators
{
    public class ProductTagValidator : GenericValidator<ProductTag>, IValidator<ProductTag>
    {
        private IRepository<Product> _repoProduct;
        private IRepository<Tag> _repoTag;

        public ProductTagValidator(IRepository<ProductTag> repo, IRepository<Product> repoProduct, IRepository<Tag> repoTag) : base(repo) 
        {
            _repoProduct = repoProduct;
            _repoTag = repoTag;
        }

        public override List<string> CanAdd(ProductTag obj)
        {
            var listErrors = IsObjectValid(obj);
            if (listErrors.Any()) return listErrors;

            // Check if already exists
            IsObjectExisting(listErrors, x => x.Tag.Id == obj.Tag.Id && x.Product.Id == obj.Product.Id);
            return listErrors;
        }

        public override List<string> IsObjectValid(ProductTag obj, ConstraintsObject constraintsObject = null)
        {
            constraintsObject = new ConstraintsObject
            {
                FieldsNotNull = new List<string> { nameof(obj.Product), nameof(obj.Tag) }
            };

            // Basic check on fields (null, blank, size)
            var listErrors = base.IsObjectValid(obj, constraintsObject);
            if (listErrors.Any()) return listErrors;

            // Check if Tag exists
            var t = _repoTag.FindOne(obj.Tag.Id);
            if (t == null) listErrors.Add("ProductTag Tag doesn't exist.");
            else obj.Tag = t;

            // Check if Product exists
            var p = _repoProduct.FindOne(obj.Product.Id);
            if (p == null) listErrors.Add("ProductTag Product doesn't exist.");
            else obj.Product = p;

            return listErrors;
        }
    }
}
