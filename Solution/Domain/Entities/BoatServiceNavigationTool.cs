using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BoatServiceNavigationTool
    {
        public virtual int BoatServiceToolId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }
}
