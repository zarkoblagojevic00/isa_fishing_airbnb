using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ReservationOverviewDTO
    {
        public virtual int ReservationId { get; set; }
        public virtual int UserId { get; set; }
        public virtual int ServiceId { get; set; }
        public virtual DateTime ReservedDateTime { get; set; }
        public virtual bool IsPromo { get; set; }
        public virtual bool IsCanceled { get; set; }
        public virtual string AdditionalEquipment { get; set; }
        public virtual double Price { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
