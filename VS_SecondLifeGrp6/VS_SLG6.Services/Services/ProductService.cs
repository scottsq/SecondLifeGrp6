using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Interfaces;
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

        public List<Product> Find(int id = -1, int userId = -1, string keys = null, string date = null, string orderBy = null, bool reverse = true, int from = 0, int max = 10)
        {
            var listKeys = keys?.Split(';');
            var list = _repo.All(
                GenerateCondition(id, userId, listKeys, date),
                GenerateOrderByCondition(orderBy),
                reverse, from, max
            );
            foreach (var p in list) { p.Owner.Login = p.Owner.Password = null; }
            return list;
        }

        public List<ProductWithPhoto> FindWithPhoto(int id = -1, int userId = -1, string keys = null, string date = null, string orderBy = nameof(Product.CreationDate), bool reverse = true, int from = 0, int max = 10)
        {
            var list = Find(id, userId, keys, date, orderBy, reverse, from, max);
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

        public static Expression<Func<Product, bool>> GenerateCondition(int id = -1, int userId = -1, string[] keys = null, string date = null)
        {
            Expression<Func<Product, bool>> condition = x => true;
            if (id > -1) condition = condition.And(x => x.Id == id);
            if (userId > -1) condition = condition.And(x => x.Owner.Id == userId);
            if (keys != null && keys.Any())
            {
                foreach (var key in keys) condition = condition.And(x => x.Name.Contains(key));
            }
            if (date != null)
            {
                var d = DateTime.MinValue;
                var res = DateTime.TryParse(date, out d);
                if (res) condition = condition.And(x => x.CreationDate == d);
            }
            return condition;
        }
    }
}
