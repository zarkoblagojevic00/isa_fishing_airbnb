using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AdditionalBoatServiceInfo
    {
        public virtual int ServiceId { get; set; }
        public virtual double Length { get; set; }
        public virtual  BoatType BoatType { get; set; }
        public virtual int NumberOfEngines { get; set; }
        public virtual double PowerOfEngines { get; set; }
        public virtual double MaxSpeed { get; set; }
    }

    public enum BoatType
    {

    }
}
