﻿using Microsoft.AspNetCore.JsonPatch;
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

        public GenericService(IRepository<T> repo, IValidator<T> validator)
        {
            _repo = repo;
            _validator = validator;
            _validationModel = new Models.ValidationModel<T>();
        }

        public List<T> List()
        {
            return _repo.All();
        }

        public virtual T Get(int id)
        {
            return _repo.FindOne(id);
        }

        public virtual ValidationModel<T> Add(T obj)
        {
            var v = _validator.CanAdd(obj);
            if (v.Value) _validationModel.Value = _repo.Add(obj);
            else
            {
                _validationModel.Value = obj;
                _validationModel.Errors = _validationModel.Errors.Concat(v.Errors).ToList();
            }
            return _validationModel;
        }

        public T Remove(T obj)
        {
            _repo.Remove(obj);
            return obj;
        }

        public T Patch(int id, JsonPatchDocument<T> jsonPatch)
        {
            var obj = Get(id);
            jsonPatch.ApplyTo(obj);
            _repo.Update(obj);
            return obj;
        }
    }
}