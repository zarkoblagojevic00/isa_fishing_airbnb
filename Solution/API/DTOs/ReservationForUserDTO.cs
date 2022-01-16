using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ReservationForUserDTO
    {
        public string UserEmail { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
    }
}
