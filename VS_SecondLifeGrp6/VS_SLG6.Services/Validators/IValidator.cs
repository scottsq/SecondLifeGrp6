using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Services.Models;

namespace VS_SLG6.Services.Validators
{
    public interface IValidator<T> where T : class
    {
        public ValidationModel<bool> CanAdd(T obj);
        public ValidationModel<bool> CanEdit(T obj);
        public ValidationModel<bool> CanDelete(T obj);
        public ValidationModel<bool> IsObjectValid(T obj);
    }
}
