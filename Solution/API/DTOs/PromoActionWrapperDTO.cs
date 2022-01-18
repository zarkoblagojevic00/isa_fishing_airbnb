using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities.Helpers;

namespace API.DTOs
{
    public class PromoActionWrapperDTO : CalendarItem
    {
        public int PromoActionId { get; set; }
        public int ServiceId { get; set; }
        public double PricePerDay { get; set; }
        public bool IsTaken { get; set; }
        public int Capacity { get; set; }
        //Just for serialization, we will have intern POCO 
        public string AddedBenefits { get; set; }
        public string Place { get; set; }
        public string Role { get; set; }
    }
}
