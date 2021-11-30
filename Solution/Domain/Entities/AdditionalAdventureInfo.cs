using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AdditionalAdventureInfo
    {
        public virtual int AdventureId { get; set; }
        public virtual string AdditionalOffers { get; set; }
    }
}
