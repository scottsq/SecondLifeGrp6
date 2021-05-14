﻿using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using VS_SLG6.Services.Models;

namespace VS_SLG6.Services.Interfaces
{
    public interface IService<T> where T : class
    {
        ValidationModel<List<T>> List();
        ValidationModel<T> Get(int id);
        ValidationModel<T> Add(T obj);
        ValidationModel<T> Patch(T obj, JsonPatchDocument<T> jsonPatch);
        ValidationModel<T> Remove(T obj);
    }
}
