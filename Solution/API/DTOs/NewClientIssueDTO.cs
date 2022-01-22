using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class NewClientIssueDTO
    {
        public int IssuedEntityId { get; set; }
        public string Reason { get; set; }
    }
}
