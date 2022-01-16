using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Helpers
{
    public class SummaryItem : CalendarItem
    {
        public string Name { get; set; }
        public int NumberOfReservations { get; set; }
        public double MoneyMade { get; set; }
    }
}
