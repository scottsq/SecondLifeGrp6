using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;
using VS_SLG6.Services.Validators;

namespace VS_SLG6.Services.Services
{
    public class GenericService<T> : IService<T> where T : class
    {
        protected IRepository<T> _repo;
        protected IValidator<T> _validator;
        protected Models.ValidationModel<T> _validationModel;
        public static ContextUser contextUser = null;

        public GenericService(IRepository<T> repo, IValidator<T> validator)
        {
            _repo = repo;
            _validator = validator;
            _validationModel = new Models.ValidationModel<T>();
        }

        public ValidationModel<List<T>> List()
        {
            var res = new ValidationModel<List<T>>();
            var check = _validator.CanGet(Roles.USER);
            if (check.Value) res.Value = _repo.All();
            else res.Errors = check.Errors;
            return res;
        }

        public virtual ValidationModel<T> Get(int id)
        {
            var check = _validator.CanGet(Roles.USER);
            if (check.Value) _validationModel.Value = _repo.FindOne(id);
            else _validationModel.Errors = check.Errors;
            return _validationModel;
        }

        public virtual ValidationModel<T> Add(T obj)
        {
            var check = _validator.CanAdd(obj);
            if (check.Value) _validationModel.Value = _repo.Add(obj);
            else _validationModel.Errors = check.Errors; 
            return _validationModel;
        }

        public ValidationModel<T> Remove(T obj)
        {
            var v = _validator.CanDelete(obj);
            if (v.Value)
            {
                _validationModel.Value = obj;
                _repo.Remove(obj);
            }
            else _validationModel.Errors = v.Errors;
            return _validationModel;
        }

        public ValidationModel<T> Patch(int id, JsonPatchDocument<T> jsonPatch)
        {
            var obj = Get(id);
            if (obj.Value == null) return _validationModel;

            jsonPatch.ApplyTo(obj.Value);
            var v = _validator.CanEdit(obj.Value);
            if (v.Value)
            {
                _repo.Update(obj.Value);
                _validationModel.Value = obj.Value;
            }
            else _validationModel.Errors = v.Errors;
            return _validationModel;
        }

        public void SetContextUser(ContextUser cUser)
        {
            contextUser = cUser;
        }
    }
}