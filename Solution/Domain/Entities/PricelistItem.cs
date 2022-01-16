using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PricelistItem
    {
        public virtual int PricelistItemId { get; set; }
        public virtual string Name { get; set; }
        public virtual double Price { get; set; }
        public virtual int ServiceId { get; set; }
    }
}
