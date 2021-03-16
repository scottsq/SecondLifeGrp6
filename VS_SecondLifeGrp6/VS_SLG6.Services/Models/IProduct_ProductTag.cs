using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Services.Models
{
    public interface IProduct_ProductTag
    {
        public List<ProductTag> GetByProductId(int id);
        public List<ProductTag> List();
    }
}
