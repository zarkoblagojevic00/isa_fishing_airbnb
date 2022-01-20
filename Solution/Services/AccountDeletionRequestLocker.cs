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
    public class AccountDeletionRequestLocker
    {
        private readonly IUnitOfWork uow;
        private int timeout;
        private readonly int pollingInterval;

        public AccountDeletionRequestLocker(IUnitOfWork uow)
        {
            this.uow = uow;
            timeout = uow.GetRepository<ISystemConfigReadRepository>().GetValue<int>("LockingInterval");
            pollingInterval = uow.GetRepository<ISystemConfigReadRepository>().GetValue<int>("PollingInterval");
        }

        public AccountDeletionRequest ObtainLockedAccountDeletionRequest(int userId)
        {
            while (timeout != 0)
            {
                timeout -= pollingInterval;
                try
                {
                    var request = uow.GetRepository<IAccountDeletionRequestReadRepository>()
                        .GetById(userId, LockMode.UpgradeNoWait);
                    return request;
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
