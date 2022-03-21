using System.Collections.Generic;

namespace Flock.API.Models
{
    public class ValidationErrorModel
    {
        public string FieldName { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
