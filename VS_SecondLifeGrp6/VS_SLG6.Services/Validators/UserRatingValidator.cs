﻿using System;
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
            _validationModel.Value = false;
            if (obj == null)
            {
                _validationModel.Errors.Add("Cannot add a null rating");
                return _validationModel;
            }
            // Check null fields
            if (obj.Origin == null || obj.Target == null)
            {
                _validationModel.Errors.Add("Rating object cannot have empty fields.");
                return _validationModel;
            }
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
            // Check if Rating already exists
            if (_repo.All(x => x.Origin.Id == obj.Origin.Id && x.Target.Id == obj.Target.Id).Count > 0)
            {
                _validationModel.Errors.Add("Rating on this Target already exists for this Origin.");
            }
            // Format Comment
            if (obj.Comment != null && StringIsEmptyOrBlank(obj, "Comment").Value) obj.Comment = null;
            else if (obj.Comment != null) obj.Comment = obj.Comment.Trim();
            _validationModel.Value = _validationModel.Errors.Count == 0;
            return _validationModel;
        }
    }
}
