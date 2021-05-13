using System.Collections.Generic;

namespace VS_SLG6.Services.Models
{
    // Helper for generic service validator
    public class ConstraintsObject
    {
        public List<string> FieldsNotNull { get; set; } = new List<string>();
        public List<string> FieldsStringNotBlank { get; set; } = new List<string>();
        public List<string> FieldsStringNotLongerThanMax { get; set; } = new List<string>();
    }
}
