using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;
using VS_SLG6.Services.Services;

namespace VS_SLG6.Services.Validators
{
    public class UserRatingValidator : GenericValidator<UserRating>, IValidator<UserRating>
    {
        private IRepository<User> _repoUser;

        public UserRatingValidator(IRepository<UserRating> repo, ValidationModel<bool> validationModel, IRepository<User> repoUser) : base(repo, validationModel) 
        {
            _repoUser = repoUser;
        }

        public override ValidationModel<bool> CanAdd(UserRating obj)
        {
            _validationModel = IsObjectValid(obj);
            if (!_validationModel.Value) return _validationModel;
            CheckUserAuthorization(obj.Origin.Id);
            if (!_validationModel.Value) return _validationModel;

            // Check if Rating already exists
            if (_repo.All(x => x.Origin.Id == obj.Origin.Id && x.Target.Id == obj.Target.Id).Count > 0)
            {
                _validationModel.Errors.Add("Rating on this Target already exists for this Origin.");
            }

            _validationModel.Value = _validationModel.Errors.Count == 0;
            return _validationModel;
        }

        public override ValidationModel<bool> CanEdit(UserRating obj)
        {
            _validationModel = IsObjectValid(obj);
            if (!_validationModel.Value) return _validationModel;
            CheckUserAuthorization(obj.Origin.Id);
            return _validationModel;
        }

        public override ValidationModel<bool> CanDelete(UserRating obj)
        {
            _validationModel = IsObjectValid(obj);
            if (!_validationModel.Value) return _validationModel;
            CheckUserAuthorization(obj.Origin.Id);
            return _validationModel;
        }

        public override ValidationModel<bool> IsObjectValid(UserRating obj)
        {
            var listProps = new List<string> { nameof(obj.Origin), nameof(obj.Target) };
            _constraintsObject = new ConstraintsObject
            {
                PropsNonNull = listProps
            };
            // Basic check on fields (null, blank, size)
            _validationModel = base.CanAdd(obj);
            if (!_validationModel.Value) return _validationModel;

            // Check stars between 1 and 5
            if (obj.Stars < 1 || obj.Stars > 5) _validationModel.Errors.Add("Rating Stars must be between 1 and 5.");

            // Check if Origin exists
            var o = _repoUser.FindOne(obj.Origin.Id);
            if (o == null) _validationModel.Errors.Add("Rating Origin doesn't exist.");
            else obj.Origin = o;

            // Check if Product exists
            var t = _repoUser.FindOne(obj.Target.Id);
            if (t == null) _validationModel.Errors.Add("Rating Target doesn't exist.");
            else obj.Target = t;

            // Format Comment (can be optional that's why we don't give it to parent function)
            if (obj.Comment != null && StringIsEmptyOrBlank(obj, "Comment").Value) obj.Comment = null;
            else if (obj.Comment != null) obj.Comment = obj.Comment.Trim();

            _validationModel.Value = _validationModel.Errors.Count == 0;
            return _validationModel;
        }
    }
}
