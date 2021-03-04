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
        List<T> List();
        T Get(int id);
        ValidationModel<T> Add(T obj);
        T Patch(int id, JsonPatchDocument<T> jsonPatch);
        T Remove(T obj);
    }
}
