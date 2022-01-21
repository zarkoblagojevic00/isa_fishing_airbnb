using Domain.Entities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    [TestCategory("ServiceTests")]
    public class InstructorUnavailabilityPeriodTests
    {
        [TestMethod]
        public void FindsReservationsByOwnerOverlappingWithPeriod()
        {
            List<Reservation> reservations = new List<Reservation>();
            List<Service> services = new List<Service>();

            reservations.Add(new Reservation { ServiceId = 201, StartDateTime = DateTime.Now.AddDays(-6), EndDateTime = DateTime.Now.AddDays(-2), ReservationId = 301 });
            services.Add(new Service { ServiceId = 201, OwnerId = 401, Name = "TestService" });

            UserUnavailabilityValidationService service = new UserUnavailabilityValidationService();

            var extractedReservations = service.FindReservationsForOwnerInPeriod(reservations, services, DateTime.Now.AddDays(-4), DateTime.Now.AddDays(1));

            extractedReservations.Should().NotBeEmpty();
        }

        [TestMethod]
        public void FindsExactReservationByOwnerOverlappingWithPeriod()
        {
            List<Reservation> reservations = new List<Reservation>();
            List<Service> services = new List<Service>();

            reservations.Add(new Reservation { ServiceId = 201, StartDateTime = DateTime.Now.AddDays(-6), EndDateTime = DateTime.Now.AddDays(-2), ReservationId = 301 });
            services.Add(new Service { ServiceId = 201, OwnerId = 401, Name = "TestService" });

            UserUnavailabilityValidationService service = new UserUnavailabilityValidationService();

            var extractedReservations = service.FindReservationsForOwnerInPeriod(reservations, services, DateTime.Now.AddDays(-4), DateTime.Now.AddDays(1));

            extractedReservations.First().ReservationId.Should().Be(301);
        }

        [TestMethod]
        public void DoesNotFindReservationsByOwnerOverlappingWithPeriod()
        {
            List<Reservation> reservations = new List<Reservation>();
            List<Service> services = new List<Service>();

            reservations.Add(new Reservation { ServiceId = 201, StartDateTime = DateTime.Now.AddDays(-6), EndDateTime = DateTime.Now.AddDays(-2), ReservationId = 301 });
            services.Add(new Service { ServiceId = 201, OwnerId = 401, Name = "TestService" });

            UserUnavailabilityValidationService service = new UserUnavailabilityValidationService();

            var extractedReservations = service.FindReservationsForOwnerInPeriod(reservations, services, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2));

            extractedReservations.Should().BeEmpty();
        }

        [TestMethod]
        public void UnavailabilityPeriodCanBeAdded()
        {
            IEnumerable<Reservation> emptyReservations = new List<Reservation>();
            IEnumerable<PromoAction> emptyPromoActions = new List<PromoAction>();
            IEnumerable<UserAvailability> emptyUnavailabilityPeriods = new List<UserAvailability>();

            UserUnavailabilityValidationService service = new UserUnavailabilityValidationService();

            bool isAvailable = service.CanUnavailabilityPeriodBeAdded(emptyReservations, emptyPromoActions, emptyUnavailabilityPeriods);

            isAvailable.Should().Be(true);
        }

        [TestMethod]
        public void UnavailabilityPeriodCanNotBeAdded()
        {
            List<Reservation> reservations = new List<Reservation>();
            reservations.Add(new Reservation { ReservationId = 201, StartDateTime = DateTime.Now.AddDays(-10), EndDateTime = DateTime.Now.AddDays(-5) });

            IEnumerable<PromoAction> emptyPromoActions = new List<PromoAction>();
            IEnumerable<UserAvailability> emptyUnavailabilityPeriods = new List<UserAvailability>();

            UserUnavailabilityValidationService service = new UserUnavailabilityValidationService();

            bool isAvailable = service.CanUnavailabilityPeriodBeAdded(reservations, emptyPromoActions, emptyUnavailabilityPeriods);

            isAvailable.Should().Be(false);
        }
    }
}
