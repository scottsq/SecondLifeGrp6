using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Interfaces;
using VS_SLG6.Services.Models;
using VS_SLG6.Services.Validators;

namespace VS_SLG6.Services.Services
{
    public class GenericService<T> : IService<T> where T : class
    {
        protected IRepository<T> _repo;
        protected IValidator<T> _validator;

        public GenericService(IRepository<T> repo, IValidator<T> validator)
        {
            _repo = repo;
            _validator = validator;
        }

        protected ValidationModel<X> GetErrors<X>(List<string> values)
        {
            var vm = new ValidationModel<X>();
            vm.Errors.AddRange(values);
            return vm;
        }

        public virtual List<T> Find(Expression<Func<T, bool>> condition = null, string orderBy = null, bool reverse = false, int from = 0, int max = 10)
        {
            return _repo.All(condition, GenerateOrderByCondition(orderBy), reverse, from, max);
        }

        public virtual ValidationModel<T> Get(int id)
        {
            return new ValidationModel<T> { Value = _repo.FindOne(id) };
        }

        public virtual ValidationModel<T> Add(T obj)
        {
            var vm = GetErrors<T>(_validator.CanAdd(obj));
            if (!vm.HasErrors) vm.Value = _repo.Add(obj);
            return vm;
        }

        public virtual ValidationModel<T> Remove(T obj)
        {
            var vm = GetErrors<T>(_validator.CanDelete(obj));
            if (!vm.HasErrors) _repo.Remove(obj);
            return vm;
        }

        public ValidationModel<T> Patch(T obj, JsonPatchDocument<T> jsonPatch)
        {
            jsonPatch.ApplyTo(obj);
            var vmRes = GetErrors<T>(_validator.CanEdit(obj));
            if (!vmRes.HasErrors) vmRes.Value = _repo.Update(obj);
            return vmRes;
        }

        public Func<T, object> GenerateOrderByCondition(string propName)
        {
            var prop = typeof(T).GetProperties().Where(x => x.Name == propName).FirstOrDefault();
            if (prop == null) return null;
            if (!prop.PropertyType.IsPrimitive && prop.PropertyType != typeof(DateTime)) return null;
            return x => prop.GetValue(x);
        }
    }
}