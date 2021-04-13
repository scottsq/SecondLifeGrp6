using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;
using VS_SLG6.Services.Services;

namespace VS_SLG6.Services.Validators
{
    public class GenericValidator<T> : IValidator<T> where T : class
    {
        protected readonly IRepository<T> _repo;
        protected ValidationModel<bool> _validationModel;
        protected ConstraintsObject _constraintsObject = new ConstraintsObject();

        protected static String FieldNullError = "{0} {1} cannot be null";
        protected static String FieldEmptyError = "{0} {1} cannot be empty.";
        protected static int StringMaxLength = 64;
        protected static String CharCountError = "{0} {1} exceeds limit of " + StringMaxLength.ToString() + " characters.";
        protected static String CannotPerformActionError = "This user cannot perform this action.";

        public GenericValidator(IRepository<T> repo, ValidationModel<bool> validationModel)
        {
            _repo = repo;
            _validationModel = validationModel;
        }

        public virtual ValidationModel<bool> CanGet(T obj)
        {
            return new ValidationModel<bool> { Value = false };
        }

        public virtual ValidationModel<bool> CanGet(Roles role)
        {
            CheckRoleAuthorization(role);
            return _validationModel;
        }

        public virtual ValidationModel<bool> CanAdd(T obj)
        {
            return IsObjectValid(obj);
        }

        public virtual ValidationModel<bool> CanDelete(T obj)
        {
            _validationModel.Value = false;
            return _validationModel;
        }

        public virtual ValidationModel<bool> CanEdit(T obj)
        {
            return IsObjectValid(obj);
        }

        public virtual ValidationModel<bool> IsObjectValid(T obj)
        {
            _validationModel.Value = false;
            // 1. Null case
            if (obj == null)
            {
                _validationModel.Errors.Add("Cannot add null " + nameof(T));
                return _validationModel;
            }

            var properties = new List<PropertyInfo>(obj.GetType().GetProperties());
            // 2. Null fields
            var errorList = new List<string>();
            foreach (var field in _constraintsObject.PropsNonNull)
            {
                if (properties.FirstOrDefault(x => x.Name == field).GetValue(obj) == null) errorList.Add(field);

            }
            if (errorList.Count > 0)
            {
                AppendFormattedErrors(errorList, FieldNullError);
            }

            // 3.1 Empty strings
            var check = StringIsEmptyOrBlank(obj, _constraintsObject.PropsStringNotBlank.ToArray());
            if (check.Errors.Count > 0) AppendFormattedErrors(check.Errors, FieldEmptyError);

            // 3.2 Too long strings
            check = StringIsLongerThanMax(obj, StringMaxLength, _constraintsObject.PropsStringNotLongerThanMax.ToArray());
            if (check.Errors.Count > 0) AppendFormattedErrors(check.Errors, CharCountError);


            _validationModel.Value = _validationModel.Errors.Count == 0;
            return _validationModel;
        }

        public virtual void CheckUserAuthorization(int id)
        {
            var cUser = GenericService<T>.contextUser;
            if (cUser != null && id != cUser.Id && cUser.Role != Roles.ADMIN)
            {
                _validationModel.Value = false;
                _validationModel.Errors.Add(CannotPerformActionError);
            }
            _validationModel.Value = true;
        }
        public virtual void CheckRoleAuthorization(Roles role)
        {
            var cUser = GenericService<T>.contextUser;
            if (cUser != null && cUser.Role != role && cUser.Role != Roles.ADMIN)
            {
                _validationModel.Value = false;
                _validationModel.Errors.Add(CannotPerformActionError);
            }
            _validationModel.Value = true;
        }






        public ValidationModel<bool> StringIsEmptyOrBlank(T obj, params string[] properties)
        {
            var res = new ValidationModel<bool>();
            res.Value = false;
            var list = GetPropsValues(obj, properties);
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != null && Regex.Replace(list[i], " +", "").Length == 0)
                {
                    res.Value = true;
                    res.Errors.Add(properties[i]);
                }
            }
            return res;
        }

        public ValidationModel<bool> StringIsLongerThanMax(T obj, int max, params string[] properties)
        {
            var res = new ValidationModel<bool>();
            res.Value = false;
            var list = GetPropsValues(obj, properties);
            for (int i = 0; i < list.Count; i++) 
            {
                if (list[i] != null && list[i].Length > max)
                {
                    res.Value = true;
                    res.Errors.Add(properties[i]);
                }
            }
            return res;
        }

        public void AppendFormattedErrors(List<string> list, string error)
        {
            for (int i = 0; i < list.Count; i++)
            {
                _validationModel.Errors.Add(String.Format(error, typeof(T).Name , list[i]));
            }
        }

        public List<string> GetPropsValues(T obj, params string[] properties)
        {
            var props = new List<PropertyInfo>(obj.GetType().GetProperties());
            return props.Aggregate(new List<string>(), (acc, item) =>
            {
                if (properties.Contains(item.Name)) acc.Add(item.GetValue(obj, null).ToString());
                return acc;
            });
        }

    }
}
