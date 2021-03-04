using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;
using VS_SLG6.Services.Validators;

namespace VS_SLG6.Services.Services
{
    public class ProductService : GenericService<Product>, IProductService
    {
        IService<Proposal> _serviceProposal;
        IService<Tag> _serviceTag;

        public ProductService(IRepository<Product> repo, IValidator<Product> validator, IService<Proposal> serviceProposal, IService<Tag> serviceTag) : base(repo, validator)
        {
            _serviceProposal = serviceProposal;
            _serviceTag = serviceTag;
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

        public List<Product> GetUserProducts(int id)
        {
            return _repo.FindAll(x => x.Owner.Id == id);
        }

        public List<Product> GetProductsByKeys(string[] keys)
        {
            var list = _repo.All();
            return list.Where(x =>
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    if (x.Name.Contains(keys[i])) return true;
                }
                return false;
            }).ToList();
        }

        public List<Product> GetProductsByInterest(int id)
        {
            throw new NotImplementedException();
        }
    }
}
