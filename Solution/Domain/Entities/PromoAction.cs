using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PromoAction
    {
        public virtual int PromoActionId { get; set; }
        public virtual int ServiceId { get; set; }
        public virtual DateTime StartDateTime { get; set; }
        public virtual DateTime EndDateTime { get; set; }
        public virtual double PricePerDay { get; set; }
        public virtual bool IsTaken { get; set; }
        public virtual int Capacity { get; set; }
        //Just for serialization, we will have intern POCO 
        public virtual string AddedBenefits { get; set; }
    }
}
