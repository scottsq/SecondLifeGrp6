using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;

namespace VS_SLG6.Services.Validators
{
    public class UserValidator : GenericValidator<User>, IValidator<User>
    {
        

        public UserValidator(IRepository<User> repo, ValidationModel<bool> validationModel): base(repo, validationModel)
        {
        }

        public override ValidationModel<bool> CanAdd(User obj)
        {
            var listProps = new List<string> { nameof(obj.Login), nameof(obj.Password), nameof(obj.Email), nameof(obj.Name) };
            _constraintsObject = new ConstraintsObject
            {
                PropsNonNull = listProps,
                PropsStringNotBlank = listProps,
                PropsStringNotLongerThanMax = listProps.Where(x => x != nameof(obj.Email)).ToList()
            };

            // Basic check on fields (null, blank, size)
            _validationModel = base.CanAdd(obj);
            if (!_validationModel.Value) return _validationModel;
            
            // Check Email
            var splittedMail = obj.Email.Split('@');
            if (obj.Email.Contains("..") || splittedMail.Length < 2 || splittedMail[0].Trim().Length == 0 || splittedMail[1].Trim().Length == 0 || splittedMail[1].Trim().Split('.').Length < 2)
            {
                _validationModel.Errors.Add("User Email is invalid.");
            }

            // Check if exists
            if (_repo.All(x => x.Login == obj.Login).Count > 0)
            {
                _validationModel.Errors.Add("User with this Login already exists");
            }
            _validationModel.Value = _validationModel.Errors.Count == 0;
            return _validationModel;
        }
    }
}
