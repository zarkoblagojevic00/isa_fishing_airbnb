using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;

namespace UnitTests
{
    [TestClass]
    [TestCategory("ServiceTests")]
    public class BusinessReportServiceTests
    {
        [TestMethod]
        public void OneReservationNoMarksTest()
        {
            var reservations = new List<Reservation>
            {
                new Reservation()
                {
                    StartDateTime = DateTime.Now.Subtract(new TimeSpan(3,0,0,0)),
                    EndDateTime = DateTime.Now,
                    Price = 10
                }
            };

            var summary = new BusinessReportService().GetBusinessSummary(reservations, new List<Mark>(), 1);

            summary.AverageMark.Should().Be(0.0);

            summary.Weekly.Should().HaveCount(1);
            summary.Weekly.First().MoneyMade.Should().BeInRange(30, 31);

            summary.Monthly.Should().HaveCount(1);
            summary.Monthly.First().MoneyMade.Should().BeInRange(30, 31);

            summary.Yearly.Should().HaveCount(1);
            summary.Yearly.First().MoneyMade.Should().BeInRange(30, 31);
        }

        [TestMethod]
        public void MoreReservationsNoMarksTest()
        {
            var reservations = new List<Reservation>()
            {
                new Reservation()
                {
                    StartDateTime = DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0)),
                    EndDateTime = DateTime.Now,
                    Price = 10
                },
                new Reservation()
                {
                    StartDateTime = DateTime.Now.Subtract(new TimeSpan(10, 0, 0, 0)),
                    EndDateTime = DateTime.Now.Subtract(new TimeSpan(9, 0, 0, 0)),
                    Price = 10
                },
                new Reservation()
                {
                    StartDateTime = DateTime.Now.Subtract(new TimeSpan(13, 0, 0, 0)),
                    EndDateTime = DateTime.Now.Subtract(new TimeSpan(11, 0, 0, 0)),
                    Price = 10
                }
            };

            var summary = new BusinessReportService().GetBusinessSummary(reservations, new List<Mark>(), 2);

            summary.AverageMark.Should().Be(0.0);

            summary.Weekly.Should().HaveCount(2);
            summary.Monthly.Should().HaveCount(2);
            summary.Yearly.Should().HaveCount(2);

            var weeklyFirst = summary.Weekly.First();
            var weeklyLast = summary.Weekly.Last();

            weeklyFirst.NumberOfReservations.Should().Be(1);
            weeklyFirst.MoneyMade.Should().BeInRange(30, 31);

            weeklyLast.NumberOfReservations.Should().Be(2);
            weeklyLast.MoneyMade.Should().BeInRange(30, 31);

            var monthlyFirst = summary.Monthly.First();
            var monthlyLast = summary.Monthly.Last();

            monthlyFirst.NumberOfReservations.Should().Be(3);
            monthlyFirst.MoneyMade.Should().BeInRange(60, 61);
            monthlyLast.NumberOfReservations.Should().Be(0);
            monthlyLast.MoneyMade.Should().BeInRange(0, 1);

            var yearlyFirst = summary.Yearly.First();
            var yearlyLast = summary.Yearly.Last();

            yearlyFirst.NumberOfReservations.Should().Be(3);
            yearlyFirst.MoneyMade.Should().BeInRange(60, 61);
            yearlyLast.NumberOfReservations.Should().Be(0);
            yearlyLast.MoneyMade.Should().BeInRange(0, 1);
        }

        [TestMethod]
        public void OneReservationWithOneMark()
        {
            var reservations = new List<Reservation>()
            {
                new Reservation()
                {
                    StartDateTime = DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0)),
                    EndDateTime = DateTime.Now,
                    Price = 10,
                    MarkId = 1
                }
            };

            var marks = new List<Mark>()
            {
                new Mark()
                {
                    GivenMark = 5,
                    MarkId = 1
                }
            };

            var summary = new BusinessReportService().GetBusinessSummary(reservations, marks, 1);

            summary.AverageMark.Should().BeInRange(4.99, 5.1);
        }

        [TestMethod]
        public void MoreReservationsWithMoreMarks()
        {
            var reservations = new List<Reservation>()
            {
                new Reservation()
                {
                    StartDateTime = DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0)),
                    EndDateTime = DateTime.Now,
                    Price = 10,
                    MarkId = 1
                },
                new Reservation()
                {
                    StartDateTime = DateTime.Now.Subtract(new TimeSpan(10, 0, 0, 0)),
                    EndDateTime = DateTime.Now.Subtract(new TimeSpan(9, 0, 0, 0)),
                    Price = 10,
                    MarkId = 2
                },
                new Reservation()
                {
                    StartDateTime = DateTime.Now.Subtract(new TimeSpan(13, 0, 0, 0)),
                    EndDateTime = DateTime.Now.Subtract(new TimeSpan(11, 0, 0, 0)),
                    Price = 10,
                    MarkId = 3
                }
            };

            var marks = new List<Mark>()
            {
                new Mark()
                {
                    GivenMark = 5,
                    MarkId = 1
                },
                new Mark()
                {
                    GivenMark = 4,
                    MarkId = 2
                },
                new Mark()
                {
                    GivenMark = 3,
                    MarkId = 3
                }
            };

            var summary = new BusinessReportService().GetBusinessSummary(reservations, marks, 2);

            summary.AverageMark.Should().BeInRange(3.99, 4.01);
        }
    }
}
