using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace VS_SLG6.Repositories.Repositories
{
    public interface IRepository<T> where T : class
    {
        T Add(T obj);
        T Update(T obj);
        T Remove(T obj);
        bool Exists(T obj);
        T FindOne(params object[] values);
        List<T> All(Expression<Func<T, bool>> condition = null, int from = 0, int max = 10);
        T FindOne(Expression<Func<T, bool>> condition = null);
    }
}
