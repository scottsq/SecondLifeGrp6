using System;
using System.Collections.Generic;
using System.Text;

namespace VS_SLG6.Model.Entities
{
    public class Photo
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public string Url { get; set; }
    }
}
