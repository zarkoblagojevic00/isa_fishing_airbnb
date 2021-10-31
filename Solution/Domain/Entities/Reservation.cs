using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Reservation
    {
        public virtual int ReservationId { get; set; }
        public virtual int UserId { get; set; }
        public virtual int ServiceId { get; set; }
        public virtual DateTime ReservedDateTime { get; set; }
        public virtual bool IsPromo { get; set; }
        public virtual DateTime StartDateTime { get; set; }
        public virtual DateTime EndDateTime { get; set; }
        public virtual bool IsCanceled { get; set; }
        public virtual bool IsServiceUnavailableMarker { get; set; }
        public virtual int ReportId { get; set; }
        public virtual int MarkId { get; set; }
        public virtual string AdditionalEquipment { get; set; }
        public virtual double Price { get; set; }
    }
}
