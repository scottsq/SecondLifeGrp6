﻿using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Validators;

namespace VS_SLG6.Services.Services
{
    public class ProductTagService : GenericService<ProductTag>, IProductTagService
    {
        public ProductTagService(IRepository<ProductTag> repo, IValidator<ProductTag> validator) : base(repo, validator)
        {
        }

        public List<ProductTag> GetByProductId(int id)
        {
            return _repo.All(x => x.Product.Id == id);
        }
    }
}
