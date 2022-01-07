using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests.Parameters
{
    public class QuickActionParameter
    {
        public int BeginDaysAfterToday { get; set; } //For: DateTime.Now.AddDays(BeginDaysAfterToday)
        public int EndDaysAfterToday { get; set; }
        public double PricePerDay { get; set; }
        public int Capacity { get; set; }
    }
}
