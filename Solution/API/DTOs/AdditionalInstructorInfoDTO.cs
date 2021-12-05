using API.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class AdditionalInstructorInfoDTO
    {

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
