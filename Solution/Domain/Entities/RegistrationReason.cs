using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RegistrationReason
    {
        public virtual int UserId { get; set; }
        public virtual string Reason { get; set; }
        public virtual string DenialReason { get; set; }
        public virtual bool IsReviewed { get; set; }
    }
}
