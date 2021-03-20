using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Services.Services
{
    public interface IProductRatingService : IService<ProductRating>
    {
        public ProductRating GetUserRating(int idProduct, int idUser);
        public double GetAverageRating(int id);
        public List<ProductRating> GetRatings(int id);
    }
}
