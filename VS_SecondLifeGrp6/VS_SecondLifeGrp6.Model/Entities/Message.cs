using System;
using System.Collections.Generic;
using System.Text;

namespace VS_SLG6.Model.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public User Sender { get; set; }
        public User Receipt { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
