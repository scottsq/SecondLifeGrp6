using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Services.Interfaces
{
    interface IPhotoService : IGenericService<Photo>
    {
        public List<Photo> GetByProduct(int id);
    }
}
