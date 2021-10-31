using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Country
    {
        public virtual int CountryId { get; set; }
        public virtual string Name { get; set; }
    }
}
