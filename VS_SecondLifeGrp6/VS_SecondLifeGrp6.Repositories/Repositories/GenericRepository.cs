using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using VS_SLG6.Model;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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

        public virtual T FindOne(int id)
        {
            return _context.Set<T>().Find(new object[] { id });
        }

        public List<T> All(Expression<Func<T, bool>> condition = null)
        {
            if (condition == null) condition = x => true;
            return _contextWithIncludes.Where(condition).ToList();
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
