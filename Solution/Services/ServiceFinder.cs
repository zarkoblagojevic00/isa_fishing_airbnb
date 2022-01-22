using Domain.Entities;
using Domain.Entities.Helpers;
using Domain.Repositories;
using Domain.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ServiceFinder
    {
        private ServiceType serviceType;
        private IUnitOfWork UoW;

        public ServiceFinder(ServiceType serviceType, IUnitOfWork uoW)
        {
            this.serviceType = serviceType;
            UoW = uoW;
        }

        public IEnumerable<FutureServiceAvailability> FindServices(ServiceSearchParameters @params, int userId)
        {
            var services = UoW.GetRepository<IServiceReadRepository>().GetAll().Where(service => service.ServiceType == serviceType);
            var futureServiceAvailabilities = services.Select(service => new FutureServiceAvailability(
                service,
                UoW.GetRepository<ICityReadRepository>().GetById(service.CityId).Name,
                new AverageMarkCalculator(UoW).CalculateAverageMark(service.ServiceId),
                GetUnavailableTimeIntervalsForNewReservation(service.ServiceId, userId)
                )
            );

            return futureServiceAvailabilities.Where(ser => ser.IsAvailable(@params));
        }

        public IEnumerable<PromoAction> GetAvailablePromoActions(int userId)
        {
            var potentiallyAvailablePromoActions = UoW.GetRepository<IPromoActionReadRepository>()
                .GetAll()
                .Where(p => p.StartDateTime > DateTime.Now.AddDays(1).Date && !p.IsTaken);

            if (userId < 0)
                return potentiallyAvailablePromoActions;

            return potentiallyAvailablePromoActions.Where(p => IsUserAvailableForPromoAction(p, userId));
        }

        private bool IsUserAvailableForPromoAction(PromoAction promo, int userId)
        {
            IEnumerable<CalendarItem> userReservations = GetUserUnavailableTimeIntervals(userId);
            IEnumerable<CalendarItem> serviceReservations = GetServiceUnavailableTimeIntervals(promo.ServiceId, userId);

            return !promo.AnyOverlapping(userReservations.Union(serviceReservations));
        }

        private IEnumerable<CalendarItem> GetUnavailableTimeIntervalsForNewReservation(int serviceId, int userId)
        {
            IEnumerable<CalendarItem> userReservations = GetUserUnavailableTimeIntervals(userId);
            IEnumerable<CalendarItem> serviceReservations = GetServiceUnavailableTimeIntervals(serviceId, userId);
            IEnumerable<CalendarItem> servicePromoActions = GetServicePromoActionsUnavailableTimeIntervals(serviceId);

            return userReservations.Union(serviceReservations).Union(servicePromoActions);
        }

        private IEnumerable<CalendarItem> GetUserUnavailableTimeIntervals(int userId)
        {
            return UoW.GetRepository<IReservationReadRepository>()
                            .GetAll()
                            .Where(x => x.UserId == userId && !x.IsCanceled && x.EndDateTime > DateTime.Now)
                            .Select(x => new CalendarItem()
                            {
                                StartDateTime = x.StartDateTime,
                                EndDateTime = x.EndDateTime
                            });
        }
        
        private IEnumerable<CalendarItem> GetServiceUnavailableTimeIntervals(int serviceId, int userId)
        {
            //3.19 klijent moze samo jednom da rezervise isti entitet u isto vreme
            return UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId == serviceId && (!x.IsCanceled || x.UserId == userId) && x.EndDateTime > DateTime.Now)
                .Select(x => new CalendarItem()
                {
                    StartDateTime = x.StartDateTime,
                    EndDateTime = x.EndDateTime
                });
        }

        private IEnumerable<CalendarItem> GetServicePromoActionsUnavailableTimeIntervals(int serviceId)
        {
            return UoW.GetRepository<IPromoActionReadRepository>()
                            .GetAll()
                            .Where(x => x.ServiceId == serviceId && x.EndDateTime > DateTime.Now)
                            .Select(x => new CalendarItem()
                            {
                                StartDateTime = x.StartDateTime,
                                EndDateTime = x.EndDateTime
                            });
        }

    }
}
