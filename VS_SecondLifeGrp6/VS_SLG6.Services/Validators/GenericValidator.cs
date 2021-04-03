using System;
using System.Collections.Generic;
using System.Text;

namespace VS_SLG6.Services.Validators
{
    public class GenericValidator<T> : IValidator<T> where T : class
    {
        public bool canAdd(T obj)
        {
            return true;
        }

        public bool canDelete(T obj)
        {
            throw new NotImplementedException();
        }

        public bool canEdit(T obj)
        {
            throw new NotImplementedException();
        }
    }
}
