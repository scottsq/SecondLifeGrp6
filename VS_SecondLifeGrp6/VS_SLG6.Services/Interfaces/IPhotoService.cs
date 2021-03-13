using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Services;

namespace VS_SLG6.Services.Interfaces
{
    public interface IPhotoService : IService<Photo>
    {
        public List<Photo> GetByProduct(int id);
    }
}
