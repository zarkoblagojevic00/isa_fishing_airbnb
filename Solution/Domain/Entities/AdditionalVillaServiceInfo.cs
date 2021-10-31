using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AdditionalVillaServiceInfo
    {
        public virtual int ServiceId { get; set; }
        public virtual int NumberOfBeds { get; set; }
        public virtual int NumberOfRooms { get; set; }
    }
}
