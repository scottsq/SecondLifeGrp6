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
        private IRepository<Proposal> _repoProposal;
        private IRepository<ProductTag> _repoProductTag;

        public ProductService(IRepository<Product> repo, IValidator<Product> validator, IRepository<Proposal> repoProposal, IRepository<ProductTag> repoProductTag) : base(repo, validator)
        {
            _repoProposal = repoProposal;
            _repoProductTag = repoProductTag;
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
            return _repo.All(x => x.Owner.Id == id);
        }

        public List<Product> GetProductsByKeys(params string[] keys)
        {
            var list = _repo.All();
            if (keys == null) return list;
            return list.Where(x =>
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    if (x.Name.Contains(keys[i].Trim())) return true;
                }
                return false;
            }).ToList();
        }

        public List<Tag> GetProductTags(int id)
        {
            var list = _repoProductTag.All(x => x.Product.Id == id);
            return list.Aggregate(new List<Tag>(), (acc, item) =>
            {
                acc.Add(item.Tag);
                return acc;
            });
        }

        public List<Product> GetByTag(int id)
        {
            var list = _repoProductTag.All(x => x.Tag.Id == id);

            // keep duplicates
            return list.Aggregate(new List<Product>(), (acc, item) =>
            {
                acc.Add(item.Product);
                return acc;
            });
        }

        public List<T> OrderByOccurence<T>(List<T> list)
        {
            if (list == null) return null;
            var orderedList = list.GroupBy(x => x).OrderByDescending(x => x.Count());

            // Need to convert IOrderedEnumerable<IGrouping<>> into List<Product>
            return orderedList.Aggregate(new List<T>(), (acc, item) => {
                acc.Add(item.Key);
                return acc;
            });
        }

        public List<Product> GetProductsByInterest(int id)
        {
            // Get all accepted proposals from user
            var listProposalAccepted = _repoProposal.All(x => x.State == State.ACCEPTED && (x.Target.Id == id || x.Origin.Id == id));
            if (listProposalAccepted.Count == 0) return new List<Product>(); // to not have empty list

            // Get tags from products of those proposals
            var tags = new List<Tag>();
            foreach (var proposal in listProposalAccepted) tags = tags.Concat(GetProductTags(proposal.Product.Id)).ToList();
            tags = OrderByOccurence(tags);

            // Get all Products which have those tags
            var listProducts = new List<Product>();
            foreach (var t in tags) listProducts = listProducts.Concat(GetByTag(t.Id)).ToList();
            return OrderByOccurence(listProducts);
        }
    }
}
