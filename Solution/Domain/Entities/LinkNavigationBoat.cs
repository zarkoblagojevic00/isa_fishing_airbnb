using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class LinkNavigationBoat
    {
        public virtual int ServiceId { get; set; }
        public virtual int BoatServiceNavigationToolId { get; set; }
        public virtual int Id { get; set; }
    }
}
