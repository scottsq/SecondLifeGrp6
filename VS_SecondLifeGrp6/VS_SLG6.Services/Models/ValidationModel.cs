using System;
using System.Collections.Generic;
using System.Linq;

namespace VS_SLG6.Services.Models
{
    public class ValidationModel<T> where T : notnull
    {
        public T Value { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public bool HasErrors => Errors.Any();

        internal bool Any()
        {
            throw new NotImplementedException();
        }
    }
}
