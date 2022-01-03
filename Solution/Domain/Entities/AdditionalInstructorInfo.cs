using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AdditionalInstructorInfo
    {
        public virtual int UserId { get; set; }
        public virtual DateTime? AvailableFrom { get; set; }
        public virtual DateTime? AvailableTo { get; set; }
        public virtual string ShortBiography { get; set; }
    }
}
