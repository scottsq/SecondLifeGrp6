using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace VS_SLG6.Repositories.Repositories
{
    public interface IRepository<T> where T : class
    {
        List<T> All();
        T Add(T obj);
        T Update(T obj);
        T Remove(T obj);
        bool Exists(T obj);
        T FindOne(Expression<Func<T, bool>> condition);
    }
}
