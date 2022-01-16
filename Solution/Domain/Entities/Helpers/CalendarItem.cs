using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Helpers
{
    public class CalendarItem
    {
        public virtual DateTime StartDateTime { get; set; }
        public virtual DateTime EndDateTime { get; set; }

        public virtual bool IsWithinRange(CalendarItem item)
        {
            return  item.StartDateTime < StartDateTime && EndDateTime < item.EndDateTime;
        }

        public virtual bool IsOverlapping(CalendarItem item) {
            return StartDateTime < item.EndDateTime && item.StartDateTime < EndDateTime;
        }

        public virtual bool AnyOverlapping(IEnumerable<CalendarItem> items)
        {
            return items.Any(res => res.IsOverlapping(this));
        }
    }
}
