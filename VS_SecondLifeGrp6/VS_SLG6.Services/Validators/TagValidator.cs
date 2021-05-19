using System.Collections.Generic;
using System.Linq;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;

namespace VS_SLG6.Services.Validators
{
    public class TagValidator : GenericValidator<Tag>, IValidator<Tag>
    {
        public TagValidator(IRepository<Tag> repo) : base(repo) { }

        public override List<string> CanAdd(Tag obj)
        {
            var listErrors = IsObjectValid(obj);
            if (listErrors.Any()) return listErrors;

            // Check if already exists
            IsObjectExisting(listErrors, x => x.Name == obj.Name);

            return listErrors;
        }

        public override List<string> IsObjectValid(Tag obj, ConstraintsObject constraintsObject = null)
        {
            var listProps = new List<string> { nameof(Tag.Name) };
            constraintsObject = new ConstraintsObject
            {
                FieldsNotNull = listProps,
                FieldsStringNotBlank = listProps
            };
            // Basic check on fields (null, blank, size)
            var listErrors = base.IsObjectValid(obj, constraintsObject);
            return listErrors;
        }
    }
}
