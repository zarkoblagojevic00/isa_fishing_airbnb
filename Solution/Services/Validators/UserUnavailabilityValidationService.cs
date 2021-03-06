using Domain.Entities;
using Domain.Repositories;
using Domain.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Validators
{
    public class UserUnavailabilityValidationService
    {
        private readonly IUnitOfWork UoW;
        public UserUnavailabilityValidationService(IUnitOfWork uow)
        {
            UoW = uow;
        }

        public UserUnavailabilityValidationService() { }


        public IEnumerable<Reservation> FindReservationsForOwnerInPeriod(IEnumerable<Reservation> allReservations, IEnumerable<Service> ownerServices, DateTime periodStart, DateTime periodEnd)
        {
            var reservations = allReservations.Where(res => (res.StartDateTime <= periodEnd && res.EndDateTime >= periodStart));

            var userReservations = reservations
                .Join(ownerServices, r => r.ServiceId, a => a.ServiceId, (r, a) => new { r, a })
                .Select(reservation => new Reservation
                {
                    ReservationId = reservation.r.ReservationId,
                    StartDateTime = reservation.r.StartDateTime,
                    EndDateTime = reservation.r.EndDateTime,
                    ServiceId = reservation.r.ServiceId,
                });

            var distinctReservations = userReservations
                .GroupBy(res => res.ReservationId)
                .Select(res => res.First());

            return distinctReservations;

        }

        public IEnumerable<UserAvailability> GetAllUnavailabilityPeriods(int ownerId, DateTime periodStart, DateTime periodEnd)
        {
            var instructorAvailabilityPeriods = UoW.GetRepository<IUserAvailabilityReadRepository>().GetAll()
                .Where(x => x.UserId == ownerId)
                .Where(x => x.PeriodStart <= periodEnd && x.PeriodEnd >= periodStart);

            return instructorAvailabilityPeriods;

        }

        public IEnumerable<PromoAction> GetAllPromoActionsForOwnerInPeriod(int ownerId, DateTime periodStart, DateTime periodEnd)
        {
            var services = UoW.GetRepository<IServiceReadRepository>()
               .GetAll()
               .Where(x => x.OwnerId == ownerId);

            var promoActions = UoW.GetRepository<IPromoActionReadRepository>()
                .GetAll()
                .Where(act => (act.StartDateTime <= periodEnd && act.EndDateTime >= periodStart));

            var userActions = promoActions
                .Join(services, r => r.ServiceId, a => a.ServiceId, (r, a) => new { r, a })
                .Select(action => new PromoAction
                {
                    PromoActionId = action.r.PromoActionId,
                    StartDateTime = action.r.StartDateTime,
                    EndDateTime = action.r.EndDateTime,
                    ServiceId = action.r.ServiceId,
                });

            var distinctActions = userActions
                .GroupBy(res => res.PromoActionId)
                .Select(res => res.First());

            return distinctActions;

        }

        public bool CanUnavailabilityPeriodBeAdded(IEnumerable<Reservation> reservations, IEnumerable<PromoAction> promoActions, IEnumerable<UserAvailability> userAvailabilities)
        {
            if (reservations.Any() || promoActions.Any() || userAvailabilities.Any())
            {
                return false;
            }
            return true;
        }

        public bool IsInstructorAvailable(int ownerId, DateTime periodStart, DateTime periodEnd)
        {
            var userUnavailabilities = GetAllUnavailabilityPeriods(ownerId, periodStart, periodEnd);
            if (userUnavailabilities.Any())
            {
                return false;
            }
            return true;
        }
    }
}
