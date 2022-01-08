using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.Helpers;
using Domain.UnitOfWork;

namespace Services.Validators
{
    public class ValidateReservationDatesService
    {
        public bool CalendarItemOverlap(CalendarItem first, CalendarItem second)
        {
            return first.IsOverlapping(second);
        }

        public bool CalendarItemOverlapsWithAny(CalendarItem reservation, IEnumerable<CalendarItem> otherReservations)
        {
            return reservation.AnyOverlapping(otherReservations);
        }
    }
}
