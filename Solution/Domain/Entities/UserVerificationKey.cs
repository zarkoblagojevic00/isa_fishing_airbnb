using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserVerificationKey
    {
        public virtual int UserId { get; set; }
        public virtual Guid VerificationKey { get; set; }
        public virtual bool IsUsed { get; set; }
    }
}
