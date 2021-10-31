using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class FurtherClientInfo
    {
        public virtual int UserId { get; set; }
        public virtual int CollectedPoints { get; set; }
        public virtual int NumberOfPenalties { get; set; }
    }
}
