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
            return _repo.FindAll(x => x.Product.Id == id).Average(x => x.Stars);
        }

        public Rating GetUserRating(int id)
        {
            return _repo.FindAll(x => x.User.Id == id).FirstOrDefault();
        }
    }
}
