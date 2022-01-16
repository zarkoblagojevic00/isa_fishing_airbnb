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

        public IEnumerable<FutureServiceAvailability> FindServices(ServiceSearchParameters @params)
        {
            var services = UoW.GetRepository<IServiceReadRepository>().GetAll().Where(service => service.ServiceType == serviceType);
            var futureReservations = UoW.GetRepository<IReservationReadRepository>().GetAll().Where(res => res.StartDateTime > DateTime.Now);
            var futureServiceAvailabilities = services.Select(service => new FutureServiceAvailability(
                service,
                UoW.GetRepository<ICityReadRepository>().GetById(service.CityId).Name,
                new AverageMarkCalculator(UoW).CalculateAverageMark(service.ServiceId),
                futureReservations.Where(res => res.ServiceId == service.ServiceId)
                )
            );

            return futureServiceAvailabilities.Where(ser => ser.IsAvailable(@params));
        }

        
    }
}
