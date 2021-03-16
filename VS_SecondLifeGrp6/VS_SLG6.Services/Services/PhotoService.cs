using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Interfaces;
using VS_SLG6.Services.Validators;

namespace VS_SLG6.Services.Services
{
    public class PhotoService : GenericService<Photo>, IPhotoService
    {
        public PhotoService(IRepository<Photo> repo, IValidator<Photo> validator): base(repo, validator) 
        {       
        }
        public List<Photo> GetByProduct(int id)
        {
            return _repo.All(x => x.Product.Id == id);
        }
    }
}
