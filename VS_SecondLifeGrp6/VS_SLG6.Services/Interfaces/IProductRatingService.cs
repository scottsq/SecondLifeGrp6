using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Services.Services
{
    public interface IProductRatingService : IService<ProductRating>
    {
        public List<ProductRating> GetUserRatings(int id);
        public double GetProductRating(int id);
        public List<ProductRating> GetRatings(int id);
    }
}
