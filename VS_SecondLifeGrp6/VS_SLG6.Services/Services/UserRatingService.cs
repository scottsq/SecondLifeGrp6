using System.Collections.Generic;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using System;
using VS_SLG6.Services.Models;
using VS_SLG6.Services.Validators;
using System.Linq;

namespace VS_SLG6.Services.Services
{
    public class UserRatingService : GenericService<UserRating>, IUserRatingService
    {
        public UserRatingService(IRepository<UserRating> repo, IValidator<UserRating> validator) : base(repo, validator)
        {
        }

        public double GetAverageRating(int id)
        {
            var res = _repo.FindAll(x => x.Target.Id == id);
            if (res.Count == 0) return 0;
            return res.Average(x => x.Stars);
        }

        public List<UserRating> GetRatings(int id)
        {
            return _repo.FindAll(x => x.Target.Id == id);
        }

        public List<UserRating> GetUserRatings(int id)
        {
            return _repo.FindAll(x => x.Origin.Id == id);
        }
    }
}
