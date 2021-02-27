using System;
using System.Collections.Generic;
using System.Text;

namespace VS_SLG6.Model.Entities
{
    public class ProductTag
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public int ProductId { get; set; }
    }
}
