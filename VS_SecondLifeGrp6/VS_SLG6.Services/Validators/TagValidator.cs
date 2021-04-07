using System.Collections.Generic;
using System.Linq;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;

namespace VS_SLG6.Services.Validators
{
    public class TagValidator : GenericValidator<Tag>, IValidator<Tag>
    {
        public TagValidator(IRepository<Tag> repo, ValidationModel<bool> validationModel) : base(repo, validationModel) { }

        public override ValidationModel<bool> CanAdd(Tag obj)
        {
            var listProps = new List<string> { nameof(obj.Name) };
            _constraintsObject = new ConstraintsObject
            {
                PropsNonNull = listProps,
                PropsStringNotBlank = listProps
            };
            // Basic check on fields (null, blank, size)
            _validationModel = base.CanAdd(obj);
            if (!_validationModel.Value) return _validationModel;

            // Check if already exists
            if (_repo.All(x => x.Name == obj.Name).Count > 0) _validationModel.Errors.Add("Tag with similar name already exists.");
            _validationModel.Value = _validationModel.Errors.Count == 0;
            return _validationModel;
        }
    }
}
