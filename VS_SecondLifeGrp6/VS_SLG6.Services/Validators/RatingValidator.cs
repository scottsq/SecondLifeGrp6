using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;
using VS_SLG6.Services.Services;

namespace VS_SLG6.Services.Validators
{
    public class RatingValidator : GenericValidator<Rating>, IValidator<Rating>
    {
        private IService<Product> _serviceProduct;
        private IService<User> _serviceUser;

        public RatingValidator(IRepository<Rating> repo, ValidationModel<bool> validationModel, IService<Product> serviceProduct, IService<User> serviceUser) : base(repo, validationModel) 
        {
            _serviceProduct = serviceProduct;
            _serviceUser = serviceUser;
        }
    }
}
