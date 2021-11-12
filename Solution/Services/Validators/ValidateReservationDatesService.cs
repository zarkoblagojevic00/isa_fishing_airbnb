using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.Abstractions;
using Domain.UnitOfWork;

namespace Services.Validators
{
    public class ValidateReservationDatesService
    {
        public bool CalendarItemOverlap(CalendarItem first, CalendarItem second)
        {
            return first.StartDateTime < second.EndDateTime && second.StartDateTime < first.EndDateTime;
        }

        public bool CalendarItemOverlapsWithAny(CalendarItem reservation, IEnumerable<CalendarItem> otherReservations)
        {
            var ret = false;

            foreach (var res in otherReservations)
            {
                if (CalendarItemOverlap(reservation, res))
                {
                    ret = true;
                    break;
                }
            }

            return ret;
        }
    }
}
