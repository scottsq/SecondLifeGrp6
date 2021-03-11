using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;
using VS_SLG6.Services.Services;

namespace VS_SLG6.Services.Validators
{
    public class ProductValidator : GenericValidator<Product>, IValidator<Product>
    {
        private IService<User> _serviceUser;

        public ProductValidator(IRepository<Product> repo, ValidationModel<bool> validationModel, IService<User> serviceUser) : base(repo, validationModel) 
        {
            _serviceUser = serviceUser;
        }
    }
}
