using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VS_SLG6.Model;
using Microsoft.EntityFrameworkCore;

namespace VS_SLG6.Repositories.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly VS_SLG6DbContext _context;
        protected readonly IQueryable<T> _contextWithIncludes;
        protected virtual List<string> _includes { get; } = new List<string>();

        public GenericRepository(VS_SLG6DbContext context)
        {
            _context = context;
            _contextWithIncludes = _context.Set<T>().AsQueryable();
            foreach (var item in _includes)
            {
                _contextWithIncludes = _contextWithIncludes.Include(item);
            }
        }

        public T Add(T obj)
        {
            _context.Add(obj);
            _context.SaveChanges();
            return obj;
        }

        public bool Exists(T obj)
        {
            return _context.Set<T>().FirstOrDefault(x => x.Equals(obj)) != null;
        }

        public virtual T FindOne(params object[] values)
        {
            return _context.Set<T>().Find(values);
        }

        public List<T> All(Expression<Func<T, bool>> condition=null, Func<T, object> orderBy=null, bool reverse=false, int from=0, int max=10)
        {
            condition ??= x => true;
            var list = _contextWithIncludes.Where(condition).ToList();
            if (orderBy != null && reverse) list = list.OrderByDescending(orderBy).ToList();
            else if (orderBy != null) list = list.OrderBy(orderBy).ToList();

            from = Math.Max(from, 0);
            if (from >= list.Count) return new List<T>();

            max = Math.Min(Math.Max(max, 1), list.Count);
            return list.Skip(from).Take(max).ToList();
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

        public T FindOne(Expression<Func<T, bool>> condition = null)
        {
            return _contextWithIncludes.FirstOrDefault(condition);
        }
    }

    public interface Entity
    {
        int Id { get; set; }
    }
}
