using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Validators;

namespace VS_SLG6.Services.Services
{
    public class ProductService : GenericService<Product>, IService<Product>
    {
        public ProductService(IRepository<Product> repo, IValidator<Product> validator) : base(repo, validator)
        {
        }

        public List<Product> GetLatest()
        {
            Comparison<Product> c = new Comparison<Product>((a, b) =>
            {
                return a.CreationDate - b.CreationDate > new TimeSpan(0) ? -1 : a.CreationDate == b.CreationDate ? 0 : 1;
            });
            var list = _repo.All();
            list.Sort(c);
            return list.GetRange(0, Math.Min(10, list.Count));
        }
    }
}
