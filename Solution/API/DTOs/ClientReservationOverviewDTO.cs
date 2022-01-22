using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ClientReservationOverviewDTO
    {
        public ReservationOverviewDTO Reservation { get; set; }

        public ServiceOverviewDTO Service { get; set; }
    }
}
