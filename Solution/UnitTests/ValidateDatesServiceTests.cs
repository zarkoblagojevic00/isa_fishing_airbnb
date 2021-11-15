using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Abstractions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Validators;

namespace UnitTests
{
    [TestClass]
    [TestCategory("ServiceTests")]
    public class ValidateDatesServiceTests
    {
        [TestMethod]
        public void NonOverlappingDates()
        {
            var dateToCheck = new CalendarItem()
            {
                StartDateTime = DateTime.Now,
                EndDateTime = DateTime.Now.AddDays(3)
            };

            var otherDate = new CalendarItem()
            {
                StartDateTime = DateTime.Now.AddDays(4),
                EndDateTime = DateTime.Now.AddDays(7)
            };

            var validationResponse = new ValidateReservationDatesService().CalendarItemOverlap(dateToCheck, otherDate);

            validationResponse.Should().Be(false);
        }

        [TestMethod]
        public void OverlappingDates()
        {
            var dateToCheck = new CalendarItem()
            {
                StartDateTime = DateTime.Now,
                EndDateTime = DateTime.Now.AddDays(2)
            };

            var otherDate = new CalendarItem()
            {
                StartDateTime = DateTime.Now.AddDays(1),
                EndDateTime = DateTime.Now.AddDays(4)
            };

            var validationResponse = new ValidateReservationDatesService().CalendarItemOverlap(dateToCheck, otherDate);

            validationResponse.Should().Be(true);
        }

        [TestMethod]
        public void NonOverlappingDatesArray()
        {
            var dateToCheck = new CalendarItem()
            {
                StartDateTime = DateTime.Now.AddDays(5),
                EndDateTime = DateTime.Now.AddDays(7)
            };

            var listOfDates = new List<CalendarItem>()
            {
                new CalendarItem()
                {
                    StartDateTime = DateTime.Now,
                    EndDateTime = DateTime.Now.AddDays(1)
                },
                new CalendarItem()
                {
                    StartDateTime = DateTime.Now.AddDays(3),
                    EndDateTime = DateTime.Now.AddDays(4)
                },
                new CalendarItem()
                {
                    StartDateTime = DateTime.Now.AddDays(8),
                    EndDateTime = DateTime.Now.AddDays(10)
                }
            };

            var validationResponse = new ValidateReservationDatesService().CalendarItemOverlapsWithAny(dateToCheck, listOfDates);

            validationResponse.Should().Be(false);
        }

        [TestMethod]
        public void OverlappingDatesArray()
        {
            var dateToCheck = new CalendarItem()
            {
                StartDateTime = DateTime.Now.AddDays(3),
                EndDateTime = DateTime.Now.AddDays(5)
            };

            var listOfDates = new List<CalendarItem>()
            {
                new CalendarItem()
                {
                    StartDateTime = DateTime.Now,
                    EndDateTime = DateTime.Now.AddDays(3),
                },
                new CalendarItem()
                {
                    StartDateTime = DateTime.Now.AddDays(4),
                    EndDateTime = DateTime.Now.AddDays(5)
                },
                new CalendarItem()
                {
                    StartDateTime = DateTime.Now.AddDays(6),
                    EndDateTime = DateTime.Now.AddDays(8)
                }
            };

            var validationResponse = new ValidateReservationDatesService().CalendarItemOverlapsWithAny(dateToCheck, listOfDates);

            validationResponse.Should().Be(true);
        }

        [TestMethod]
        public void SuspiciousTest()
        {
            var dateToCheck = new CalendarItem()
            {
                StartDateTime = DateTime.Now.AddDays(3),
                EndDateTime = DateTime.Now.AddDays(6)
            };

            var listOfDates = new List<CalendarItem>()
            {
                new CalendarItem()
                {
                    StartDateTime = DateTime.Now.AddDays(1),
                    EndDateTime = DateTime.Now.AddDays(5),
                },
                new CalendarItem()
                {
                    StartDateTime = DateTime.Now.AddDays(6),
                    EndDateTime = DateTime.Now.AddDays(9)
                }
            };

            var validationResponse = new ValidateReservationDatesService().CalendarItemOverlapsWithAny(dateToCheck, listOfDates);

            validationResponse.Should().Be(true);
        }
    }
}
