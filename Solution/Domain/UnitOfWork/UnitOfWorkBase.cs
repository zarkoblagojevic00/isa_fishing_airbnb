using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace Domain.UnitOfWork
{
    public class UnitOfWorkBase : IUnitOfWorkBase
    {
        protected readonly ISession Session;

        protected UnitOfWorkBase(ISession session)
        {
            Session = session;
        }

        ~UnitOfWorkBase()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            
            //Dispose managed resources
            Session?.Dispose();
        }

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            Session.BeginTransaction(isolationLevel);
        }

        public void Rollback()
        {
            if (Session?.GetCurrentTransaction() != null && Session.GetCurrentTransaction().IsActive)
            {
                Session.GetCurrentTransaction().Rollback();
            }
        }

        public void Commit()
        {
            if (Session?.GetCurrentTransaction() != null && Session.GetCurrentTransaction().IsActive)
            {
                Session.GetCurrentTransaction().Commit();
            }
        }
    }
}
