using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Services.Services
{
    public interface IRatingService : IService<Rating>
    {
        public List<Rating> GetUserRatings(int id);
        public double GetProductRating(int id);
    }
}
