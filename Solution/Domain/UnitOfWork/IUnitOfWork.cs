using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace Domain.UnitOfWork
{
    public interface IUnitOfWork : IUnitOfWorkBase
    {
        T GetRepository<T>();

        ISession GetSession();
    }
}
