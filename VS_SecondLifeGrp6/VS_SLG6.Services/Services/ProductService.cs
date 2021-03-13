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
        IProposalService _serviceProposal;
        IProductTagService _serviceProductTag;

        public ProductService(IRepository<Product> repo, IValidator<Product> validator, IProposalService serviceProposal, IProductTagService serviceProductTag) : base(repo, validator)
        {
            _serviceProposal = serviceProposal;
            _serviceProductTag = serviceProductTag;
        }

        public List<Product> GetLatest(int max = 10)
        {
            Comparison<Product> c = new Comparison<Product>((a, b) =>
            {
                return a.CreationDate - b.CreationDate > new TimeSpan(0) ? -1 : a.CreationDate == b.CreationDate ? 0 : 1;
            });
            var list = _repo.All();
            list.Sort(c);
            return list.GetRange(0, Math.Min(max, list.Count));
        }

        public List<Product> GetUserProducts(int id)
        {
            return _repo.FindAll(x => x.Owner.Id == id);
        }

        public List<Product> GetProductsByKeys(params string[] keys)
        {
            var list = _repo.All();
            if (keys == null) return list;
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
            // Get all accepted proposals from user
            var listProposalAccepted = _serviceProposal.GetAcceptedProposalByUser(id);
            if (listProposalAccepted.Count == 0) return new List<Product>();
            // Count tags occurence from those proposals
            Dictionary<Tag, int> listTags = listProposalAccepted.Aggregate(new Dictionary<Tag, int>(), (acc, item) =>
            {
                var tags = _serviceProductTag.GetByProductId(item.Product.Id);
                for (int i = 0; i < tags.Count; i++)
                {
                    if (acc.ContainsKey(tags[i].Tag)) acc[tags[i].Tag] += 1;
                    else acc.Add(tags[i].Tag, 1);
                }                
                return acc;
            });
            // Get the 3 most used tags
            var listMostUsed = new List<Tag>();
            for (int i = 0; i < Math.Min(3, listTags.Count); i++)
            {
                var t = listTags.Where(x => x.Value == listTags.Max(x => x.Value)).ToDictionary(x => x.Key, y => y.Value);
                listTags.Remove(t.First().Key);
                listMostUsed.Add(t.First().Key);
            }
            // Get all ProductTags which have those tags
            List<Product> listProducts = _serviceProductTag.List().Where(productTag =>
            {
                for (int i = 0; i < listMostUsed.Count; i++)
                {
                    if (listMostUsed.Contains(productTag.Tag)) return true;
                }
                return false;
            }).ToList().Aggregate(new List<Product>(), (acc, item) =>
            {
                // Get every products matching this ProductTag
                acc.Add(item.Product);
                return acc;
            });

            // Remove doublons
            return _repo.All().Where(x => listProducts.Contains(x)).ToList();
        }
    }
}
