using System;
using System.Collections.Generic;
using System.Text;

namespace VS_SLG6.Services.Models
{
    public class ValidationModel<T> where T : class
    {
        public T Value { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
