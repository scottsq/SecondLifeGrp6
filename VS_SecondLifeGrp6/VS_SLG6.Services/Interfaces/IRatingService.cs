using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Services.Services
{
    public interface IRatingService : IService<Rating>
    {
        public Rating GetUserRating(int id);
        public double GetProductRating(int id);
    }
}
