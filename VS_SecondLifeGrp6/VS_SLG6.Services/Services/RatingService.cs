using System.Collections.Generic;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using System;
using VS_SLG6.Services.Models;
using VS_SLG6.Services.Validators;
using System.Linq;

namespace VS_SLG6.Services.Services
{
    public class RatingService : GenericService<Rating>, IRatingService
    {
        public RatingService(IRepository<Rating> repo, IValidator<Rating> validator) : base(repo, validator)
        {
        }

        public double GetProductRating(int id)
        {
            var res = _repo.FindAll(x => x.Product.Id == id);
            if (res.Count == 0) return 0;
            return res.Average(x => x.Stars);
        }

        public List<Rating> GetUserRatings(int id)
        {
            return _repo.FindAll(x => x.User.Id == id);
        }
    }
}
