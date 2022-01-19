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
            var futureReservations = UoW.GetRepository<IReservationReadRepository>().GetAll().Where(res => res.StartDateTime > DateTime.Now);
            var futureServiceAvailabilities = services.Select(service => new FutureServiceAvailability(
                service,
                UoW.GetRepository<ICityReadRepository>().GetById(service.CityId).Name,
                new AverageMarkCalculator(UoW).CalculateAverageMark(service.ServiceId),
                GetRelevantDateIntervals(service.ServiceId, userId)
                )
            );

            return futureServiceAvailabilities.Where(ser => ser.IsAvailable(@params));
        }

        private IEnumerable<CalendarItem> GetRelevantDateIntervals(int serviceId, int userId)
        {
            var userReservations = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(x => x.UserId == userId && !x.IsCanceled && x.EndDateTime > DateTime.Now)
                .Select(x => new CalendarItem()
                {
                    StartDateTime = x.StartDateTime,
                    EndDateTime = x.EndDateTime
                });

            var serviceReservations = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId == serviceId && !x.IsCanceled && x.EndDateTime > DateTime.Now)
                .Select(x => new CalendarItem()
                {
                    StartDateTime = x.StartDateTime,
                    EndDateTime = x.EndDateTime
                });

            var serviceQuickActions = UoW.GetRepository<IPromoActionReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId == serviceId && x.EndDateTime > DateTime.Now)
                .Select(x => new CalendarItem()
                {
                    StartDateTime = x.StartDateTime,
                    EndDateTime = x.EndDateTime
                });

            return userReservations.Union(serviceReservations).Union(serviceQuickActions);
        }


    }
}
