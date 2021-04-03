using System;
using System.Collections.Generic;
using System.Text;

namespace VS_SLG6.Services.Validators
{
    public interface IValidator<T> where T : class
    {
        bool canAdd(T obj);
        bool canEdit(T obj);
        bool canDelete(T obj);
    }
}
