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
        private const int MaxLength = 32;
        private const String FieldEmptyError = "User {0} cannot be empty.";
        private String CharCountError = "User {0} exceeds limit of " + MaxLength.ToString() + " characters.";

        public UserValidator(IRepository<User> repo, ValidationModel<bool> validationModel): base(repo, validationModel)
        {
        }

        public override ValidationModel<bool> CanAdd(User obj)
        {
            _validationModel.Value = false;
            // Check null cases
            if (obj == null) {
                _validationModel.Errors.Add("Cannot add a null User.");
                return _validationModel;
            }
            if (obj.Login == null || obj.Password == null || obj.Email == null || obj.Name == null)
            {
                _validationModel.Errors.Add("Cannot add User with null fields.");
                return _validationModel;
            }

            // Format fields
            obj.Login = obj.Login.Trim();
            obj.Password = obj.Password.Trim();
            obj.Email = obj.Email.Trim();
            obj.Name = obj.Name.Trim();

            // Check Login, Password and Name as they have same constraints + Email for 
            var check = StringIsEmptyOrBlank(obj, "Login", "Password", "Name", "Email");
            if (check.Errors.Count > 0) AppendFormattedErrors(check.Errors, FieldEmptyError);
            check = StringIsLongerThanMax(obj, MaxLength, "Login", "Password", "Name");
            if (check.Errors.Count > 0) AppendFormattedErrors(check.Errors, CharCountError);

            // Check Email
            var expression = @"^([a-zA-Z0-9]{1,}(\\.{1}[a-zA-Z0-9]{1,}){0,}@[a-zA-Z0-9]{1,}\\.{1}[a-zA-Z0-9]{1,})$";
            var splittedMail = obj.Email.Split('@');
            if (obj.Email.Contains("..") || splittedMail.Length < 2 || splittedMail[0].Trim().Length == 0 || splittedMail[1].Trim().Length == 0 || splittedMail[1].Trim().Split('.').Length < 2)
            {
                _validationModel.Errors.Add("User email is invalid.");
            }

            // Check if exists
            if (_repo.FindAll(x => x.Login == obj.Login).Count > 0)
            {
                _validationModel.Errors.Add("User with this login already exists");
            }
            _validationModel.Value = _validationModel.Errors.Count == 0;
            return _validationModel;
        }
    }
}
