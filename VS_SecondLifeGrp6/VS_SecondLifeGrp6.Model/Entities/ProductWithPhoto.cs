using System;
using System.Collections.Generic;
using System.Text;

namespace VS_SLG6.Model.Entities
{
    public class ProductWithPhoto
    {
        public Product Product { get; set; }
        public List<Photo> Photos { get; set; }
    }
}
