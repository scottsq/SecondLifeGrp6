using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;

namespace VS_SLG6.Services.Validators
{
    public class RatingValidator : IValidator<Rating>
    {
        readonly IRepository<Rating> _repo;

        public RatingValidator(IRepository<Rating> repo)
        {
            _repo = repo;
        }

        public bool canAdd(Rating obj)
        {
            throw new NotImplementedException();
        }

        public bool canDelete(Rating obj)
        {
            throw new NotImplementedException();
        }

        public bool canEdit(Rating obj)
        {
            throw new NotImplementedException();
        }
    }
}
