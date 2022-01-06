using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Service
    {
        public virtual int ServiceId { get; set; }
        public virtual int OwnerId { get; set; }
        public virtual ServiceType ServiceType { get; set; }
        public virtual string Name { get; set; }
        public virtual double PricePerDay { get; set; }
        public virtual int CityId { get; set; }
        public virtual string Address { get; set; }
        public virtual double Longitude { get; set; }
        public virtual double Latitude { get; set; }
        //Maybe WYSIWYG editor html saved code here?
        public virtual string PromoDescription { get; set; }
        public virtual string TermsOfUse { get; set; }
        //Just for serialization, we will have intern POCO for this
        public virtual string AdditionalEquipment { get; set; }
        public virtual DateTime? AvailableFrom { get; set; }
        public virtual DateTime? AvailableTo { get; set; }
        public virtual int Capacity { get; set; }
        public virtual bool IsPercentageTakenFromCanceledReservations { get; set; }
        public virtual double PercentageToTake { get; set; }
    }

    public enum ServiceType
    {
        Villa,
        Boat,
        Adventure,
    }
}
