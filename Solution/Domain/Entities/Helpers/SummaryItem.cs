using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Abstractions;

namespace Domain.Entities.Helpers
{
    public class SummaryItem : CalendarItem
    {
        public string Name { get; set; }
        public int Attendance { get; set; }
        public double MoneyMade { get; set; }
    }
}
