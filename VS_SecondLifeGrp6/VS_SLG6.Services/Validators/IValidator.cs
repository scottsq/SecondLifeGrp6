using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Models;

namespace VS_SLG6.Services.Validators
{
    public interface IValidator<T> where T : class
    {
        public List<string> CanGet(T obj);
        public List<string> CanAdd(T obj);
        public List<string> CanEdit(T obj);
        public List<string> CanDelete(T obj);
        public List<string> IsObjectValid(T obj, ConstraintsObject constraintsObject);
    }
}
