using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;

namespace VS_SLG6.Services.Validators
{
    public class GenericValidator<T> : IValidator<T> where T : class
    {
        protected readonly IRepository<T> _repo;

        protected static String FieldNullError = "{0} {1} cannot be null";
        protected static String FieldEmptyError = "{0} {1} cannot be empty.";
        protected static int StringMaxLength = 64;
        protected static String CharCountError = "{0} {1} exceeds limit of " + StringMaxLength.ToString() + " characters.";
        protected static String CannotPerformActionError = "This user cannot perform this action.";
        protected static String ExistingError = "{0} with identical properties already exists.";
            
        public GenericValidator(IRepository<T> repo)
        {
            _repo = repo;
        }

        public virtual List<string> CanGet(T obj)
        {
            var list = new List<string>();
            if (obj == null) list.Add("Cannot get null " + nameof(T));
            return list;
        }

        public virtual List<string> CanAdd(T obj)
        {
            return IsObjectValid(obj, null);
        }

        public virtual List<string> CanDelete(T obj)
        {
            var list = new List<string>();
            if (obj == null) list.Add("Cannot remove null " + nameof(T));
            return list;
        }

        public virtual List<string> CanEdit(T obj)
        {
            return IsObjectValid(obj, null);
        }

        public virtual List<string> IsObjectValid(T obj, ConstraintsObject constraintsObject)
        {
            var listErrors = new List<string>();
            // 1. Null case
            if (obj == null)
            {
                listErrors.Add("Cannot add null " + nameof(T));
                return listErrors;
            }
            if (constraintsObject == null) return listErrors;

            var properties = new List<PropertyInfo>(obj.GetType().GetProperties());
            // 2. Null fields
            var errorList = new List<string>();
            foreach (var field in constraintsObject.FieldsNotNull)
            {
                if (properties.FirstOrDefault(x => x.Name == field).GetValue(obj) == null) errorList.Add(field);

            }
            if (errorList.Count > 0) AppendFormattedErrors(listErrors, errorList, FieldNullError);

            // 3.1 Empty strings
            var check = StringHelper.StringIsEmptyOrBlank(obj, constraintsObject.FieldsStringNotBlank.ToArray());
            if (check.Errors.Count > 0) AppendFormattedErrors(listErrors, check.Errors, FieldEmptyError);

            // 3.2 Too long strings
            check = StringHelper.StringIsLongerThanMax(obj, StringMaxLength, constraintsObject.FieldsStringNotLongerThanMax.ToArray());
            if (check.Errors.Count > 0) AppendFormattedErrors(listErrors, check.Errors, CharCountError);

            return listErrors;
        }

        public void IsObjectExisting(List<string> listErrors, Expression<Func<T, bool>> condition)
        {
            if (_repo.All(condition).Any()) listErrors.Add(String.Format(ExistingError, typeof(T).Name));
        }

        public void AppendFormattedErrors(List<string> listErrors, List<string> list, string error)
        {
            for (int i = 0; i < list.Count; i++)
            {
                listErrors.Add(String.Format(error, typeof(T).Name , list[i]));
            }
        }

        
    }
}
