using System.Collections.Generic;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Services.Interfaces
{
    public interface IUserRatingService : IService<UserRating>
    {
        public List<UserRating> Find(int id = -1, int idOrigin = -1, int idTarget = -1, int stars = -1, string orderBy = null, bool reverse = false, int from = 0, int max = 10);
        public double GetAverageRating(int id);
    }
}
