using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        private IRepository<Photo> _repoPhoto;

        public ProductService(IRepository<Product> repo, IValidator<Product> validator, IRepository<Proposal> repoProposal, IRepository<ProductTag> repoProductTag, IRepository<Photo> repoPhoto) : base(repo, validator)
        {
            _repoProposal = repoProposal;
            _repoProductTag = repoProductTag;
            _repoPhoto = repoPhoto;
        }

        public override ValidationModel<Product> Remove(Product obj)
        {
            var listPhotos = _repoPhoto.All(x => x.Product.Id == obj.Id);
            foreach (var photo in listPhotos) _repoPhoto.Remove(photo);
            return base.Remove(obj);
        }

        public List<Product> Find(int userId = -1, string[] keys = null, string orderBy = nameof(Product.CreationDate), bool reverse = true, int from = 0, int max = 10)
        {
            var list = _repo.All(GenerateCondition(userId, keys), from, max);
            if (orderBy == nameof(Product.CreationDate))
            {
                if (reverse) list = list.OrderByDescending(x => x.CreationDate).ToList();
                else list = list.OrderBy(x => x.CreationDate).ToList();
            }
            return list;
        }

        public List<ProductWithPhoto> FindWithPhoto(int userId = -1, string[] keys = null, string orderBy = nameof(Product.CreationDate), bool reverse = true, int from = 0, int max = 10)
        {
            var list = Find(userId, keys, orderBy, reverse, from, max);
            var listWithPhotos = new List<ProductWithPhoto>();
            foreach (var product in list)
            {
                var pWithPhotos = new ProductWithPhoto();
                pWithPhotos.Photos = _repoPhoto.All(x => x.Product.Id == product.Id);
                pWithPhotos.Product = product;
                listWithPhotos.Add(pWithPhotos);
            }
            return listWithPhotos;
        }

        public static Expression<Func<Product, bool>> GenerateCondition(int userId = -1, string[] keys = null)
        {
            Expression<Func<Product, bool>> condition = x => true;
            if (userId > -1) condition.And(x => x.Owner.Id == userId);
            if (keys.Any()) {
                condition.And(x => keys.Where(key => x.Name.Contains(key)).Any());
            }
            return condition;
        }
    }
}
