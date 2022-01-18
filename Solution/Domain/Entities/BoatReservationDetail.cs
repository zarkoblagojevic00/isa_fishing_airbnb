using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BoatReservationDetail
    {
        public virtual int Id { get; set; }
        public virtual BoatOwnerResponsibilityType BoatOwnerResponsibilityType { get; set; }
        public virtual int RelevantId { get; set; }
        public virtual bool IsPromo { get; set; }
    }

    public enum BoatOwnerResponsibilityType
    {
        Captain,
        FirstAssistant
    }
}
