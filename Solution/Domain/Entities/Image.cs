using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Image
    {
        public virtual int ImageId { get; set; }
        public virtual byte[] Bytes { get; set; }
        public virtual int ServiceId { get; set; }
    }
}
