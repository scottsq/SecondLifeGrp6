using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using VS_SLG6.Services.Models;

namespace VS_SLG6.Services.Services
{
    public interface IService<T> where T : class
    {
        ValidationModel<List<T>> List();
        ValidationModel<T> Get(int id);
        ValidationModel<T> Add(T obj);
        ValidationModel<T> Patch(int id, JsonPatchDocument<T> jsonPatch);
        ValidationModel<T> Remove(T obj);
        void SetContextUser(ContextUser cUser);
    }
}
