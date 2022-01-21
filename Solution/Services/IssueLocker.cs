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
    public class IssueLocker
    {
        private readonly IUnitOfWork uow;
        private int timeout;
        private readonly int pollingInterval;

        public IssueLocker(IUnitOfWork uow)
        {
            this.uow = uow;
            timeout = uow.GetRepository<ISystemConfigReadRepository>().GetValue<int>("LockingInterval");
            pollingInterval = uow.GetRepository<ISystemConfigReadRepository>().GetValue<int>("PollingInterval");
        }

        public Issue ObtainLockedIssue(int issueId)
        {
            while (timeout != 0)
            {
                timeout -= pollingInterval;
                try
                {
                    var issue = uow.GetRepository<IIssueReadRepository>()
                        .GetById(issueId, LockMode.UpgradeNoWait);
                    return issue;
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
