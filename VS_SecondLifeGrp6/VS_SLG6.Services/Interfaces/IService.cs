using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VS_SLG6.Services.Models;

namespace VS_SLG6.Services.Interfaces
{
    public interface IService<T> where T : class
    {
        List<T> Find(Expression<Func<T, bool>> condition = null, string orderBy = null, bool reverse = false, int from = 0, int max = 10);
        ValidationModel<T> Get(int id);
        ValidationModel<T> Add(T obj);
        ValidationModel<T> Patch(T obj, JsonPatchDocument<T> jsonPatch);
        ValidationModel<T> Remove(T obj);
    }
}
