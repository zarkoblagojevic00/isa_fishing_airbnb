using Domain.Entities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Helpers
{
    public class FutureServiceAvailability
    {
        public Service Service { get; }
        public string LocationName { get; }
        public double AverageMark { get; }

        public IEnumerable<Reservation> FutureReservations { get; }

        public FutureServiceAvailability(Service service, string locationName, double averageMark, IEnumerable<Reservation> futureReservations)
        {
            Service = service;
            LocationName = locationName;
            AverageMark = averageMark;
            FutureReservations = futureReservations;
        }

        public bool IsAvailable(ServiceSearchParameters @params) {
            return
                IsServiceNameSatisfied(@params.ServiceName) &&
                IsLocationNameSatisfied(@params.LocationName) &&
                IsMarkSatisfied(@params.GivenMark) &&
                IsCapacitySatisfied(@params.Capacity) &&
                IsPriceRangeSatisfied(@params.PriceRange) &&
                IsDateRangeSatisfied(@params.DateRange);
        }

        private bool IsServiceNameSatisfied(string serviceName)
        {
            return Service.Name.Contains(serviceName);
        }

        private bool IsLocationNameSatisfied(string locationName)
        {
            return LocationName.Contains(locationName);
        }

        private bool IsMarkSatisfied(double givenMark)
        {
            double offset = 0.5;
            return givenMark == 0 || (givenMark - offset < AverageMark && AverageMark <= givenMark + offset);
        }

        private bool IsCapacitySatisfied(int capacity)
        {
            return capacity == 0 || capacity < Service.Capacity;
        }

        private bool IsPriceRangeSatisfied(PriceRange priceRange)
        {
            if (priceRange.From == 0 && priceRange.To == 0)
                return true;
            return priceRange.IsPriceWithinRange(Service.PricePerDay);
        }

        private bool IsDateRangeSatisfied(CalendarItem dateRange)
        {
            return IsFutureReservationsSatisfied(dateRange) && IsGlobalAvailabilitySatisfied(dateRange);
        }

        private bool IsFutureReservationsSatisfied(CalendarItem dateRange)
        {
            return !dateRange.AnyOverlapping(FutureReservations);
        }

        private bool IsGlobalAvailabilitySatisfied(CalendarItem dateRange)
        {
            if (dateRange.StartDateTime == DateTime.MinValue && dateRange.EndDateTime == DateTime.MinValue)
                return true;

            DateTime globalAvailabilityFrom = Service.AvailableFrom ?? DateTime.MinValue;
            DateTime globalAvailabilityTo = Service.AvailableTo ?? DateTime.MaxValue;
            CalendarItem globalAvailability = new CalendarItem() { StartDateTime = globalAvailabilityFrom, EndDateTime = globalAvailabilityTo };
            return dateRange.IsWithinRange(globalAvailability);
        }
    }
}
