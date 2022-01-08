using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Helpers;
using Domain.Entities;
using FluentAssertions;

namespace UnitTests
{
    [TestClass]
    [TestCategory("ServiceTests")]
    public class FutureServiceAvailabilityTests
    {
        [TestMethod]
        public void ServiceAllCriteriaSatisfied()
        {
            FutureServiceAvailability serviceAvailability = GetFutureServiceAvailability(
                DateTime.Now,
                DateTime.Now.AddDays(100)
            );

            ServiceSearchParameters @params = new ServiceSearchParameters()
            {
                ServiceName = "TestService",
                LocationName = "Test Location",
                Capacity = 8,
                GivenMark = 4,
                PriceRange = new PriceRange(10, 30),
                DateRange = new CalendarItem() { StartDateTime = DateTime.Now.AddDays(31), EndDateTime = DateTime.Now.AddDays(41) }
            };

            bool isAvailable = serviceAvailability.IsAvailable(@params);

            isAvailable.Should().Be(true);
        }

        [TestMethod]
        public void ServicePartNameSatisfied()
        {
            FutureServiceAvailability serviceAvailability = GetFutureServiceAvailability(
                DateTime.Now,
                DateTime.Now.AddDays(100)
            );

            ServiceSearchParameters @params = new ServiceSearchParameters()
            {
                ServiceName = "Service",
                LocationName = "Test Location",
                Capacity = 8,
                GivenMark = 4,
                PriceRange = new PriceRange(10, 30),
                DateRange = new CalendarItem() { StartDateTime = DateTime.Now.AddDays(31), EndDateTime = DateTime.Now.AddDays(41) }
            };

            bool isAvailable = serviceAvailability.IsAvailable(@params);

            isAvailable.Should().Be(true);
        }

        [TestMethod]
        public void ServiceNameNotSatisfied()
        {
            FutureServiceAvailability serviceAvailability = GetFutureServiceAvailability(
                DateTime.Now,
                DateTime.Now.AddDays(100)
            );

            ServiceSearchParameters @params = new ServiceSearchParameters()
            {
                ServiceName = "NotCorrectName",
                LocationName = "Test Location",
                Capacity = 8,
                GivenMark = 4,
                PriceRange = new PriceRange(10, 30),
                DateRange = new CalendarItem() { StartDateTime = DateTime.Now.AddDays(31), EndDateTime = DateTime.Now.AddDays(41) }
            };

            bool isAvailable = serviceAvailability.IsAvailable(@params);

            isAvailable.Should().Be(false);
        }

        [TestMethod]
        public void ServiceLocationPartNameSatisfied()
        {
            FutureServiceAvailability serviceAvailability = GetFutureServiceAvailability(
                DateTime.Now,
                DateTime.Now.AddDays(100)
            );

            ServiceSearchParameters @params = new ServiceSearchParameters()
            {
                ServiceName = "TestService",
                LocationName = "Location",
                Capacity = 8,
                GivenMark = 4,
                PriceRange = new PriceRange(10, 30),
                DateRange = new CalendarItem() { StartDateTime = DateTime.Now.AddDays(31), EndDateTime = DateTime.Now.AddDays(41) }
            };

            bool isAvailable = serviceAvailability.IsAvailable(@params);

            isAvailable.Should().Be(true);
        }

        [TestMethod]
        public void ServiceLocationNameNotSatisfied()
        {
            FutureServiceAvailability serviceAvailability = GetFutureServiceAvailability(
                DateTime.Now,
                DateTime.Now.AddDays(100)
            );

            ServiceSearchParameters @params = new ServiceSearchParameters()
            {
                ServiceName = "TestService",
                LocationName = "NotCorrectName",
                Capacity = 8,
                GivenMark = 4,
                PriceRange = new PriceRange(10, 30),
                DateRange = new CalendarItem() { StartDateTime = DateTime.Now.AddDays(31), EndDateTime = DateTime.Now.AddDays(41) }
            };

            bool isAvailable = serviceAvailability.IsAvailable(@params);

            isAvailable.Should().Be(false);
        }

        [TestMethod]
        public void ServiceCapacityLowerThanNeeded()
        {
            FutureServiceAvailability serviceAvailability = GetFutureServiceAvailability(
                DateTime.Now,
                DateTime.Now.AddDays(100)
            );

            ServiceSearchParameters @params = new ServiceSearchParameters()
            {
                ServiceName = "TestService",
                LocationName = "Test Location",
                Capacity = 12,
                GivenMark = 4,
                PriceRange = new PriceRange(10, 30),
                DateRange = new CalendarItem() { StartDateTime = DateTime.Now.AddDays(31), EndDateTime = DateTime.Now.AddDays(41) }
            };

            bool isAvailable = serviceAvailability.IsAvailable(@params);

            isAvailable.Should().Be(false);
        }

        [TestMethod]
        public void ServiceAverageMarkTopBorderOfGivenRange()
        {
            FutureServiceAvailability serviceAvailability = GetFutureServiceAvailability(
                DateTime.Now,
                DateTime.Now.AddDays(100),
                averageMark: 3.5
            );

            ServiceSearchParameters @params = new ServiceSearchParameters()
            {
                ServiceName = "TestService",
                LocationName = "Test Location",
                Capacity = 8,
                GivenMark = 3,
                PriceRange = new PriceRange(10, 30),
                DateRange = new CalendarItem() { StartDateTime = DateTime.Now.AddDays(31), EndDateTime = DateTime.Now.AddDays(41) }
            };

            bool isAvailable = serviceAvailability.IsAvailable(@params);

            isAvailable.Should().Be(true);
        }

        [TestMethod]
        public void ServiceAverageMarkBottomBorderOfGivenRange()
        {
            FutureServiceAvailability serviceAvailability = GetFutureServiceAvailability(
                DateTime.Now,
                DateTime.Now.AddDays(100),
                averageMark: 2.5
            );

            ServiceSearchParameters @params = new ServiceSearchParameters()
            {
                ServiceName = "TestService",
                LocationName = "Test Location",
                Capacity = 8,
                GivenMark = 3,
                PriceRange = new PriceRange(10, 30),
                DateRange = new CalendarItem() { StartDateTime = DateTime.Now.AddDays(31), EndDateTime = DateTime.Now.AddDays(41) }
            };

            bool isAvailable = serviceAvailability.IsAvailable(@params);

            isAvailable.Should().Be(false);
        }

        [TestMethod]
        public void ServiceAverageMarkLowerThanGivenRange()
        {
            FutureServiceAvailability serviceAvailability = GetFutureServiceAvailability(
                DateTime.Now,
                DateTime.Now.AddDays(100)
            );

            ServiceSearchParameters @params = new ServiceSearchParameters()
            {
                ServiceName = "TestService",
                LocationName = "Test Location",
                Capacity = 8,
                GivenMark = 5,
                PriceRange = new PriceRange(10, 30),
                DateRange = new CalendarItem() { StartDateTime = DateTime.Now.AddDays(31), EndDateTime = DateTime.Now.AddDays(41) }
            };

            bool isAvailable = serviceAvailability.IsAvailable(@params);

            isAvailable.Should().Be(false);
        }

        [TestMethod]
        public void ServiceAverageMarkHigherThanGivenRange()
        {
            FutureServiceAvailability serviceAvailability = GetFutureServiceAvailability(
                DateTime.Now,
                DateTime.Now.AddDays(100)
            );

            ServiceSearchParameters @params = new ServiceSearchParameters()
            {
                ServiceName = "TestService",
                LocationName = "Test Location",
                Capacity = 8,
                GivenMark = 2,
                PriceRange = new PriceRange(10, 30),
                DateRange = new CalendarItem() { StartDateTime = DateTime.Now.AddDays(31), EndDateTime = DateTime.Now.AddDays(41) }
            };

            bool isAvailable = serviceAvailability.IsAvailable(@params);

            isAvailable.Should().Be(false);
        }

        [TestMethod]
        public void ServicePriceNotInRange()
        {
            FutureServiceAvailability serviceAvailability = GetFutureServiceAvailability(
                DateTime.Now,
                DateTime.Now.AddDays(100)
            );

            ServiceSearchParameters @params = new ServiceSearchParameters()
            {
                ServiceName = "TestService",
                LocationName = "Test Location",
                Capacity = 8,
                GivenMark = 4,
                PriceRange = new PriceRange(10, 15),
                DateRange = new CalendarItem() { StartDateTime = DateTime.Now.AddDays(31), EndDateTime = DateTime.Now.AddDays(41) }
            };

            bool isAvailable = serviceAvailability.IsAvailable(@params);

            isAvailable.Should().Be(false);
        }

        [TestMethod]
        public void ServiceDateRangeOverlapingWithFutureReservations()
        {
            FutureServiceAvailability serviceAvailability = GetFutureServiceAvailability(
                DateTime.Now,
                DateTime.Now.AddDays(100)
            );

            ServiceSearchParameters @params = new ServiceSearchParameters()
            {
                ServiceName = "TestService",
                LocationName = "Test Location",
                Capacity = 8,
                GivenMark = 4,
                PriceRange = new PriceRange(10, 30),
                DateRange = new CalendarItem() { StartDateTime = DateTime.Now.AddDays(2), EndDateTime = DateTime.Now.AddDays(6) }
            };

            bool isAvailable = serviceAvailability.IsAvailable(@params);

            isAvailable.Should().Be(false);
        }

        [TestMethod]
        public void ServiceDateRangeWithinOneOfFutureReservations()
        {
            FutureServiceAvailability serviceAvailability = GetFutureServiceAvailability(
                DateTime.Now,
                DateTime.Now.AddDays(100)
            );

            ServiceSearchParameters @params = new ServiceSearchParameters()
            {
                ServiceName = "TestService",
                LocationName = "Test Location",
                Capacity = 8,
                GivenMark = 4,
                PriceRange = new PriceRange(10, 30),
                DateRange = new CalendarItem() { StartDateTime = DateTime.Now.AddDays(11), EndDateTime = DateTime.Now.AddDays(13) }
            };

            bool isAvailable = serviceAvailability.IsAvailable(@params);

            isAvailable.Should().Be(false);
        }

        [TestMethod]
        public void ServiceDateRangeNotWithinGlobalAvailabilityRange()
        {
            FutureServiceAvailability serviceAvailability = GetFutureServiceAvailability(
                DateTime.Now,
                DateTime.Now.AddDays(100)
            );

            ServiceSearchParameters @params = new ServiceSearchParameters()
            {
                ServiceName = "TestService",
                LocationName = "Test Location",
                Capacity = 8,
                GivenMark = 4,
                PriceRange = new PriceRange(10, 30),
                DateRange = new CalendarItem() { StartDateTime = DateTime.Now.AddDays(96), EndDateTime = DateTime.Now.AddDays(110) }
            };

            bool isAvailable = serviceAvailability.IsAvailable(@params);

            isAvailable.Should().Be(false);
        }

        [TestMethod]
        public void ServiceFromToNotSpecified()
        {
            FutureServiceAvailability serviceAvailability = GetFutureServiceAvailability(
                null,
                null
            );

            ServiceSearchParameters @params = new ServiceSearchParameters()
            {
                ServiceName = "TestService",
                LocationName = "Test Location",
                Capacity = 8,
                GivenMark = 4,
                PriceRange = new PriceRange(10, 30),
                DateRange = new CalendarItem() { StartDateTime = DateTime.Now.AddDays(96), EndDateTime = DateTime.Now.AddDays(110) }
            };

            bool isAvailable = serviceAvailability.IsAvailable(@params);

            isAvailable.Should().Be(true);
        }


        private static FutureServiceAvailability GetFutureServiceAvailability(
            DateTime? serviceAvailableFrom,
            DateTime? serviceAvailableTo,
            string serviceName = "TestService",
            int capacity = 10,
            int pricePerDay = 20,
            double averageMark = 3.85) {
            
            Service service = new ()
            {
                Name = serviceName,
                Capacity = capacity,
                PricePerDay = pricePerDay,
                AvailableFrom = serviceAvailableFrom,
                AvailableTo = serviceAvailableTo,
            };

            return new FutureServiceAvailability(
                service, 
                "Test Location", 
                averageMark,
                GetFutureReservations()
            );
        }

        

        private static IEnumerable<Reservation> GetFutureReservations()
        {
            return new List<Reservation>() {
                new Reservation() { StartDateTime=DateTime.Now.AddDays(1), EndDateTime=DateTime.Now.AddDays(5) },
                new Reservation() { StartDateTime=DateTime.Now.AddDays(10), EndDateTime=DateTime.Now.AddDays(15)},
                new Reservation() { StartDateTime=DateTime.Now.AddDays(25), EndDateTime=DateTime.Now.AddDays(30)},
            };
        }
    }
}
