using System;
using System.Collections.Generic;
using System.Linq;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;

namespace VS_SLG6.Services.Validators
{
    public class ProductValidator : GenericValidator<Product>, IValidator<Product>
    {
        private IRepository<User> _repoUser;

        public ProductValidator(IRepository<Product> repo, IRepository<User> repoUser) : base(repo) 
        {
            _repoUser = repoUser;
        }

        public override List<string> CanAdd(Product obj)
        {
            var listErrors = IsObjectValid(obj);
            if (listErrors.Any()) return listErrors;

            IsObjectExisting(listErrors, x => x.Name == obj.Name && x.Price == obj.Price && x.Owner.Id == obj.Owner.Id);
            return listErrors;
        }

        public override List<string> IsObjectValid(Product obj, ConstraintsObject constraintsObject = null)
        {
            var listProps = new List<string> { nameof(obj.Description), nameof(obj.Name), nameof(obj.Owner) };
            constraintsObject = new ConstraintsObject
            {
                FieldsNotNull = listProps,
                FieldsStringNotBlank = listProps.Where(x => x != nameof(obj.Owner)).ToList()
            };
            // Basic check on fields (null, blank, size)
            var listErrors = base.IsObjectValid(obj, constraintsObject);
            if (listErrors.Any()) return listErrors;

            // Check negative price
            if (obj.Price < 0) listErrors.Add("Product price cannot be negative.");

            // Check if owner exists
            var u = _repoUser.FindOne(obj.Owner.Id);
            if (u == null) listErrors.Add("Product Owner doesn't exist.");
            else obj.Owner = u;

            // Format date
            if (obj.CreationDate == DateTime.MinValue) obj.CreationDate = DateTime.Now;

            return listErrors;
        }
    }
}
