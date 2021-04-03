using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using VS_SLG6.Model;

namespace VS_SLG6.Repositories.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly VS_SLG6DbContext _context;

        public GenericRepository(VS_SLG6DbContext context)
        {
            _context = context;
        }

        public T Add(T obj)
        {
            _context.Add(obj);
            _context.SaveChanges();
            return obj;
        }

        public List<T> All()
        {
            return _context.Set<T>().ToList();
        }

        public bool Exists(T obj)
        {
            return _context.Set<T>().FirstOrDefault(x => x.Equals(obj)) != null;
        }

        public T FindOne(Expression<Func<T, bool>> condition)
        {
            return _context.Set<T>().FirstOrDefault(condition);
        }

        public List<T> FindAll(Expression<Func<T, bool>> condition)
        {
            return _context.Set<T>().Where(condition).ToList();
        }

        public T Remove(T obj)
        {
            _context.Set<T>().Remove(obj);
            _context.SaveChanges();
            return obj;
        }

        public T Update(T obj)
        {
            _context.Set<T>().Update(obj);
            _context.SaveChanges();
            return obj;
        }
    }
}
