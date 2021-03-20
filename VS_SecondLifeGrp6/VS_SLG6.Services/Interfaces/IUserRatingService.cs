using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Services.Services
{
    public interface IUserRatingService : IService<UserRating>
    {
        public UserRating GetUserRating(int idOrigin, int idTarget);
        public double GetAverageRating(int id);
        public List<UserRating> GetRatings(int id);
    }
}
