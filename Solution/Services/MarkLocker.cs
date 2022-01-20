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
    public class MarkLocker
    {
        private readonly IUnitOfWork uow;
        private int timeout;
        private readonly int pollingInterval;

        public MarkLocker(IUnitOfWork uow)
        {
            this.uow = uow;
            timeout = uow.GetRepository<ISystemConfigReadRepository>().GetValue<int>("LockingInterval");
            pollingInterval = uow.GetRepository<ISystemConfigReadRepository>().GetValue<int>("PollingInterval");
        }

        public Mark ObtainLockedMark(int markId)
        {
            while (timeout != 0)
            {
                timeout -= pollingInterval;
                try
                {
                    var mark = uow.GetRepository<IMarkReadRepository>()
                        .GetById(markId, LockMode.UpgradeNoWait);
                    return mark;
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
