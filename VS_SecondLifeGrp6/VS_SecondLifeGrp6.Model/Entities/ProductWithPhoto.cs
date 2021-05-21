using System.Collections.Generic;

namespace VS_SLG6.Model.Entities
{
    public class ProductWithPhoto
    {
        public Product Product { get; set; }
        public List<Photo> Photos { get; set; }
    }
}
