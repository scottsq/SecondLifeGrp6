using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using VS_SLG6.Repositories.Repositories;
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

        public ValidationModel<List<T>> List()
        {
            return new ValidationModel<List<T>> { Value = _repo.All() };
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

        public ValidationModel<T> Patch(int id, JsonPatchDocument<T> jsonPatch)
        {
            var vmObj = Get(id);
            if (vmObj.HasErrors || vmObj.Value == null) return vmObj;

            jsonPatch.ApplyTo(vmObj.Value);
            var vmRes = GetErrors<T>(_validator.CanEdit(vmObj.Value));
            if (!vmRes.HasErrors) vmRes.Value = _repo.Update(vmObj.Value);
            return vmRes;
        }
    }
}