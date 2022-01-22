using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class NewClientMarkDTO
    {
        public double GivenMark { get; set; }
        public string Description { get; set; }
        public int ServiceId { get; set; }
    }
}
