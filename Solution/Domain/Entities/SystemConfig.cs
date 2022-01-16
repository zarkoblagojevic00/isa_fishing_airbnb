using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SystemConfig
    {
        public virtual string Name { get; set; }
        public virtual string Value { get; set; }
        public virtual int Id { get; set; }
    }
}
