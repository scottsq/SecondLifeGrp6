using System.Collections.Generic;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Services.Services
{
    public interface IProductRatingService : IService<ProductRating>
    {
        public List<ProductRating> Find(int idProduct = -1, int indUser = -1, int from = 0, int max = 10);
        public double GetAverageRating(int id);
    }
}
