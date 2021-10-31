using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class LegacyCategory
    {
        public virtual int LegacyId { get; set; }
        public virtual string Name { get; set; }
        public virtual int MinPoints { get; set; }
        public virtual int MaxPoints { get; set; }
        //Just serialized like this, we will have it converted into some POCO
        public virtual string Benefits { get; set; }
        public virtual double Discount { get; set; }

    }
}
