using System.Collections.Generic;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using System;
using VS_SLG6.Services.Models;
using VS_SLG6.Services.Validators;

namespace VS_SLG6.Services.Services
{
    public class RatingService : GenericService<Rating>, IService<Rating>
    {
        public RatingService(IRepository<Rating> repo, IValidator<Rating> validator) : base(repo, validator)
        {
        }
    }
}
