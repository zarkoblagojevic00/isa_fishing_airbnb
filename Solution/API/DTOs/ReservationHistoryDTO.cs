using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace API.DTOs
{
    public class ReservationHistoryDTO
    {
        public Reservation Reservation { get; set; }
        public Mark Mark { get; set; }
        public Report Report { get; set; }
        public string Role { get; set; }
    }
}
