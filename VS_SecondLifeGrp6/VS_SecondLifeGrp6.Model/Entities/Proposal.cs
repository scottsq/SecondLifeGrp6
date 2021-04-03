using System;
using System.Collections.Generic;
using System.Text;

namespace VS_SLG6.Model.Entities
{
    public class Proposal
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public TimeSpan Period { get; set; }
        public string State { get; set; }
        public int ProductId { get; set; }
        public int TargetId { get; set; }
        public int OriginId { get; set; }
    }
}
