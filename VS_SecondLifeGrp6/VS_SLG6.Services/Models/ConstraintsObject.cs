using System;
using System.Collections.Generic;
using System.Text;

namespace VS_SLG6.Services.Models
{
    public class ConstraintsObject
    {
        public List<string> PropsNonNull { get; set; } = new List<string>();
        public List<string> PropsStringNotBlank { get; set; } = new List<string>();
        public List<string> PropsStringNotLongerThanMax { get; set; } = new List<string>();
    }
}
