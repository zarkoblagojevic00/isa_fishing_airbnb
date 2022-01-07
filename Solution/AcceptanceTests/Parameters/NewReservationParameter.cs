using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests.Parameters
{
    public class NewReservationParameter
    {
        public double PricePerDay { get; set; }
        public string AdditionalEquipment { get; set; }
        public int BeginDaysAfterToday { get; set; }
        public int EndDaysAfterToday { get; set; }
    }
}
