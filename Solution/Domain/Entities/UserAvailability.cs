using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserAvailability
    {
        public virtual int Id { get; set; }
        public virtual int UserId { get; set; }
        public virtual DateTime PeriodStart { get; set; }
        public virtual DateTime PeriodEnd { get; set; }
        public virtual bool Status { get; set; }
    }
}
