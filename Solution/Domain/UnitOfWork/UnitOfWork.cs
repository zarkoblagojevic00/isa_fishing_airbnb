using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using NHibernate;

namespace Domain.UnitOfWork
{
    public class UnitOfWork : UnitOfWorkBase, IUnitOfWork
    {
        private readonly IComponentContext Context;
        public UnitOfWork(IComponentContext context, ISession session) : base(session)
        {
            Context = context;
        }

        public T GetRepository<T>()
        {
            return Context.Resolve<T>(GetBaseSession());
        }

        private TypedParameter GetBaseSession()
        {
            return new TypedParameter(typeof(ISession), Session);
        }
    }
}
