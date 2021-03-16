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
            _validationModel.Value = false;
            if (obj == null)
            {
                _validationModel.Errors.Add("Cannot add null Tag.");
                return _validationModel;
            }
            if (obj.Name == null)
            {
                _validationModel.Errors.Add("Tag object cannot have null fields.");
                return _validationModel;
            }
            // Check Name
            var check = StringIsEmptyOrBlank(obj, "Name");
            if (!check.Value) AppendFormattedErrors(check.Errors, "Tag {0} cannot be blank.");
            // Check if already exists
            if (_repo.FindAll(x => x.Name == obj.Name).Count > 0) _validationModel.Errors.Add("Tag with similar name already exists.");
            _validationModel.Value = _validationModel.Errors.Count == 0;
            return _validationModel;
        }
    }
}
