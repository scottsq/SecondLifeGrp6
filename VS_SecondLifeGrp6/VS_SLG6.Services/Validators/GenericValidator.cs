using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;

namespace VS_SLG6.Services.Validators
{
    public class GenericValidator<T> : IValidator<T> where T : class
    {
        protected readonly IRepository<T> _repo;
        protected ValidationModel<bool> _validationModel;

        public GenericValidator(IRepository<T> repo, ValidationModel<bool> validationModel)
        {
            _repo = repo;
            _validationModel = validationModel;
        }

        public virtual ValidationModel<bool> CanAdd(T obj)
        {
            _validationModel.Value = false;
            if (obj == null)
            {
                _validationModel.Errors.Add("Cannot add null " + obj.GetType().Name);
                return _validationModel;
            }
            _validationModel.Value = _validationModel.Errors.Count == 0;
            return _validationModel;
        }

        public virtual ValidationModel<bool> CanDelete(T obj)
        {
            _validationModel.Value = true;
            return _validationModel;
        }

        public virtual ValidationModel<bool> CanEdit(T obj)
        {
            _validationModel.Value = true;
            return _validationModel;
        }

        public ValidationModel<bool> StringIsEmptyOrBlank(T obj, params string[] properties)
        {
            var res = new ValidationModel<bool>();
            res.Value = false;
            var list = GetPropsValues(obj, properties);
            for (int i = 0; i < list.Count; i++)
            {
                if (Regex.Replace(list[i], " +", "").Length == 0)
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
                if (list[i].Length > max)
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
                _validationModel.Errors.Add(String.Format(error, list[i]));
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
