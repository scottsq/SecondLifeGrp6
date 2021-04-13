using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Services.Models
{
    public class ContextUser
    {
        public int Id { get; set; }
        public Roles Role { get; set; }
    }
}
