using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Helpers
{
    public class PriceRange
    {
        public double From { get; }
        public double To { get; }

        public PriceRange(double from, double to)
        {
            From = from;
            To = to;
        }

        public bool IsPriceWithinRange(double price)
        {
            return From <= price && price <= To;
        }
    }
}
