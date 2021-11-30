using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Mark
    {
        public virtual int MarkId { get; set; }
        public virtual int UserId { get; set; }
        public virtual double GivenMark { get; set; }
        public virtual string Description { get; set; }
        public virtual int ServiceId { get; set; }
    }
}
