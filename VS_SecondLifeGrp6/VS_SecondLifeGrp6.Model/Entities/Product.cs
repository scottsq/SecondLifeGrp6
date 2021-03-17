using System;
using System.Collections.Generic;
using System.Text;

namespace VS_SLG6.Model.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public User Owner { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }

        public override string ToString()
        {
            return String.Format("Id: [{0}], Owner: [{1}], Name: [{2}]", Id, Owner != null ? Owner.ToString() : "null", Name);
        }
    }
}
