using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Helpers
{
    public class ServiceSearchParameters
    {
        public string ServiceName { get; set; }

        public CalendarItem DateRange { get; set; }

        public PriceRange PriceRange { get; set; }
        
        public string LocationName { get; set; }

        public double GivenMark { get; set; }

        public int Capacity { get; set; }
    }
}
