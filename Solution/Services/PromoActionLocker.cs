using Domain.Entities;
using Domain.Repositories;
using Domain.UnitOfWork;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    public class PromoActionLocker
    {
        private readonly IUnitOfWork uow;
        private int timeout;
        private readonly int pollingInterval;

        public PromoActionLocker(IUnitOfWork uow)
        {
            this.uow = uow;
            timeout = uow.GetRepository<ISystemConfigReadRepository>().GetValue<int>("LockingInterval");
            pollingInterval = uow.GetRepository<ISystemConfigReadRepository>().GetValue<int>("PollingInterval");
        }

        public PromoAction ObtainLockedPromoAction(int promoActionId)
        {
            while (timeout != 0)
            {
                timeout -= pollingInterval;
                try
                {
                    var promoAction = uow.GetRepository<IPromoActionReadRepository>()
                        .GetById(promoActionId, LockMode.UpgradeNoWait);
                    return promoAction;
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
