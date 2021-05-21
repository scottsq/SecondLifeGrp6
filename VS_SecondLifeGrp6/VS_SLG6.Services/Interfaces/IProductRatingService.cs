using System.Collections.Generic;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Services.Interfaces
{
    public interface IProductRatingService : IService<ProductRating>
    {
        public List<ProductRating> Find(int id = -1, int idProduct = -1, int idUser = -1, int stars = -1, string orderBy = null, bool reverse = false, int from = 0, int max = 10);
        public double GetAverageRating(int id);
    }
}
