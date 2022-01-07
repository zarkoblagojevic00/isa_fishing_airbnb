using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Mappings;
using Domain.Repositories;
using Domain.UnitOfWork;
using NHibernate;

namespace Services
{
    public class ServiceLocker
    {
        private readonly IUnitOfWork uow;
        private int timeout;
        private readonly int pollingInterval;

        public ServiceLocker(IUnitOfWork uow)
        {
            this.uow = uow;
            timeout = uow.GetRepository<ISystemConfigReadRepository>().GetValue<int>("LockingInterval");
            pollingInterval = uow.GetRepository<ISystemConfigReadRepository>().GetValue<int>("PollingInterval");
        }

        public Service ObtainLockedService(int serviceId)
        {
            while (timeout != 0)
            {
                timeout -= pollingInterval;
                try
                {
                    var service = uow.GetRepository<IServiceReadRepository>()
                        .GetById(serviceId, LockMode.Upgrade);
                    return service;
                }
                catch
                {
                    Thread.Sleep(pollingInterval);
                }
            }

            throw new ADOException();
        }
    }
}
