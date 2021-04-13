using System.Collections.Generic;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using System;
using VS_SLG6.Services.Models;
using VS_SLG6.Services.Validators;
using System.Linq;

namespace VS_SLG6.Services.Services
{
    public class ProductRatingService : GenericService<ProductRating>, IProductRatingService
    {
        public ProductRatingService(IRepository<ProductRating> repo, IValidator<ProductRating> validator) : base(repo, validator)
        {
        }

        public double GetAverageRating(int id)
        {
            var list = _repo.All(x => x.Product.Id == id);
            if (list.Count == 0) return 0;
            return list.Average(x => x.Stars);
        }

        public List<ProductRating> GetRatings(int id)
        {
            return _repo.All(x => x.Product.Id == id);
        }

        public ProductRating GetUserRating(int idProduct, int idUser)
        {
            return _repo.FindOne(x => x.Product.Id == idProduct && x.User.Id == idUser);
        }
    }
}
