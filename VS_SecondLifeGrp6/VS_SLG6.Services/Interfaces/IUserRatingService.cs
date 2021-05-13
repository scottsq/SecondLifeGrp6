using System.Collections.Generic;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Services.Services
{
    public interface IUserRatingService : IService<UserRating>
    {
        public List<UserRating> Find(int idOrigin = -1, int idTarget = -1, string orderBy = nameof(UserRating.Stars), bool reverse = false, int from = 0, int max = 10);
        public double GetAverageRating(int id);
    }
}
